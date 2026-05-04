# 🏨 Sistema de Hotelaria Luxury

Sistema completo de gerenciamento de hotel desenvolvido em **ASP.NET Core MVC** utilizando os padrões de projeto **Builder** e **Prototype**. Interface web premium com design de hotel 5 estrelas.

![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4)
![C#](https://img.shields.io/badge/C%23-12.0-239120)
![MVC](https://img.shields.io/badge/ASP.NET%20MVC-Core-blue)
![Patterns](https://img.shields.io/badge/Patterns-Builder%20%2B%20Prototype-orange)

---

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Padrões de Projeto](#padrões-de-projeto)
  - [Builder Pattern](#builder-pattern)
  - [Prototype Pattern](#prototype-pattern)
- [Funcionalidades](#funcionalidades)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Tecnologias](#tecnologias)
- [Como Executar](#como-executar)
- [Telas do Sistema](#telas-do-sistema)
- [Diagrama de Classes](#diagrama-de-classes)

---

## 🎯 Sobre o Projeto

O **Sistema de Hotelaria Luxury** é uma aplicação web para gestão de reservas de hotel, demonstrando a aplicação prática dos padrões de projeto **Builder** e **Prototype** no contexto de desenvolvimento ASP.NET Core MVC.

### Contexto do Negócio
O hotel oferece três tipos de quartos (Standard, Luxo e Suíte) e quatro pacotes de hospedagem (Romântico, Business, Básico e Fim de Semana). O sistema gerencia disponibilidade, check-in/check-out e cálculo de valores.

---

## 🏗️ Padrões de Projeto

### Builder Pattern

O padrão **Builder** é utilizado para construir objetos complexos de `PacoteHospedagem` passo a passo, permitindo a criação de diferentes representações do mesmo objeto.

#### Implementação

```
Builder/
├── IPacoteHospedagemBuilder.cs    # Interface do Builder
├── PacoteHospedagem.cs            # Produto final
├── PacoteRomanticoBuilder.cs      # ConcreteBuilder A
├── PacoteNegociosBuilder.cs       # ConcreteBuilder B
└── HotelDirector.cs               # Director
```

#### Componentes

| Componente | Descrição | Arquivo |
|------------|-----------|---------|
| **Builder** | Interface que define os passos para construir um pacote | `IPacoteHospedagemBuilder.cs` |
| **ConcreteBuilder** | Implementação específica para cada tipo de pacote | `PacoteRomanticoBuilder.cs`, `PacoteNegociosBuilder.cs` |
| **Product** | Objeto complexo resultante da construção | `PacoteHospedagem.cs` |
| **Director** | Orquestra a construção dos pacotes pré-definidos | `HotelDirector.cs` |

#### Fluxo Builder
```
Cliente → Director → Builder → PacoteHospedagem
                    ↓
              [Definir nome]
              [Selecionar quarto]
              [Adicionar refeições]
              [Adicionar serviços]
              [Aplicar desconto]
```

### Prototype Pattern

O padrão **Prototype** é utilizado para criar novos objetos de quarto através da clonagem de protótipos existentes, evitando a criação de instâncias do zero.

#### Implementação

```
Prototype/
├── IQuarto.cs          # Interface Prototype (contrato Clone)
├── QuartoStandard.cs   # ConcretePrototype A
├── QuartoLuxo.cs       # ConcretePrototype B
└── Suite.cs          # ConcretePrototype C
```

#### Tipos de Quarto

| Quarto | Preço Base | Capacidade | Comodidades |
|--------|------------|------------|-------------|
| **Standard** | R$ 250,00 | 2 pessoas | Wi-Fi, TV, Ar condicionado |
| **Luxo** | R$ 550,00 | 3 pessoas | Vista para o mar, Mini bar, Café expresso |
| **Suíte** | R$ 1.050,00 | 4 pessoas | Sala de estar, Hidromassagem, Serviço de quarto 24h |

#### Método Clone
```csharp
public IQuarto Clone()
{
    return (IQuarto)this.MemberwiseClone();
}
```

---

## ✨ Funcionalidades

### Gerenciamento de Quartos
- [x] Cadastro de protótipos de quartos (Prototype)
- [x] Clonagem de quartos para novas reservas
- [x] Visualização de comodidades e capacidade

### Pacotes de Hospedagem
- [x] **Romântico** (10% OFF): Café no quarto, spa, decoração especial
- [x] **Business** (15% OFF): Café expresso, sala de reuniões, transfer
- [x] **Básico** (Sem desconto): Café da manhã buffet
- [x] **Fim de Semana** (20% OFF): Café, jantar, academia

### Sistema de Reservas
- [x] Criar reserva com pacote ou apenas quarto
- [x] Verificar disponibilidade por data e tipo
- [x] Check-in e check-out
- [x] Cálculo automático de valores totais
- [x] Listagem de todas as reservas

### Design Premium
- [x] Interface de hotel 5 estrelas
- [x] Paleta de cores dourado + azul marinho
- [x] Tipografia Playfair Display + Montserrat
- [x] Totalmente responsivo

---

## 📁 Estrutura do Projeto

```
Builder e Prototype/
├── 📂 Builder/                      # Padrão Builder
│   ├── IPacoteHospedagemBuilder.cs
│   ├── PacoteHospedagem.cs
│   ├── PacoteRomanticoBuilder.cs
│   ├── PacoteNegociosBuilder.cs
│   └── HotelDirector.cs
│
├── 📂 Prototype/                    # Padrão Prototype
│   ├── IQuarto.cs
│   ├── QuartoStandard.cs
│   ├── QuartoLuxo.cs
│   └── Suite.cs
│
├── 📂 Models/                       # Modelos de dados
│   ├── Reserva.cs
│   ├── Refeicao.cs
│   └── ServicoAdicional.cs
│
├── 📂 Services/                     # Serviços de negócio
│   ├── GerenciadorReservas.cs     # Singleton para gerenciamento
│   └── HotelService.cs              # Centraliza lógica de hotel
│
├── 📂 Controllers/                  # Controllers MVC
│   ├── HomeController.cs
│   └── ReservasController.cs
│
├── 📂 Views/                        # Views Razor
│   ├── 📂 Home/
│   │   ├── Index.cshtml
│   │   └── Pacotes.cshtml
│   ├── 📂 Reservas/
│   │   ├── Index.cshtml
│   │   ├── Criar.cshtml
│   │   └── Detalhes.cshtml
│   ├── 📂 Shared/
│   │   └── _Layout.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
│
├── 📂 wwwroot/
│   └── 📂 css/
│       └── site.css                 # Estilos premium
│
├── Program.cs                       # Configuração da aplicação
├── SistemaHotelaria.csproj
└── README.md
```

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 9.0 | Framework principal |
| ASP.NET Core MVC | 9.0 | Arquitetura web |
| C# | 12.0 | Linguagem de programação |
| Razor | - | Engine de templates |
| HTML5 | - | Estrutura das views |
| CSS3 | - | Estilização premium |
| JavaScript | ES6+ | Interatividade |

### Bibliotecas
- **Google Fonts**: Playfair Display, Montserrat
- **CSS**: Design system próprio (sem frameworks externos)

---

## 🚀 Como Executar

### Pré-requisitos
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) instalado
- Git (opcional, para clonagem)

### Passo a Passo

1. **Clone ou baixe o projeto**
```bash
cd "Builder e Prototype"
```

2. **Restaure as dependências**
```bash
dotnet restore
```

3. **Compile o projeto**
```bash
dotnet build
```

4. **Execute a aplicação**
```bash
dotnet run
```

5. **Acesse no navegador**
```
http://localhost:5000
```

### Comando Único
```bash
dotnet run --urls "http://localhost:5000"
```

---

## 📱 Telas do Sistema

### 🏠 Página Inicial
- Hero section com design premium
- Cards de quartos com informações detalhadas
- Stats bar com métricas do hotel

### 📦 Pacotes
- Grid de 4 pacotes de hospedagem
- **Tabela de preços por tipo de quarto** (incluindo quarto + serviços)
- Descontos visualizados

### 📝 Nova Reserva
- Formulário de reserva com validações
- **Lógica inteligente**: se pacote selecionado, quarto é desabilitado
- Cálculo automático de valores

### 📋 Lista de Reservas
- Tabela com todas as reservas
- Status visual (badge colorido)
- Ações rápidas (check-in/out)

### 🔍 Detalhes da Reserva
- Informações completas da reserva
- Timeline de status
- Valor total destacado

---

## 📊 Diagrama de Classes

### Builder Pattern
```
┌─────────────────────────┐
│  IPacoteHospedagem      │
│  <<interface>>          │
├─────────────────────────┤
│ + DefinirNome()         │
│ + SelecionarQuarto()    │
│ + AdicionarCafe()       │
│ + AdicionarAlmoco()     │
│ + AdicionarJantar()     │
│ + AdicionarServico()    │
│ + AplicarDesconto()     │
│ + Build()               │
└─────────────────────────┘
           △
           │ implements
    ┌──────┴──────────────┐
    │                     │
┌───┴────────┐      ┌──────┴─────────┐
│ Pacote     │      │ Pacote         │
│ Romantico  │      │ Negocios       │
│ Builder    │      │ Builder        │
└────────────┘      └────────────────┘
```

### Prototype Pattern
```
┌─────────────────┐
│ IQuarto         │
│ <<interface>>   │
├─────────────────┤
│ + Clone()       │
│ + Tipo          │
│ + Descricao     │
│ + PrecoBase     │
│ + Capacidade    │
│ + Comodidades   │
└─────────────────┘
         △
    ┌────┼────┐
    │    │    │
┌───┴┐ ┌─┴──┐ ┌┴────┐
│Std │ │Luxo│ │Suite│
└────┘ └────┘ └─────┘
```

### MVC Architecture
```
    ┌─────────────┐
    │   Cliente   │
    └──────┬──────┘
           │ HTTP
           ▼
    ┌─────────────┐
    │  Controller │  ← Recebe requisição
    │  (Actions)  │
    └──────┬──────┘
           │ usa
           ▼
    ┌─────────────┐
    │   Service   │  ← Lógica de negócio
    │  (Hotel)    │
    └──────┬──────┘
           │ usa
           ▼
    ┌─────────────┐
    │  Prototype  │  ← Clone quartos
    │  + Builder  │  ← Cria pacotes
    └──────┬──────┘
           │ retorna
           ▼
    ┌─────────────┐
    │    View     │  ← Razor + CSS
    │   (HTML)    │
    └─────────────┘
```

---

## 💡 Exemplos de Uso

### Criar Reserva com Pacote
```csharp
// Controller automaticamente usa Builder + Prototype
[HttpPost]
public IActionResult Criar(string hospedeNome, string tipoQuarto, 
                           string tipoPacote, DateTime dataEntrada, 
                           DateTime dataSaida)
{
    // 1. Prototype: Clona o quarto
    var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuarto);
    
    // 2. Builder: Cria pacote com Director
    var builder = _hotelService.CriarBuilder(tipoPacote);
    var director = new HotelDirector(builder);
    director.ConstruirPacoteRomanticoCompleto(quarto);
    
    // 3. Cria reserva
    var reserva = _gerenciador.CriarReservaWeb(
        hospedeNome, tipoQuarto, dataEntrada, dataSaida, 
        director.ObterPacote()
    );
}
```

---

## 👨‍💻 Autor

Desenvolvido como projeto acadêmico demonstrando padrões de projeto em C# ASP.NET Core MVC.

---

## 📄 Licença

Este projeto é de uso educacional. Sinta-se livre para estudar, modificar e aprender com o código.

---

## 🔗 Referências

- [Design Patterns - GoF](https://en.wikipedia.org/wiki/Design_Patterns)
- [Builder Pattern](https://refactoring.guru/design-patterns/builder)
- [Prototype Pattern](https://refactoring.guru/design-patterns/prototype)
- [ASP.NET Core MVC](https://docs.microsoft.com/aspnet/core/mvc/)

---

<div align="center">
  <h3>🏨 Hotelaria Luxury</h3>
  <p>Design Patterns in Action</p>
  <p>
    <strong>Builder</strong> + <strong>Prototype</strong> + <strong>ASP.NET Core</strong>
  </p>
</div>
