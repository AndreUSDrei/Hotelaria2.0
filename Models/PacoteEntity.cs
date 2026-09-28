using System.ComponentModel.DataAnnotations;

namespace SistemaHotelaria.Models;

public class PacoteEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal DescontoPercentual { get; set; }

    public virtual ICollection<RefeicaoEntity> Refeicoes { get; set; } = new List<RefeicaoEntity>();
    public virtual ICollection<ServicoAdicionalEntity> ServicosAdicionais { get; set; } = new List<ServicoAdicionalEntity>();
}
