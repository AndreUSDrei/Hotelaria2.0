namespace SistemaHotelaria.Services.Memento;

/// <summary>
/// Caretaker: guarda e devolve mementos sem inspecionar o estado da reserva.
/// </summary>
public interface IReservaCaretaker
{
    void Salvar(ReservaMemento memento);
    ReservaMemento? RemoverUltimo(string reservaId);
    IReadOnlyList<ReservaMemento> ObterHistorico(string reservaId);
    bool PossuiHistorico(string reservaId);
}
