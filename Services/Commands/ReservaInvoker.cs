using System.Collections.Concurrent;

namespace SistemaHotelaria.Services.Commands;

public class ReservaInvoker : ICommandInvoker
{
    private readonly Stack<ICommand> _historicoComandos = new();
    private readonly int _maxHistorico = 10;

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _historicoComandos.Push(command);

        if (_historicoComandos.Count > _maxHistorico)
        {
            _historicoComandos.Pop();
        }
    }

    public void UndoLastCommand()
    {
        if (_historicoComandos.Count > 0)
        {
            var command = _historicoComandos.Pop();
            command.Undo();
        }
    }

    public void ClearHistory()
    {
        _historicoComandos.Clear();
    }

    public int GetCommandHistoryCount() => _historicoComandos.Count;
}