using SistemaHotelaria.Builder;
using SistemaHotelaria.Models;

namespace SistemaHotelaria.Decorator;

// ============================================================
// PADRÃO DECORATOR - Classe Base Abstrata
// ============================================================
// Esta é a classe base que todos os decorators concretos 
// vão herdar. Ela implementa a composição (COMPOSIÇÃO = 
// um objeto tem outro objeto, não é herança).
//
// IMPORTANTE - COMPOSIÇÃO vs HERANÇA:
// - Herança: "é um" (ex: Cachorro é um Animal)
// - Composição: "tem um" (ex: Carro tem um Motor)
//
// No Decorator, usamos COMPOSIÇÃO porque queremos "embrulhar"
// o objeto original em camadas. Cada decorator tem uma referência
// ao objeto que está decorando (o _pacoteDecorado).
//
// Isso permite que encadeemos decorators:
// pacoteBase -> spaDecorator -> transferDecorator -> lateCheckoutDecorator
// ============================================================
public abstract class PacoteDecoratorBase : IPacoteDecorator
{
    // O pacote que está sendo decorado (pode ser o pacote original
    // ou outro decorator que já adicionou serviços)
    protected PacoteHospedagem _pacoteDecorado;

    // Construtor recebe o pacote que vai ser decorado
    // Isso é o coração do padrão - cada decorator "conhece" o objeto
    // que está decorando
    public PacoteDecoratorBase(PacoteHospedagem pacote)
    {
        _pacoteDecorado = pacote;
    }

    // Método da interface - retorna o pacote (possivelmente modificado)
    public virtual PacoteHospedagem ObterPacoteDecorado()
    {
        return _pacoteDecorado;
    }

    // Método abstrato - cada decorator concreto implementa sua própria
    // lógica de adição de serviço
    public abstract void AdicionarServicoExtra();

    // Calcula o valor total considerando os decorators
    // IMPORTANTE: Este método delega o cálculo para o pacote decorado,
    // então cada decorator pode adicionar seu próprio valor extra
    public virtual decimal CalcularValorTotalComDecorators(int dias)
    {
        return _pacoteDecorado.CalcularValorTotal(dias);
    }
}
