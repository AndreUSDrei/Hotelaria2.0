# Sistema de Hotelaria - Demonstração de Padrões de Projeto

Este projeto foi simplificado para demonstrar três padrões de projeto essenciais em uma aplicação MVC: **Observer**, **Strategy** e **Composite**.

![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4)
![C#](https://img.shields.io/badge/C%23-12.0-239120)
![MVC](https://img.shields.io/badge/ASP.NET%20MVC-Core-blue)
![Patterns](https://img.shields.io/badge/Patterns-Observer%20%7C%20Strategy%20%7C%20Composite-orange)

---

## 📋 Estrutura do Projeto

```
SistemaHotelaria/
├── Controllers/
│   ├── HomeController.cs          - Controlador principal simplificado
│   └── ReservasController.cs      - Demonstração dos 3 padrões em ação
├── Models/
│   └── Reserva.cs                 - Modelo principal (Subject do Observer)
├── Services/
│   ├── Observer/                  - Padrão Observer
│   │   ├── ISubject.cs           - Interface do Subject
│   │   ├── IObserver.cs          - Interface do Observer
│   │   ├── Recepcao.cs           - Observer: Recepção
│   │   ├── ServicoEmail.cs       - Observer: Serviço de E-mail
│   │   └── ServicoLimpeza.cs     - Observer: Serviço de Limpeza
│   ├── Strategy/                  - Padrão Strategy
│   │   ├── IEstrategiaPagamento.cs    - Interface da estratégia
│   │   ├── ResultadoPagamento.cs      - Resultado do pagamento
│   │   ├── EstrategiaPagamentoFactory.cs - Factory para estratégias
│   │   ├── PagamentoPix.cs           - Estratégia: Pix
│   │   ├── PagamentoCartaoCredito.cs  - Estratégia: Cartão de Crédito
│   │   └── PagamentoBoleto.cs         - Estratégia: Boleto
│   └── Notifications/             - Padrão Composite
│       ├── INotificacaoReserva.cs    - Interface do componente
│       ├── CompositeNotificacaoAdapter.cs - Componente composto
│       ├── ConsoleNotificacaoAdapter.cs   - Folha: Console
│       └── WebNotificacaoAdapter.cs       - Folha: Web
├── Views/                        - Views simplificadas
└── Program.cs                    - Configuração simplificada
```

## 🔔 Padrão Observer

### Propósito
Define uma dependência um-para-muitos entre objetos, de forma que quando um objeto muda de estado, todos os seus dependentes são notificados automaticamente.

### Implementação
- **Subject**: `Reserva` (implementa `ISubject`)
- **Observers**: `Recepcao`, `ServicoEmail`, `ServicoLimpeza` (implementam `IObserver`)
- **Uso**: Quando o status da reserva muda (Check-in/Check-out), todos os observadores são notificados

### Código de Exemplo
```csharp
// No Controller
reserva.Anexar(new Recepcao());
reserva.Anexar(new ServicoEmail());
reserva.Anexar(new ServicoLimpeza());

// Quando o status muda
reserva.CheckIn(); // Notifica automaticamente todos os observers
```

## 💳 Padrão Strategy

### Propósito
Define uma família de algoritmos, encapsula cada um e os torna intercambiáveis. Strategy permite que o algoritmo varie independentemente dos clientes que o usam.

### Implementação
- **Interface**: `IEstrategiaPagamento`
- **Estratégias Concretas**: `PagamentoPix`, `PagamentoCartaoCredito`, `PagamentoBoleto`
- **Factory**: `EstrategiaPagamentoFactory` para criar estratégias
- **Uso**: O método de pagamento pode ser alterado em tempo de execução

### Código de Exemplo
```csharp
// No Controller
var estrategia = EstrategiaPagamentoFactory.Criar("pix");
reserva.DefinirEstrategiaPagamento(estrategia);
reserva.ProcessarPagamento(500.00m);
```

## 📦 Padrão Composite

### Propósito
Compõe objetos em estruturas de árvore para representar hierarquias parte-todo. Permite que clientes tratem objetos individuais e composições de objetos de maneira uniforme.

### Implementação
- **Componente**: `INotificacaoReserva` (interface comum)
- **Composite**: `CompositeNotificacaoAdapter` (gerencia múltiplos canais)
- **Folhas**: `ConsoleNotificacaoAdapter`, `WebNotificacaoAdapter`
- **Uso**: Notificações são enviadas através de múltiplos canais simultaneamente

### Código de Exemplo
```csharp
// No Controller
var composite = new CompositeNotificacaoAdapter();
composite.Adicionar(new ConsoleNotificacaoAdapter());
composite.Adicionar(new WebNotificacaoAdapter());
composite.InformarSucesso("Mensagem enviada para todos os canais");
```

## 🚀 Como Executar

1. **Restaurar dependências**:
   ```bash
   dotnet restore
   ```

2. **Compilar o projeto**:
   ```bash
   dotnet build
   ```

3. **Executar a aplicação**:
   ```bash
   dotnet run
   ```

4. **Acessar no navegador**:
   - Home: `https://localhost:5001/`
   - Lista de Reservas: `https://localhost:5001/Reservas`
   - Criar Reserva: `https://localhost:5001/Reservas/Criar`

## 📝 Demonstração Prática

### Passo 1: Criar uma Reserva
1. Acesse `/Reservas/Criar`
2. Preencha os dados do hóspede
3. Escolha o método de pagamento (Strategy Pattern)
4. Ao criar, os observadores são anexados (Observer Pattern)
5. Notificações são enviadas via Composite Pattern

### Passo 2: Realizar Check-in
1. Acesse os detalhes de uma reserva
2. Clique em "Realizar Check-in"
3. Observe no console que Recepção, E-mail e Limpeza são notificados (Observer)

### Passo 3: Realizar Check-out
1. Clique em "Realizar Check-out"
2. Novamente, todos os observadores são notificados automaticamente

## 💡 Pontos-Chave para Apresentação

### Observer Pattern
- **Desacoplamento**: Subject não precisa saber quem são os observers
- **Aberto/Fechado**: Novos observers podem ser adicionados sem modificar o Subject
- **Comunicação**: Notificação automática de mudanças de estado

### Strategy Pattern
- **Flexibilidade**: Algoritmos podem ser alterados em tempo de execução
- **Reutilização**: Estratégias podem ser reutilizadas em diferentes contextos
- **Testabilidade**: Cada estratégia pode ser testada independentemente

### Composite Pattern
- **Tratamento Uniforme**: Clientes tratam individuais e composições igualmente
- **Hierarquias**: Representa estruturas de árvore naturalmente
- **Extensibilidade**: Novos componentes podem ser adicionados facilmente

## 🎯 Benefícios da Simplificação

- **Código Limpo**: Removeu padrões não essenciais (Builder, Decorator, Prototype, State)
- **Foco nos 3 Padrões**: Observer, Strategy e Composite são os únicos padrões demonstrados
- **Sem Banco de Dados**: Usa lista em memória para simplicidade
- **Views Simplificadas**: Interface focada em demonstrar os padrões
- **Documentação**: Código com XML comments explicando cada padrão

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 9.0 | Framework principal |
| ASP.NET Core MVC | 9.0 | Arquitetura web |
| C# | 12.0 | Linguagem de programação |
| Razor | - | Engine de templates |

## 📚 Referências

- Observer Pattern: https://refactoring.guru/design-patterns/observer
- Strategy Pattern: https://refactoring.guru/design-patterns/strategy
- Composite Pattern: https://refactoring.guru/design-patterns/composite

---

<div align="center">
  <h3>🏨 Sistema de Hotelaria</h3>
  <p>Design Patterns in Action - Observer, Strategy & Composite</p>
</div>
