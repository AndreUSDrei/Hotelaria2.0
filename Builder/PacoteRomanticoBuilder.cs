using SistemaHotelaria.Models;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Builder;

public class PacoteRomanticoBuilder : IPacoteHospedagemBuilder
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
            Nome = "Café da Manhã Romântico",
            Descricao = "Servido no quarto com champagne e frutas",
            Horario = "08:00 - 11:00",
            Preco = 120.00m
        });
        return this;
    }

    public IPacoteHospedagemBuilder AdicionarAlmoco()
    {
        _pacote.Refeicoes.Add(new Refeicao
        {
            Nome = "Almoço à La Carte",
            Descricao = "Menu especial do chef para casais",
            Horario = "12:00 - 15:00",
            Preco = 180.00m
        });
        return this;
    }

    public IPacoteHospedagemBuilder AdicionarJantar()
    {
        _pacote.Refeicoes.Add(new Refeicao
        {
            Nome = "Jantar à Luz de Velas",
            Descricao = "Experiência gastronômica romântica",
            Horario = "19:00 - 23:00",
            Preco = 250.00m
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
