using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Decorator;

// ============================================================
// PADRÃO DECORATOR - Decorator Concreto: PET FRIENDLY
// ============================================================
// Decorator que adiciona a opção de permitir pets no quarto.
//
// IMPORTANTE - VALIDAÇÃO NO DECORATOR:
// Este decorator demonstra que o padrão pode incluir lógica
// de validação ou regras de negócio específicas, não apenas
// adição de serviços.
// ============================================================
public class PetFriendlyDecorator : PacoteDecoratorBase
{
    // Valor diário extra por pet
    private const decimal VALOR_PET_POR_DIA = 30.00m;

    public PetFriendlyDecorator(PacoteHospedagem pacote) : base(pacote)
    {
        // Construtor padrão
    }

    // ============================================================
    // ADICIONA SERVIÇO PET FRIENDLY
    // ============================================================
    public override void AdicionarServicoExtra()
    {
        var servicoPet = new ServicoAdicional
        {
            Nome = "Pet Friendly",
            Descricao = "Permite 1 pet no quarto (cama e comedouro inclusos)",
            Preco = VALOR_PET_POR_DIA
        };

        // Adiciona o serviço
        _pacoteDecorado.Servicos.Add(servicoPet);

        // Atualiza descrição
        _pacoteDecorado.Descricao += " + Pet Friendly";
    }

    // ============================================================
    // CÁLCULO COM PET FRIENDLY
    // ============================================================
    // Pet friendly é cobrado por dia, então multiplicamos pelo
    // número de dias da estadia.
    // ============================================================
    public override decimal CalcularValorTotalComDecorators(int dias)
    {
        decimal valorBase = _pacoteDecorado.CalcularValorTotal(dias);
        decimal valorPet = VALOR_PET_POR_DIA * dias;

        return valorBase + valorPet;
    }
}
