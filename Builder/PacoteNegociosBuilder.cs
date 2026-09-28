using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Builder;

public class PacoteNegociosBuilder : IPacoteHospedagemBuilder
{
    private PacoteHospedagem _pacote = new();

    public IPacoteHospedagemBuilder DefinirNome(string nome)
    {
        _pacote.Nome = nome;
        return this;
    }

    public IPacoteHospedagemBuilder DefinirDescricao(string descricao)
    {
        _pacote.Descricao = descricao;
        return this;
    }

    public IPacoteHospedagemBuilder SelecionarQuarto(IQuarto quarto)
    {
        _pacote.Quarto = quarto.Clone();
        return this;
    }

    public IPacoteHospedagemBuilder AdicionarCafeDaManha()
    {
        _pacote.Refeicoes.Add(new Refeicao
        {
            Nome = "Café Expresso Executivo",
            Descricao = "Café rápido para executivos",
            Horario = "06:00 - 10:00",
            Preco = 45.00m
        });
        return this;
    }

    public IPacoteHospedagemBuilder AdicionarAlmoco()
    {
        _pacote.Refeicoes.Add(new Refeicao
        {
            Nome = "Business Lunch",
            Descricao = "Almoço executivo com menu rápido",
            Horario = "11:30 - 14:30",
            Preco = 85.00m
        });
        return this;
    }

    public IPacoteHospedagemBuilder AdicionarJantar()
    {
        _pacote.Refeicoes.Add(new Refeicao
        {
            Nome = "Jantar Corporativo",
            Descricao = "Opções leves para executivos",
            Horario = "18:00 - 22:00",
            Preco = 95.00m
        });
        return this;
    }

    public IPacoteHospedagemBuilder AdicionarServico(string nome, string descricao, decimal preco)
    {
        _pacote.Servicos.Add(new ServicoAdicional
        {
            Nome = nome,
            Descricao = descricao,
            Preco = preco
        });
        return this;
    }

    public IPacoteHospedagemBuilder AplicarDesconto(decimal percentual)
    {
        _pacote.DescontoPercentual = percentual;
        return this;
    }

    public PacoteHospedagem Build()
    {
        var resultado = _pacote;
        Reset();
        return resultado;
    }

    public void Reset()
    {
        _pacote = new PacoteHospedagem();
    }
}
