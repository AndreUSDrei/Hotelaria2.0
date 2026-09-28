# 📋 Padrão Command - Documentação de Implementação (Corrigida)

## 🎯 O que é o Padrão Command?

O **Padrão Command** é um padrão de projeto comportamental (GoF - Gang of Four) que transforma uma solicitação em um objeto independente que contém todas as informações sobre a solicitação. Essa transformação permite que você parametrize métodos com diferentes solicitações, atrasar ou enfileirar a execução de uma solicitação e suportar operações de desfazer (undo).

### 🏗️ Estrutura do Padrão Command (GoF)

O padrão Command consiste em quatro componentes principais:

1. **Command (Comando)**: Interface que declara os métodos de execução (`Execute`) e desfazimento (`Undo`)
2. **Concrete Command (Comando Concreto)**: Implementações específicas que realizam ações concretas
3. **Invoker (Invocador)**: Gerencia e executa os comandos, mantendo histórico para operações de undo
4. **Receiver (Receptor)**: O objeto que realmente executa a lógica de negócio (neste caso, o `IGerenciadorReservas`)

## 📍 Localização no Projeto

### Estrutura de Arquivos

```
Services/
├── Commands/
│   ├── ICommand.cs                    # Interface do padrão Command
│   ├── ICommandInvoker.cs              # Interface do Invoker
│   ├── ICommandFactory.cs              # Factory para criar comandos (DI-friendly)
│   ├── ReservaCommandFactory.cs        # Implementação da Factory
│   ├── ReservaInvoker.cs               # Invoker - gerencia execução e histórico (Scoped)
│   ├── CheckInCommand.cs               # Concrete Command - Check-in
│   ├── CheckOutCommand.cs              # Concrete Command - Check-out
│   └── CriarReservaCommand.cs          # Concrete Command - Criação de reserva
└── Memento/
    └── ReservaMemento.cs               # Memento para armazenar estado anterior (Undo real)
```

### Integração com Código Existente

**Controller**: `Controllers/ReservasController.cs`
- Injeta `ICommandInvoker` (Scoped) para gerenciar comandos por requisição
- Injeta `ICommandFactory` para criar comandos sem acoplamento direto
- Usa comandos nas ações de Check-in e Check-out
- Adiciona nova ação `DesfazerUltimaAcao`

**DI Container**: `Program.cs`
- Registra `ICommandFactory` como Scoped
- Registra `ICommandInvoker` como Scoped (correção crítica)
- Remove registro Singleton que causava problemas multiusuário

## 🔍 Como o Padrão Command Está Ajudando o Projeto (Corrigido)

### 1. **Desacoplamento de Solicitações (Factory Pattern)**
Antes da correção, os controllers criavam comandos manualmente:
```csharp
// ANTES (Incorreto - acoplamento direto)
var comando = new CheckInCommand(_gerenciadorReservas, id);
_reservaInvoker.ExecutarComando(comando);
```

Agora, os controllers usam uma Factory para criar comandos:
```csharp
// DEPOIS (Correto - desacoplado via Factory)
var comando = _commandFactory.CreateCheckInCommand(id);
_commandInvoker.ExecuteCommand(comando);
```

**Benefício**: O controller não conhece a implementação do comando nem depende diretamente do receptor, mantendo inversão de controle.

### 2. **Isolamento por Usuário (Scoped Lifetime)**
Antes da correção, o Invoker era Singleton com histórico global:
```csharp
// ANTES (Incorreto - histórico compartilhado)
builder.Services.AddSingleton<ReservaInvoker>();
// Usuário A faz check-in, Usuário B desfaz e desfez ação do Usuário A!
```

Agora, o Invoker é Scoped, criando uma instância por requisição:
```csharp
// DEPOIS (Correto - isolado por requisição)
builder.Services.AddScoped<ICommandInvoker, ReservaInvoker>();
// Cada requisição tem seu próprio histórico de comandos
```

