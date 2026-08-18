using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Services.Persistence;

public class EntityFrameworkReservaRepository : IReservaRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly ApplicationDbContext _context;

    public EntityFrameworkReservaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Reserva> ObterTodas()
    {
        return _context.Reservas
            .AsNoTracking()
            .Select(e => MapearParaReserva(e))
            .ToList();
    }

    public Reserva? ObterPorId(string id)
    {
        var entity = _context.Reservas.Find(id);
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

        entity.CheckInRealizado = reserva.CheckInRealizado;
        entity.CheckOutRealizado = reserva.CheckOutRealizado;
        entity.ValorTotal = reserva.ValorTotal;
        entity.MetodoPagamento = reserva.MetodoPagamento;
        entity.PagamentoTransacaoId = reserva.PagamentoTransacaoId;
        entity.PagamentoComprovante = reserva.PagamentoComprovante;
        entity.StatusReserva = reserva.Status;
        entity.EventosReserva = SerializarEventos(reserva.Eventos);
        _context.SaveChanges();
    }

    public int ContarConflitos(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        return _context.Reservas.Count(r =>
            r.TipoQuarto == tipoQuarto &&
            !r.CheckOutRealizado &&
            r.DataEntrada < dataSaida &&
            r.DataSaida > dataEntrada);
    }

    private static Reserva MapearParaReserva(ReservaEntity entity)
    {
        var status = string.IsNullOrWhiteSpace(entity.StatusReserva)
            ? DerivarStatus(entity.CheckInRealizado, entity.CheckOutRealizado)
            : entity.StatusReserva;

        return new Reserva
        {
            Id = entity.Id,
            HospedeNome = entity.HospedeNome,
            TipoQuarto = entity.TipoQuarto,
            DataEntrada = entity.DataEntrada,
            DataSaida = entity.DataSaida,
            ValorTotal = entity.ValorTotal,
            CheckInRealizado = entity.CheckInRealizado,
            CheckOutRealizado = entity.CheckOutRealizado,
            MetodoPagamento = entity.MetodoPagamento,
            PagamentoTransacaoId = entity.PagamentoTransacaoId ?? string.Empty,
            PagamentoComprovante = entity.PagamentoComprovante ?? string.Empty,
            Status = status,
            Eventos = DesserializarEventos(entity.EventosReserva),
            Pacote = new PacoteHospedagem
            {
                Nome = entity.PacoteNome,
                Descricao = entity.PacoteDescricao,
                DescontoPercentual = entity.PacoteDescontoPercentual
            }
        };
    }

    private static ReservaEntity MapearParaEntity(Reserva reserva)
    {
        return new ReservaEntity
        {
            Id = reserva.Id,
            HospedeNome = reserva.HospedeNome,
            TipoQuarto = reserva.TipoQuarto,
            DataEntrada = reserva.DataEntrada,
            DataSaida = reserva.DataSaida,
            ValorTotal = reserva.ValorTotal,
            CheckInRealizado = reserva.CheckInRealizado,
            CheckOutRealizado = reserva.CheckOutRealizado,
            MetodoPagamento = reserva.MetodoPagamento,
            PagamentoTransacaoId = reserva.PagamentoTransacaoId,
            PagamentoComprovante = reserva.PagamentoComprovante,
            StatusReserva = reserva.Status,
            EventosReserva = SerializarEventos(reserva.Eventos),
            PacoteNome = reserva.Pacote?.Nome ?? string.Empty,
            PacoteDescricao = reserva.Pacote?.Descricao ?? string.Empty,
            PacoteDescontoPercentual = reserva.Pacote?.DescontoPercentual ?? 0m
        };
    }

    private static string DerivarStatus(bool checkIn, bool checkOut) =>
        checkOut ? "Check-out" : checkIn ? "Check-in" : "Confirmada";

    private static string SerializarEventos(List<EventoReserva> eventos) =>
        JsonSerializer.Serialize(eventos ?? new List<EventoReserva>(), JsonOptions);

    private static List<EventoReserva> DesserializarEventos(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<EventoReserva>();

        try
        {
            return JsonSerializer.Deserialize<List<EventoReserva>>(json, JsonOptions) ?? new List<EventoReserva>();
        }
        catch
        {
            return new List<EventoReserva>();
        }
    }
}
