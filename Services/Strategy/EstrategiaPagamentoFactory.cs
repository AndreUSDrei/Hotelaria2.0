namespace SistemaHotelaria.Services.Strategy;

public static class EstrategiaPagamentoFactory
{
    public static IEstrategiaPagamento Criar(string metodo)
    {
        return metodo.ToLower() switch
        {
            "pix" => new PagamentoPix(),
            "cartao" => new PagamentoCartaoCredito(),
            "boleto" => new PagamentoBoleto(),
            _ => new PagamentoPix()
        };
    }
}
