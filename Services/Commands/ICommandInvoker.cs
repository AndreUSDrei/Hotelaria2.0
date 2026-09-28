namespace SistemaHotelaria.Services.Commands;

public interface ICommandInvoker
{
    void ExecuteCommand(ICommand command);
    void UndoLastCommand();
    void ClearHistory();
    int GetCommandHistoryCount();
}