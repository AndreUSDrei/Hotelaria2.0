using SistemaHotelaria.Builder;

namespace SistemaHotelaria.Decorator;

// ============================================================
// PADRÃO DECORATOR - Interface Base
// ============================================================
// O padrão Decorator permite adicionar responsabilidades 
// dinamicamente a um objeto sem alterar sua classe.
// 
// IMPORTANTE: Esta interface estende a funcionalidade do 
// PacoteHospedagem permitindo que sejam adicionados serviços
// extras em tempo de execução (execução dinâmica = em tempo 
// de execução do programa, não em tempo de compilação).
// 
// Diferença do Builder: O Builder CONSTRÓI o objeto completo
// de uma vez. O Decorator ADICIONA comportamentos 
// incrementalmente, um por vez, como se estivesse "embrulhando"
// o objeto em camadas.
// ============================================================
public interface IPacoteDecorator
{
    // Retorna o pacote decorado (com os serviços extras adicionados)
    PacoteHospedagem ObterPacoteDecorado();
    
    // Adiciona um serviço extra ao pacote
    // Este é o método principal do padrão - cada decorator
    // implementa sua própria lógica de adição
    void AdicionarServicoExtra();
    
    // Calcula o valor total considerando os serviços extras
    decimal CalcularValorTotalComDecorators(int dias);
}
