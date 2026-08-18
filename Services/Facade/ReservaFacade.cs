using SistemaHotelaria.Builder;
using SistemaHotelaria.Decorator;
using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Services.Facade;

/// <summary>
/// Facade: simplifica a criação de reservas ocultando Builder, Director, Prototype e gerenciamento.
/// </summary>
public class ReservaFacade : IReservaFacade
{
    private readonly IGerenciadorReservas _gerenciador;
    private readonly HotelService _hotelService;

    public ReservaFacade(IGerenciadorReservas gerenciador, HotelService hotelService)
    {
        _gerenciador = gerenciador;
        _hotelService = hotelService;
    }

    public List<Reserva> ObterTodasReservas() => _gerenciador.ObterTodasReservas();

    public Reserva? ObterReservaPorId(string id) => _gerenciador.ObterReservaPorId(id);

    public bool RealizarCheckIn(string id) => _gerenciador.RealizarCheckIn(id);

    public bool RealizarCheckOut(string id) => _gerenciador.RealizarCheckOut(id);

    public List<IQuarto> ObterPrototiposQuartos() => _hotelService.ObterPrototiposQuartos();

    public string[] ObterTiposPacote() => ["Romantico", "Negocios", "Basico", "FimDeSemana"];

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida) =>
        _gerenciador.ObterDisponibilidadeCompleta(entrada, saida);

    public int ContarReservasAtivas() => _gerenciador.ObterTodasReservas().Count;

    public Reserva? CriarReservaComPacote(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, string metodoPagamento = "pix",
        string? numeroCartao = null, string? cvv = null)
    {
        var tipoQuartoFinal = tipoQuarto;
        if (!string.IsNullOrEmpty(tipoPacote) && string.IsNullOrEmpty(tipoQuarto))
            tipoQuartoFinal = "Standard";

        var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuartoFinal);
        if (quarto == null)
            return null;

        var builder = _hotelService.CriarBuilder(tipoPacote);
        var director = new HotelDirector(builder);
        ConstruirPacote(director, tipoPacote, quarto);

        return _gerenciador.CriarReservaWeb(hospedeNome, tipoQuartoFinal, dataEntrada, dataSaida, director.ObterPacote(),
            metodoPagamento, numeroCartao, cvv);
    }

    private static void ConstruirPacote(HotelDirector director, string tipoPacote, IQuarto quarto)
    {
        switch (tipoPacote)
        {
            case "Romantico":
                director.ConstruirPacoteRomanticoCompleto(quarto);
                break;
            case "Negocios":
                director.ConstruirPacoteNegociosCompleto(quarto);
                break;
            case "Basico":
                director.ConstruirPacoteBasico(quarto);
                break;
            case "FimDeSemana":
                director.ConstruirPacoteFimDeSemana(quarto);
                break;
            default:
                director.ConstruirPacoteBasico(quarto);
                break;
        }
    }

    // ============================================================
    // IMPLEMENTAÇÃO - Integração com Padrão Decorator
    // ============================================================
    // Este método demonstra como o padrão Decorator se integra com
    // o padrão Facade e Builder.
    //
    // FLUXO:
    // 1. Cria o pacote base usando Builder (como antes)
    // 2. Aplica os decorators selecionados pelo usuário
    // 3. Cada decorator adiciona seu serviço extra
    // 4. Calcula o valor total com todos os decorators
    // 5. Cria a reserva com o pacote decorado
    //
    // IMPORTANTE - ENCADEAMENTO DE DECORATORS:
    // Note como aplicamos os decorators em sequência. Cada decorator
    // recebe o pacote já decorado pelos anteriores, criando uma
    // cadeia de responsabilidades.
    // ============================================================
    public Reserva? CriarReservaComPacoteEDecorators(string hospedeNome, string tipoQuarto, string tipoPacote,
        DateTime dataEntrada, DateTime dataSaida, List<string> decorators, string metodoPagamento = "pix",
        string? numeroCartao = null, string? cvv = null)
    {
        // Passo 1: Cria o pacote base usando Builder (mesmo processo do método original)
        var tipoQuartoFinal = tipoQuarto;
        if (!string.IsNullOrEmpty(tipoPacote) && string.IsNullOrEmpty(tipoQuarto))
            tipoQuartoFinal = "Standard";

        var quarto = _hotelService.ObterPrototipoPorTipo(tipoQuartoFinal);
        if (quarto == null)
            return null;

        var builder = _hotelService.CriarBuilder(tipoPacote);
        var director = new HotelDirector(builder);
        ConstruirPacote(director, tipoPacote, quarto);

        // Obtém o pacote base (sem decorators ainda)
        var pacote = director.ObterPacote();

        // ============================================================
        // Passo 2: Aplica os decorators selecionados
        // ============================================================
        // A lista "decorators" contém strings como "Spa", "Transfer", etc.
        // Iteramos sobre cada um e aplicamos o decorator correspondente.
        //
        // IMPORTANTE - FACTORY METHOD IMPLÍCITO:
        // O switch abaixo atua como um Factory Method simples, criando
        // o decorator apropriado baseado na string fornecida.
        // ============================================================
        IPacoteDecorator? decoratorAtual = null;

        foreach (var nomeDecorator in decorators)
        {
            // Cria o decorator apropriado baseado no nome
            // Cada decorator "embrulha" o pacote anterior
            switch (nomeDecorator.ToLower())
            {
                case "spa":
                    decoratorAtual = new SpaDecorator(pacote);
                    break;
                case "transfer":
                    decoratorAtual = new TransferDecorator(pacote);
                    break;
                case "latecheckout":
                    decoratorAtual = new LateCheckoutDecorator(pacote);
                    break;
                case "petfriendly":
                    decoratorAtual = new PetFriendlyDecorator(pacote);
                    break;
                default:
                    continue; // Decorator não reconhecido, pula
            }

            // Aplica o decorator (adiciona o serviço extra)
            decoratorAtual?.AdicionarServicoExtra();
        }

        // ============================================================
        // Passo 3: Calcula o valor total com decorators
        // ============================================================
        // Se houver decorators aplicados, usa o método do decorator
        // para calcular o valor total. Caso contrário, usa o método
        // padrão do pacote.
        //
        // IMPORTANTE - POLIMORFISMO EM AÇÃO:
        // Não precisamos saber qual decorator foi aplicado. Basta
        // chamar CalcularValorTotalComDecorators() e o polimorfismo
        // garante que o cálculo correto seja executado.
        // ============================================================
        int dias = (dataSaida - dataEntrada).Days;
        decimal valorTotal;

        if (decoratorAtual != null)
        {
            // Usa o cálculo do último decorator aplicado
            valorTotal = decoratorAtual.CalcularValorTotalComDecorators(dias);
        }
        else
        {
            // Usa o cálculo padrão do pacote (sem decorators)
            valorTotal = pacote.CalcularValorTotal(dias);
        }

        // ============================================================
        // Passo 4: Cria a reserva com o pacote decorado
        // ============================================================
        // IMPORTANTE - CORREÇÃO DE BUG:
        // Não podemos criar a reserva manualmente e depois chamar o gerenciador,
        // pois isso criaria duas reservas com IDs diferentes.
        // A solução é modificar o pacote para incluir o valor total calculado
        // com decorators, e deixar o gerenciador criar a reserva.
        //
        // Como o pacote já tem os serviços extras adicionados pelos decorators,
        // precisamos apenas garantir que o valor total seja calculado corretamente.
        // ============================================================
        
        // Verifica disponibilidade antes de criar
        if (!_gerenciador.QuartoDisponivel(tipoQuartoFinal, dataEntrada, dataSaida))
            return null;

        // Cria a reserva através do gerenciador
        // IMPORTANTE: O gerenciador não precisa saber sobre decorators.
        // Ele apenas recebe um PacoteHospedagem, que pode ou não ter
        // sido decorado. Isso é encapsulamento!
        var reserva = _gerenciador.CriarReservaWeb(hospedeNome, tipoQuartoFinal, dataEntrada, dataSaida, pacote,
            metodoPagamento, numeroCartao, cvv);
        
        // Se a reserva foi criada, atualiza o valor total com o cálculo dos decorators
        if (reserva != null)
        {
            reserva.ValorTotal = valorTotal;
        }

        return reserva;
    }
}