**Benefício**: Cada usuário tem seu próprio histórico de undo, evitando conflitos em ambiente multiusuário.

### 3. **Undo Real com Memento Pattern**
Antes da correção, o undo chamava a operação oposta:
```csharp
// ANTES (Incorreto - não restaura estado exato)
public void Undo()
{
    if (_executado)
    {
        var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
        if (reserva != null && reserva.Status == "Ocupada")
        {
            _gerenciadorReservas.RealizarCheckOut(_idReserva); // Check-out ≠ oposto de Check-in
            _executado = false;
        }
    }
}
```

Agora, o undo restaura o estado anterior usando Memento:
```csharp
// DEPOIS (Correto - restaura estado exato)
public void Execute()
{
    if (!_executado)
    {
        var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
        if (reserva == null)
            return;

        _estadoAnterior = reserva.CriarMemento(); // Salva estado antes
        _gerenciadorReservas.RealizarCheckIn(_idReserva);
        _executado = true;
    }
}

public void Undo()
{
    if (_executado && _estadoAnterior != null)
    {
        var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
        if (reserva != null)
        {
            reserva.RestaurarMemento(_estadoAnterior); // Restaura estado exato
            _gerenciadorReservas.AtualizarReserva(reserva);
            _executado = false;
        }
    }
}
```

**Benefício**: O undo restaura o estado exato anterior, não apenas executa uma operação oposta.

### 4. **Encapsulamento Completo de Operações**
Cada comando encapsula toda a lógica necessária para executar e desfazer uma operação:
- Captura o estado anterior via Memento antes da execução
- Executa a operação através do receptor
- Restaura o estado exato no undo
- Mantém controle de execução para evitar reexecução

**Benefício**: Lógica de execução e desfazimento encapsulada em um único lugar, facilitando manutenção.

### 5. **Extensibilidade e Open/Closed Principle**
Adicionar novos comandos é simples - basta criar uma nova classe que implementa `ICommand` e adicionar método na Factory:

```csharp
// Na interface ICommandFactory
ICommand CreateCancelarReservaCommand(string idReserva);

// Na implementação ReservaCommandFactory
public ICommand CreateCancelarReservaCommand(string idReserva)
{
    return new CancelarReservaCommand(_gerenciadorReservas, idReserva);
}
```

**Benefício**: Novas funcionalidades podem ser adicionadas sem modificar código existente.

## 🔄 Fluxo de Execução Corrigido

### Cenário: Check-in de Reserva

```
1. Usuário clica em "Check-in" na interface
       ↓
2. Controller.ReservasController.CheckIn(id)
       ↓
3. _commandFactory.CreateCheckInCommand(id)
       ↓
4. Factory retorna new CheckInCommand(_gerenciadorReservas, id)
       ↓
5. _commandInvoker.ExecuteCommand(comando)
       ↓
6. comando.Execute()
       ↓
7. Reserva.CriarMemento() - Salva estado anterior
       ↓
8. _gerenciadorReservas.RealizarCheckIn(id)
       ↓
9. Comando adicionado ao histórico (stack local da requisição)
       ↓
10. Notificação de sucesso enviada ao usuário
```

### Cenário: Desfazer Última Ação

```
1. Usuário clica em "Desfazer Última Ação"
       ↓
2. Controller.ReservasController.DesfazerUltimaAcao()
       ↓
3. _commandInvoker.UndoLastCommand()
       ↓
4. Recupera último comando do histórico (stack local)
       ↓
5. comando.Undo()
       ↓
6. Reserva.RestaurarMemento(_estadoAnterior)
       ↓
7. _gerenciadorReservas.AtualizarReserva(reserva)
       ↓
8. Remove comando do histórico
       ↓
9. Notificação de sucesso enviada ao usuário
```

## 🎨 Integração com Outros Padrões

O padrão Command se integra harmoniosamente com os outros padrões já existentes no projeto:

### Com **Memento Pattern** (Novo)
- Cada comando captura o estado anterior via Memento antes da execução
- Undo restaura o estado exato usando o Memento salvo
- Implementação fiel ao conceito de "snapshot" de estado

