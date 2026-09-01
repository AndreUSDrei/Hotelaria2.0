using System.ComponentModel.DataAnnotations;

namespace SistemaHotelaria.Models;

public class TipoQuartoEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
}
