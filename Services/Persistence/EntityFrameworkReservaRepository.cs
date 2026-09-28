using Microsoft.EntityFrameworkCore;
using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Models.States;

namespace SistemaHotelaria.Services.Persistence;

public class EntityFrameworkReservaRepository : IReservaRepository
{
    private readonly ApplicationDbContext _context;

    public EntityFrameworkReservaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Reserva> ObterTodas()
    {
        return CarregarConsulta()
            .AsNoTracking()
            .AsEnumerable()
            .Select(MapearParaReserva)
            .ToList();
    }

    public Reserva? ObterPorId(string id)
    {
        var entity = CarregarConsulta().FirstOrDefault(r => r.Id == id);
        return entity == null ? null : MapearParaReserva(entity);
    }

    public void Adicionar(Reserva reserva)
    {
        var entity = MapearParaEntity(reserva);
        _context.Reservas.Add(entity);
        _context.SaveChanges();
    }

    public void Atualizar(Reserva reserva)
    {
        var entity = _context.Reservas
            .Include(r => r.Pagamento)
            .Include(r => r.Eventos)
                .ThenInclude(e => e.Acoes)
            .FirstOrDefault(r => r.Id == reserva.Id);

        if (entity == null)
            return;

        entity.StatusReserva = reserva.Status;
        entity.ValorTotal = reserva.ValorTotal;

        if (entity.Pagamento == null)
        {
            entity.Pagamento = new PagamentoEntity
            {
                ReservaId = entity.Id,
                Metodo = reserva.MetodoPagamento,
                TransacaoId = reserva.PagamentoTransacaoId ?? "",
                Comprovante = reserva.PagamentoComprovante ?? ""
            };
        }
        else
        {
            entity.Pagamento.Metodo = reserva.MetodoPagamento;
            entity.Pagamento.TransacaoId = reserva.PagamentoTransacaoId ?? "";
            entity.Pagamento.Comprovante = reserva.PagamentoComprovante ?? "";
        }

        SincronizarEventos(entity, reserva);
        _context.SaveChanges();
    }

    public int ContarConflitos(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        return _context.Reservas
            .Include(r => r.TipoQuarto)
            .Count(r =>
                r.TipoQuarto != null && r.TipoQuarto.Nome == tipoQuarto &&
                r.StatusReserva != "Check-out" && r.StatusReserva != "Cancelada" &&
                r.DataEntrada < dataSaida &&
                r.DataSaida > dataEntrada);
    }

    private IQueryable<ReservaEntity> CarregarConsulta() =>
        _context.Reservas
            .Include(r => r.Hospede)
            .Include(r => r.TipoQuarto)
            .Include(r => r.Pagamento)
            .Include(r => r.Eventos)
                .ThenInclude(e => e.Acoes)
            .Include(r => r.Pacote)
                .ThenInclude(p => p!.Refeicoes)
            .Include(r => r.Pacote)
                .ThenInclude(p => p!.ServicosAdicionais);

    private static Reserva MapearParaReserva(ReservaEntity entity)
    {
        var reserva = new Reserva
        {
            Id = entity.Id,
            HospedeNome = entity.Hospede?.Nome ?? string.Empty,
            TipoQuarto = entity.TipoQuarto?.Nome ?? string.Empty,
            DataEntrada = entity.DataEntrada,
            DataSaida = entity.DataSaida,
            ValorTotal = entity.ValorTotal,
            MetodoPagamento = entity.Pagamento?.Metodo ?? "Pix",
            PagamentoTransacaoId = entity.Pagamento?.TransacaoId ?? "",
            PagamentoComprovante = entity.Pagamento?.Comprovante ?? ""
        };

        IEstadoReserva estado = entity.StatusReserva switch
        {
            "Check-in" => new EstadoCheckIn(),
            "Check-out" => new EstadoCheckOut(),
            "Cancelada" => new EstadoCancelada(),
            _ => new EstadoConfirmada()
        };
        reserva.DefinirEstadoInicial(estado);

        reserva.Eventos = entity.Eventos
            .OrderBy(e => e.Quando)
            .Select(e => new EventoReserva
            {
                Status = e.Status,
                Quando = e.Quando,
                Acoes = e.Acoes.Select(a => new AcaoServico
                {
                    Servico = a.Servico,
                    Icone = a.Icone,
                    Descricao = a.Descricao
                }).ToList()
            }).ToList();

        if (entity.Pacote != null)
        {
            reserva.Pacote = new PacoteHospedagem
            {
                Nome = entity.Pacote.Nome,
                Descricao = entity.Pacote.Descricao,
                DescontoPercentual = entity.Pacote.DescontoPercentual,
                Refeicoes = entity.Pacote.Refeicoes.Select(r => new Refeicao
                {
                    Nome = r.Nome,
                    Descricao = r.Descricao,
                    Horario = r.Horario,
                    Preco = r.Preco
                }).ToList(),
                Servicos = entity.Pacote.ServicosAdicionais.Select(s => new ServicoAdicional
                {
                    Nome = s.Nome,
                    Descricao = s.Descricao,
                    Preco = s.Preco
                }).ToList()
            };
        }

        return reserva;
    }

