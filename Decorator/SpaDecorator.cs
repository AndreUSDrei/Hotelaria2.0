using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Decorator;

// ============================================================
// PADRÃO DECORATOR - Decorator Concreto: SPA
// ============================================================
// Este é um decorator concreto que adiciona o serviço de SPA
// ao pacote de hospedagem.
//
// IMPORTANTE - POLIMORFISMO:
// Esta classe herda de PacoteDecoratorBase (abstrata) e 
// implementa o método AdicionarServicoExtra() de forma específica
// para o serviço de SPA.
//
// POLIMORFISMO = capacidade de objetos de diferentes classes
// responderem à mesma mensagem de formas diferentes. Aqui,
// cada decorator responde a AdicionarServicoExtra() de forma
// única (Spa adiciona serviço de spa, Transfer adiciona transfer, etc.)
// ============================================================
public class SpaDecorator : PacoteDecoratorBase
{
    // Valor fixo do serviço de spa por dia
    private const decimal VALOR_SPA_POR_DIA = 150.00m;

    public SpaDecorator(PacoteHospedagem pacote) : base(pacote)
    {
        // Chama o construtor da classe base passando o pacote
        // Isso é necessário para a composição funcionar
    }

    // ============================================================
    // IMPLEMENTAÇÃO DO MÉTODO ABSTRATO
    // ============================================================
    // Aqui está a lógica específica para adicionar o serviço de SPA.
    // Cada decorator concreto tem sua própria implementação.
    // ============================================================
    public override void AdicionarServicoExtra()
    {
        // Cria um novo serviço adicional do tipo SPA
        var servicoSpa = new ServicoAdicional
        {
            Nome = "Spa & Bem-estar",
            Descricao = "Acesso completo ao spa com massagem relaxante",
            Preco = VALOR_SPA_POR_DIA
        };

        // Adiciona o serviço à lista de serviços do pacote
        // IMPORTANTE: Estamos modificando o objeto original, mas
        // de forma controlada através do decorator
        _pacoteDecorado.Servicos.Add(servicoSpa);

        // Atualiza a descrição do pacote para incluir o spa
        _pacoteDecorado.Descricao += " + Spa & Bem-estar";
    }

    // ============================================================
    // SOBRESCRITA DO MÉTODO DE CÁLCULO
    // ============================================================
    // Sobrescreve o método de cálculo para incluir o valor do spa.
    // SOBRESCRITA (override) = substituir a implementação da classe
    // base por uma implementação específica.
    // ============================================================
    public override decimal CalcularValorTotalComDecorators(int dias)
    {
        // Primeiro calcula o valor base do pacote (sem o spa)
        decimal valorBase = _pacoteDecorado.CalcularValorTotal(dias);

        // Adiciona o valor do spa (valor por dia * número de dias)
        decimal valorSpa = VALOR_SPA_POR_DIA * dias;

        // Retorna o total com o spa incluído
        return valorBase + valorSpa;
    }
}
