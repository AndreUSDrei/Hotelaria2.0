namespace SistemaHotelaria.Services.Memento;

public class ReservaCaretaker : IReservaCaretaker
{
    private readonly Dictionary<string, List<ReservaMemento>> _historico = new();
    private readonly object _lock = new();
    private const int LimitePorReserva = 10;

    public void Salvar(ReservaMemento memento)
    {
        lock (_lock)
        {
            if (!_historico.TryGetValue(memento.Id, out var lista))
            {
                lista = new List<ReservaMemento>();
                _historico[memento.Id] = lista;
            }

            lista.Add(memento);
            if (lista.Count > LimitePorReserva)
                lista.RemoveAt(0);
        }
    }

    public ReservaMemento? RemoverUltimo(string reservaId)
    {
        lock (_lock)
        {
            if (!_historico.TryGetValue(reservaId, out var lista) || lista.Count == 0)
                return null;

            var ultimo = lista[^1];
            lista.RemoveAt(lista.Count - 1);
            return ultimo;
        }
    }

    public IReadOnlyList<ReservaMemento> ObterHistorico(string reservaId)
    {
        lock (_lock)
        {
            if (!_historico.TryGetValue(reservaId, out var lista) || lista.Count == 0)
                return Array.Empty<ReservaMemento>();

            return lista.ToList();
        }
    }

    public bool PossuiHistorico(string reservaId) => ObterHistorico(reservaId).Count > 0;
}