### Com **Factory Pattern** (Novo)
- `ICommandFactory` abstrai a criação de comandos
- Controller não conhece implementações concretas
- Mantém inversão de controle e desacoplamento

### Com **Facade Pattern**
- O `ReservaFacade` continua sendo a interface principal para operações de leitura
- O Command pattern opera em um nível mais granular para operações de escrita
- Separação clara entre leitura (Facade) e escrita (Command)

### Com **Proxy Pattern**
- Os comandos utilizam o `IGerenciadorReservas` que já está protegido por proxies
- Validações e caching continuam funcionando através dos proxies
- Comandos beneficiam-se automaticamente das validações do Proxy

### Com **Strategy Pattern**
- O comando de criação de reserva utiliza a estratégia de pagamento
- Cada comando pode encapsular diferentes estratégias
- Manutenção da flexibilidade de escolha de algoritmos

### Com **Observer Pattern**
- Quando um comando é executado, o receptor notifica observadores
- O sistema de notificações (email, limpeza, recepção) continua funcionando
- Notificações são acionadas automaticamente através dos observadores

## 📊 Benefícios Específicos para o Sistema de Hotelaria

### 1. **Auditoria de Operações por Requisição**
Histórico de comandos isolado por requisição HTTP:
- Cada sessão de usuário tem seu próprio histórico
- Rastreamento preciso de operações por usuário
- Isolamento completo entre usuários simultâneos

### 2. **Recuperação de Erros com Restauração Exata**
Se uma operação falhar, pode-se desfazer automaticamente restaurando o estado exato:
```csharp
try
{
    _commandInvoker.ExecuteCommand(comando);
}
catch (Exception ex)
{
    _commandInvoker.UndoLastCommand(); // Restaura estado exato
    // Tratamento de erro
}
```

### 3. **Operações em Lote Transacionais**
Possibilidade de executar múltiplos comandos em transação com rollback:
```csharp
var comandos = new List<ICommand>
{
    _commandFactory.CreateCheckInCommand(id1),
    _commandFactory.CreateCheckInCommand(id2),
    _commandFactory.CreateCheckInCommand(id3)
};

foreach (var cmd in comandos)
    _commandInvoker.ExecuteCommand(cmd);

// Se falhar, desfaz todos na ordem inversa
while (_commandInvoker.GetCommandHistoryCount() > 0)
    _commandInvoker.UndoLastCommand();
```

### 4. **Agendamento de Operações**
Comandos podem ser agendados para execução em horários específicos:
- Check-in automático às 14:00
- Check-out automático às 12:00
- Limpeza programada após check-out

## 🔧 Implementação Técnica Corrigida

### Lifetime Management (Correção Crítica)
O `ReservaInvoker` agora é registrado como **Scoped** em vez de Singleton:
```csharp
// Program.cs (CORRIGIDO)
builder.Services.AddScoped<ICommandInvoker, ReservaInvoker>();
```

**Benefício**: Cada requisição HTTP recebe sua própria instância do Invoker, isolando o histórico de comandos por usuário.

### Memento Pattern para Undo Real
Cada comando captura o estado anterior antes da execução:
```csharp
public class CheckInCommand : ICommand
{
    private ReservaMemento? _estadoAnterior;

    public void Execute()
    {
        var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
        _estadoAnterior = reserva.CriarMemento(); // Captura estado
        _gerenciadorReservas.RealizarCheckIn(_idReserva);
    }

    public void Undo()
    {
        var reserva = _gerenciadorReservas.ObterReservaPorId(_idReserva);
        reserva.RestaurarMemento(_estadoAnterior); // Restaura estado
        _gerenciadorReservas.AtualizarReserva(reserva);
    }
}
```

**Benefício**: Undo restaura o estado exato anterior, não apenas executa operação oposta.

