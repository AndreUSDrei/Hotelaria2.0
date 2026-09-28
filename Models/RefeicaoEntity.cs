using System.ComponentModel.DataAnnotations;

namespace SistemaHotelaria.Models;

public class RefeicaoEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Horario { get; set; } = string.Empty;
    public decimal Preco { get; set; }

    public virtual ICollection<PacoteEntity> Pacotes { get; set; } = new List<PacoteEntity>();
}
