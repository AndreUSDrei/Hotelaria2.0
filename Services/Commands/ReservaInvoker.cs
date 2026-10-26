namespace SistemaHotelaria.Services.Commands;

public class ReservaInvoker : ICommandInvoker
{
    private readonly List<ICommand> _historicoComandos = new();
    private readonly int _maxHistorico = 10;

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _historicoComandos.Add(command);

        if (_historicoComandos.Count > _maxHistorico)
            _historicoComandos.RemoveAt(0);
    }

    public void UndoLastCommand()
    {
        if (_historicoComandos.Count == 0)
            return;

        var command = _historicoComandos[^1];
        _historicoComandos.RemoveAt(_historicoComandos.Count - 1);
        command.Undo();
    }

    public void ClearHistory()
    {
        _historicoComandos.Clear();
    }

    public int GetCommandHistoryCount() => _historicoComandos.Count;
}