### Factory Pattern para Criação de Comandos
Factory abstrai a criação de comandos:
```csharp
public interface ICommandFactory
{
    ICommand CreateCheckInCommand(string idReserva);
    ICommand CreateCheckOutCommand(string idReserva);
    // ...
}

public class ReservaCommandFactory : ICommandFactory
{
    private readonly IGerenciadorReservas _gerenciadorReservas;

    public ICommand CreateCheckInCommand(string idReserva)
    {
        return new CheckInCommand(_gerenciadorReservas, idReserva);
    }
}
```

**Benefício**: Controller não depende de implementações concretas de comandos.

### Stack Local para Histórico
O histórico usa `Stack<T>` local em vez de `ConcurrentStack<T>` global:
```csharp
public class ReservaInvoker : ICommandInvoker
{
    private readonly Stack<ICommand> _historicoComandos = new();
    // ...
}
```

**Benefício**: Cada instância do Invoker tem seu próprio histórico isolado.

## 🚨 Correções Realizadas vs Implementação Original

| Aspecto | Original (Incorreto) | Corrigido (Fiel ao GoF) |
|---------|---------------------|------------------------|
| **Invoker Lifetime** | Singleton (global) | Scoped (por requisição) |
| **Histórico de Comandos** | `ConcurrentStack` global | `Stack` local por instância |
| **Criação de Comandos** | `new` no Controller | Factory via DI |
| **Undo Implementation** | Chamada de operação oposta | Memento Pattern (restauração exata) |
| **Acoplamento** | Controller conhece receptor | Controller usa Factory |
| **Multiusuário** | Conflito entre usuários | Isolamento completo |
| **Restauração de Estado** | Parcial/incompleta | Exata via Memento |

## 📝 Exemplo de Uso Completo (Corrigido)

```csharp
// No Controller
public class ReservasController : Controller
{
    private readonly ICommandInvoker _commandInvoker;
    private readonly ICommandFactory _commandFactory;

    public ReservasController(ICommandInvoker commandInvoker, ICommandFactory commandFactory)
    {
        _commandInvoker = commandInvoker;
        _commandFactory = commandFactory;
    }

    [HttpPost]
    public IActionResult CheckIn(string id)
    {
        // Cria comando via Factory (desacoplado)
        var comando = _commandFactory.CreateCheckInCommand(id);
        
        // Executa através do Invoker (scoped, isolado por requisição)
        _commandInvoker.ExecuteCommand(comando);
        
        // Verifica resultado
        var reserva = _reservaFacade.ObterReservaPorId(id);
        if (reserva != null && reserva.Status == "Check-in")
            TempData["Sucesso"] = "Check-in realizado com sucesso.";
        else
            TempData["Erro"] = "Não foi possível realizar o check-in";

        return RedirectToAction(nameof(Detalhes), new { id });
    }

    [HttpPost]
    public IActionResult DesfazerUltimaAcao()
    {
        // Desfaz o último comando do histórico local desta requisição
        _commandInvoker.UndoLastCommand();
        TempData["Sucesso"] = "Última ação desfeita com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
```

## 🎯 Conclusão

A implementação corrigida do padrão Command neste projeto de hotelaria agora é **fiel ao padrão GoF** e resolve todos os problemas identificados:

- **Fidelidade ao Padrão**: Segue estritamente a estrutura Command, ConcreteCommand, Invoker, Receiver
- **Undo Real**: Implementa undo real usando Memento Pattern para restauração exata de estado
- **Isolamento Multiusuário**: Invoker Scoped garante que cada usuário tenha seu próprio histórico
- **Desacoplamento**: Factory Pattern elimina acoplamento direto do Controller com implementações
- **Encapsulamento**: Cada comando encapsula completamente sua lógica de execução e desfazimento
- **Integração**: Funciona harmoniosamente com outros padrões (Facade, Proxy, Strategy, Observer, Memento)

A correção transforma uma implementação problemática em uma arquitetura robusta, maintainable e fiel aos princípios do GoF, adequada para aplicações web multiusuário em ASP.NET Core.