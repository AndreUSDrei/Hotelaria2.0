namespace SistemaHotelaria.Services.Strategy;

public static class EstrategiaPagamentoFactory
{
    public static IEstrategiaPagamento Criar(string metodo, string? numeroCartao = null, string? cvv = null)
    {
        return (metodo ?? "pix").Trim().ToLower() switch
        {
            "cartao" or "cartão" or "cartaocredito" or "cartão de crédito" => new PagamentoCartaoCredito
            {
                NumeroCartao = string.IsNullOrWhiteSpace(numeroCartao) ? "4111 1111 1111 1111" : numeroCartao,
                Cvv = string.IsNullOrWhiteSpace(cvv) ? "123" : cvv
            },
            "boleto" => new PagamentoBoleto(),
            _ => new PagamentoPix()
        };
    }
}
