using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Builder;

public class HotelDirector
{
    private readonly IPacoteHospedagemBuilder _builder;

    public HotelDirector(IPacoteHospedagemBuilder builder)
    {
        _builder = builder;
    }

    public void ConstruirPacoteRomanticoCompleto(IQuarto quarto)
    {
        _builder.Reset();
        _builder.DefinirNome("Pacote Romântico Completo")
                .DefinirDescricao("Experiência perfeita para casais - inclui tudo para uma estadia inesquecível")
                .SelecionarQuarto(quarto)
                .AdicionarCafeDaManha()
                .AdicionarAlmoco()
                .AdicionarJantar()
                .AdicionarServico("Spa para Casal", "Massagem relaxante de 1h", 350.00m)
                .AdicionarServico("Decoração Romântica", "Pétalas de rosas e velas", 150.00m)
                .AdicionarServico("Late Check-out", "Saída até 16h", 100.00m)
                .AplicarDesconto(10);
    }

    public void ConstruirPacoteNegociosCompleto(IQuarto quarto)
    {
        _builder.Reset();
        _builder.DefinirNome("Pacote Business Executivo")
                .DefinirDescricao("Solução completa para viajantes corporativos")
                .SelecionarQuarto(quarto)
                .AdicionarCafeDaManha()
                .AdicionarAlmoco()
                .AdicionarServico("Sala de Reuniões", "Acesso por 2h", 200.00m)
                .AdicionarServico("Lavanderia Express", "Limpeza 24h", 80.00m)
                .AdicionarServico("Transfer Executivo", "Ida e volta aeroporto", 250.00m)
                .AplicarDesconto(15);
    }

    public void ConstruirPacoteBasico(IQuarto quarto)
    {
        _builder.Reset();
        _builder.DefinirNome("Pacote Básico")
                .DefinirDescricao("Hospedagem econômica com café da manhã")
                .SelecionarQuarto(quarto)
                .AdicionarCafeDaManha();
    }

    public void ConstruirPacoteFimDeSemana(IQuarto quarto)
    {
        _builder.Reset();
        _builder.DefinirNome("Pacote Fim de Semana")
                .DefinirDescricao("Desconte e relaxe no fim de semana")
                .SelecionarQuarto(quarto)
                .AdicionarCafeDaManha()
                .AdicionarJantar()
                .AdicionarServico("Acesso Academia", "Uso ilimitado", 50.00m)
                .AplicarDesconto(20);
    }

    public PacoteHospedagem ObterPacote()
    {
        return _builder.Build();
    }
}
