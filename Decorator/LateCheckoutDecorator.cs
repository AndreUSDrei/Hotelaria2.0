using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Decorator;

// ============================================================
// PADRÃO DECORATOR - Decorator Concreto: LATE CHECKOUT
// ============================================================
// Decorator que adiciona a opção de late checkout (saída tardia)
// ao pacote de hospedagem.
//
// IMPORTANTE - FLEXIBILIDADE DO PADRÃO:
// O late checkout é um exemplo de serviço que não é um "item"
// físico como spa ou transfer, mas sim uma modificação de regra
// de negócio. O padrão Decorator lida bem com isso porque
// cada decorator pode implementar qualquer tipo de modificação.
// ============================================================
public class LateCheckoutDecorator : PacoteDecoratorBase
{
    // Valor extra pelo late checkout (taxa única)
    private const decimal VALOR_LATE_CHECKOUT = 50.00m;

    public LateCheckoutDecorator(PacoteHospedagem pacote) : base(pacote)
    {
        // Construtor padrão do decorator
    }

    // ============================================================
    // ADICIONA SERVIÇO DE LATE CHECKOUT
    // ============================================================
    public override void AdicionarServicoExtra()
    {
        var servicoLateCheckout = new ServicoAdicional
        {
            Nome = "Late Checkout",
            Descricao = "Saída tardia até às 14:00 (padrão: 12:00)",
            Preco = VALOR_LATE_CHECKOUT
        };

        // Adiciona o serviço
        _pacoteDecorado.Servicos.Add(servicoLateCheckout);

        // Atualiza descrição
        _pacoteDecorado.Descricao += " + Late Checkout";
    }

    // ============================================================
    // CÁLCULO COM LATE CHECKOUT
    // ============================================================
    // Late checkout é uma taxa única, independente do número de dias.
// ============================================================
    public override decimal CalcularValorTotalComDecorators(int dias)
    {
        decimal valorBase = _pacoteDecorado.CalcularValorTotal(dias);
        decimal valorLateCheckout = VALOR_LATE_CHECKOUT;

        return valorBase + valorLateCheckout;
    }
}
