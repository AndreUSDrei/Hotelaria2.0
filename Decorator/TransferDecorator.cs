using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Decorator;

// ============================================================
// PADRÃO DECORATOR - Decorator Concreto: TRANSFER
// ============================================================
// Decorator que adiciona serviço de transfer (aeroporto/hotel)
// ao pacote de hospedagem.
//
// IMPORTANTE - ENCADEAMENTO DE DECORATORS:
// Este decorator pode ser usado sozinho ou em conjunto com outros.
// Por exemplo:
// var pacote = new PacoteHospedagem();
// pacote = new SpaDecorator(pacote);
// pacote = new TransferDecorator(pacote);  // <- encadeia com Spa
//
// Cada decorator adiciona sua funcionalidade sem afetar os outros.
// Isso é o poder do padrão - composição flexível em runtime.
// ============================================================
public class TransferDecorator : PacoteDecoratorBase
{
    // Valor fixo do serviço de transfer (cada viagem)
    private const decimal VALOR_TRANSFER = 80.00m;

    public TransferDecorator(PacoteHospedagem pacote) : base(pacote)
    {
        // Construtor recebe o pacote e passa para a classe base
    }

    // ============================================================
    // IMPLEMENTAÇÃO ESPECÍFICA PARA TRANSFER
    // ============================================================
    // Adiciona o serviço de transfer ao pacote.
    // Note que a estrutura é idêntica ao SpaDecorator, mas o
    // conteúdo é específico para transfer.
    // ============================================================
    public override void AdicionarServicoExtra()
    {
        var servicoTransfer = new ServicoAdicional
        {
            Nome = "Transfer Privativo",
            Descricao = "Transfer aeroporto/hotel (ida e volta)",
            Preco = VALOR_TRANSFER
        };

        // Adiciona à lista de serviços do pacote
        _pacoteDecorado.Servicos.Add(servicoTransfer);

        // Atualiza descrição
        _pacoteDecorado.Descricao += " + Transfer Privativo";
    }

    // ============================================================
    // CÁLCULO COM TRANSFER
    // ============================================================
    // Diferente do spa que é cobrado por dia, o transfer é cobrado
    // uma única vez (ida e volta). Por isso não multiplicamos por dias.
    // ============================================================
    public override decimal CalcularValorTotalComDecorators(int dias)
    {
        // Valor base do pacote
        decimal valorBase = _pacoteDecorado.CalcularValorTotal(dias);

        // Adiciona valor do transfer (único, não por dia)
        decimal valorTransfer = VALOR_TRANSFER;

        return valorBase + valorTransfer;
    }
}