    private ReservaEntity MapearParaEntity(Reserva reserva)
    {
        var hospede = ObterOuCriarHospede(reserva.HospedeNome);
        var tipoQuarto = ObterOuCriarTipoQuarto(reserva.TipoQuarto);

        var entity = new ReservaEntity
        {
            Id = reserva.Id,
            DataEntrada = reserva.DataEntrada,
            DataSaida = reserva.DataSaida,
            ValorTotal = reserva.ValorTotal,
            StatusReserva = reserva.Status,
            HospedeId = hospede.Id,
            Hospede = hospede,
            TipoQuartoId = tipoQuarto.Id,
            TipoQuarto = tipoQuarto,
            Pagamento = new PagamentoEntity
            {
                Metodo = string.IsNullOrEmpty(reserva.MetodoPagamento) ? "Pix" : reserva.MetodoPagamento,
                TransacaoId = reserva.PagamentoTransacaoId ?? "",
                Comprovante = reserva.PagamentoComprovante ?? "",
                ReservaId = reserva.Id
            }
        };

        if (reserva.Eventos != null)
        {
            foreach (var ev in reserva.Eventos)
            {
                entity.Eventos.Add(MapearEvento(ev, reserva.Id));
            }
        }

        if (reserva.Pacote != null)
        {
            entity.Pacote = MontarPacoteEntity(reserva.Pacote);
            entity.PacoteId = entity.Pacote.Id;
        }

        return entity;
    }

    private void SincronizarEventos(ReservaEntity entity, Reserva reserva)
    {
        var existentes = entity.Eventos.Count;
        if (reserva.Eventos == null || reserva.Eventos.Count <= existentes)
            return;

        foreach (var ev in reserva.Eventos.Skip(existentes))
            entity.Eventos.Add(MapearEvento(ev, entity.Id));
    }

    private static EventoReservaEntity MapearEvento(EventoReserva ev, string reservaId) =>
        new()
        {
            ReservaId = reservaId,
            Status = ev.Status,
            Quando = ev.Quando,
            Acoes = ev.Acoes.Select(a => new AcaoEventoEntity
            {
                Servico = a.Servico,
                Icone = a.Icone,
                Descricao = a.Descricao
            }).ToList()
        };

    private HospedeEntity ObterOuCriarHospede(string nome)
    {
        var existente = _context.Hospedes.FirstOrDefault(h => h.Nome == nome);
        if (existente != null)
            return existente;

        var novo = new HospedeEntity { Nome = nome };
        _context.Hospedes.Add(novo);
        return novo;
    }

    private TipoQuartoEntity ObterOuCriarTipoQuarto(string nome)
    {
        var existente = _context.TiposQuarto.FirstOrDefault(t => t.Nome == nome);
        if (existente != null)
            return existente;

        var novo = new TipoQuartoEntity { Nome = nome };
        _context.TiposQuarto.Add(novo);
        return novo;
    }

    private PacoteEntity MontarPacoteEntity(PacoteHospedagem pacote)
    {
        var entity = new PacoteEntity
        {
            Nome = pacote.Nome,
            Descricao = pacote.Descricao,
            DescontoPercentual = pacote.DescontoPercentual
        };

        foreach (var r in pacote.Refeicoes)
        {
            var refeicao = _context.Refeicoes.FirstOrDefault(x =>
                x.Nome == r.Nome && x.Horario == r.Horario && x.Preco == r.Preco);

            if (refeicao == null)
            {
                refeicao = new RefeicaoEntity
                {
                    Nome = r.Nome,
                    Descricao = r.Descricao,
                    Horario = r.Horario,
                    Preco = r.Preco
                };
                _context.Refeicoes.Add(refeicao);
            }

            entity.Refeicoes.Add(refeicao);
        }

        foreach (var s in pacote.Servicos)
        {
            var servico = _context.ServicosAdicionais.FirstOrDefault(x =>
                x.Nome == s.Nome && x.Preco == s.Preco);

            if (servico == null)
            {
                servico = new ServicoAdicionalEntity
                {
                    Nome = s.Nome,
                    Descricao = s.Descricao,
                    Preco = s.Preco
                };
                _context.ServicosAdicionais.Add(servico);
            }

            entity.ServicosAdicionais.Add(servico);
        }

        return entity;
    }
}
