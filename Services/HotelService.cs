using SistemaHotelaria.Builder;
using SistemaHotelaria.Prototype;

namespace SistemaHotelaria.Services;

public class HotelService
{
    private readonly List<IQuarto> _prototipos = new();

    public HotelService()
    {
        // Inicializar protótipos
        _prototipos.Add(new QuartoStandard());
        _prototipos.Add(new QuartoLuxo());
        _prototipos.Add(new Suite());
    }

    public List<IQuarto> ObterPrototiposQuartos() => _prototipos;

    public IQuarto? ObterPrototipoPorTipo(string tipo)
    {
        return _prototipos.FirstOrDefault(q => q.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase));
    }

    public IPacoteHospedagemBuilder CriarBuilder(string tipoPacote)
    {
        return tipoPacote switch
        {
            "Romantico" => new PacoteRomanticoBuilder(),
            "Negocios" => new PacoteNegociosBuilder(),
            _ => new PacoteRomanticoBuilder()
        };
    }

    public List<object> ObterTiposQuartosInfo()
    {
        return _prototipos.Select(q => new
        {
            Tipo = q.Tipo,
            Descricao = q.Descricao,
            Capacidade = q.Capacidade,
            PrecoBaseDiaria = q.PrecoBaseDiaria,
            Comodidades = q.Comodidades
        }).ToList<object>();
    }
}
