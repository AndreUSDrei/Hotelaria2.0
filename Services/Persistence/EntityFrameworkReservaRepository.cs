using Microsoft.EntityFrameworkCore;
using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;
using SistemaHotelaria.Models.States;
using System.Text.Json;

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
        return _context.Reservas
            .Include(r => r.Hospede)
            .Include(r => r.TipoQuarto)
            .Include(r => r.Pacote)
                .ThenInclude(p => p.Refeicoes)
            .Include(r => r.Pacote)
                .ThenInclude(p => p.ServicosAdicionais)
            .AsNoTracking()
            .Select(e => MapearParaReserva(e))
            .ToList();
    }

    public Reserva? ObterPorId(string id)
    {
        var entity = _context.Reservas
            .Include(r => r.Hospede)
            .Include(r => r.TipoQuarto)
            .Include(r => r.Pacote)
                .ThenInclude(p => p.Refeicoes)
            .Include(r => r.Pacote)
                .ThenInclude(p => p.ServicosAdicionais)
            .FirstOrDefault(r => r.Id == id);
            
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
        var entity = _context.Reservas.Find(reserva.Id);
        if (entity == null)
            return;

        entity.StatusReserva = reserva.Status;
        entity.MetodoPagamento = reserva.MetodoPagamento;
        entity.PagamentoTransacaoId = reserva.PagamentoTransacaoId;
        entity.PagamentoComprovante = reserva.PagamentoComprovante;
        
        if (reserva.Eventos != null && reserva.Eventos.Any())
        {
            entity.EventosReserva = JsonSerializer.Serialize(reserva.Eventos);
        }

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
            MetodoPagamento = entity.MetodoPagamento,
            PagamentoTransacaoId = entity.PagamentoTransacaoId,
            PagamentoComprovante = entity.PagamentoComprovante
        };

        // Forçar o estado inicial baseado na string gravada
        IEstadoReserva estado = entity.StatusReserva switch
        {
            "Check-in" => new EstadoCheckIn(),
            "Check-out" => new EstadoCheckOut(),
            "Cancelada" => new EstadoCancelada(),
            _ => new EstadoConfirmada()
        };
        reserva.DefinirEstadoInicial(estado);

        if (!string.IsNullOrEmpty(entity.EventosReserva))
        {
            try 
            {
                var eventos = JsonSerializer.Deserialize<List<EventoReserva>>(entity.EventosReserva);
                if (eventos != null) reserva.Eventos = eventos;
            }
            catch { }
        }

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

    private static ReservaEntity MapearParaEntity(Reserva reserva)
    {
        var entity = new ReservaEntity
        {
            Id = reserva.Id,
            DataEntrada = reserva.DataEntrada,
            DataSaida = reserva.DataSaida,
            ValorTotal = reserva.ValorTotal,
            MetodoPagamento = string.IsNullOrEmpty(reserva.MetodoPagamento) ? "Pix" : reserva.MetodoPagamento,
            PagamentoTransacaoId = reserva.PagamentoTransacaoId ?? "",
            PagamentoComprovante = reserva.PagamentoComprovante ?? "",
            StatusReserva = reserva.Status,
            Hospede = new HospedeEntity { Nome = reserva.HospedeNome },
            TipoQuarto = new TipoQuartoEntity { Nome = reserva.TipoQuarto }
        };

        if (reserva.Eventos != null && reserva.Eventos.Any())
        {
            entity.EventosReserva = JsonSerializer.Serialize(reserva.Eventos);
        }

        if (reserva.Pacote != null)
        {
            entity.Pacote = new PacoteEntity
            {
                Nome = reserva.Pacote.Nome,
                Descricao = reserva.Pacote.Descricao,
                DescontoPercentual = reserva.Pacote.DescontoPercentual,
                Refeicoes = reserva.Pacote.Refeicoes.Select(r => new RefeicaoEntity
                {
                    Nome = r.Nome,
                    Descricao = r.Descricao,
                    Horario = r.Horario,
                    Preco = r.Preco
                }).ToList(),
                ServicosAdicionais = reserva.Pacote.Servicos.Select(s => new ServicoAdicionalEntity
                {
                    Nome = s.Nome,
                    Descricao = s.Descricao,
                    Preco = s.Preco
                }).ToList()
            };
        }

        return entity;
    }
}
