using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Builder;

public interface IPacoteHospedagemBuilder
{
    IPacoteHospedagemBuilder DefinirNome(string nome);
    IPacoteHospedagemBuilder DefinirDescricao(string descricao);
    IPacoteHospedagemBuilder SelecionarQuarto(IQuarto quarto);
    IPacoteHospedagemBuilder AdicionarCafeDaManha();
    IPacoteHospedagemBuilder AdicionarAlmoco();
    IPacoteHospedagemBuilder AdicionarJantar();
    IPacoteHospedagemBuilder AdicionarServico(string nome, string descricao, decimal preco);
    IPacoteHospedagemBuilder AplicarDesconto(decimal percentual);
    PacoteHospedagem Build();
    void Reset();
}
