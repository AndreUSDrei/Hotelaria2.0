namespace SistemaHotelaria.Services.Proxies;

/// <summary>
/// Proxy: cache de consultas de disponibilidade para reduzir recálculos em intervalos curtos.
/// </summary>
public class CachingDisponibilidadeProxy : IGerenciadorReservas
{
    private readonly IGerenciadorReservas _real;
    private readonly Dictionary<string, (DateTime ExpiraEm, object Valor)> _cache = new();
    private static readonly TimeSpan CacheDuracao = TimeSpan.FromSeconds(30);

    public CachingDisponibilidadeProxy(IGerenciadorReservas real)
    {
        _real = real;
    }

    public bool QuartoDisponivel(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        var chave = $"disp:{tipoQuarto}:{dataEntrada:O}:{dataSaida:O}";
        if (TryObterCache<bool>(chave, out var valor))
            return valor;

        valor = _real.QuartoDisponivel(tipoQuarto, dataEntrada, dataSaida);
        ArmazenarCache(chave, valor);
        return valor;
    }

    public int ObterQuantidadeQuartosDisponiveis(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        var chave = $"qtd:{tipoQuarto}:{dataEntrada:O}:{dataSaida:O}";
        if (TryObterCache<int>(chave, out var valor))
            return valor;

        valor = _real.ObterQuantidadeQuartosDisponiveis(tipoQuarto, dataEntrada, dataSaida);
        ArmazenarCache(chave, valor);
        return valor;
    }

    public Dictionary<string, int> ObterDisponibilidadeCompleta(DateTime entrada, DateTime saida)
    {
        var chave = $"compl:{entrada:O}:{saida:O}";
        if (TryObterCache<Dictionary<string, int>>(chave, out var valor) && valor != null)
            return valor;

        valor = _real.ObterDisponibilidadeCompleta(entrada, saida);
        ArmazenarCache(chave, valor);
        return valor;
    }

    public Models.Reserva? CriarReserva(string nomeHospede, Prototype.IQuarto quartoBase, Builder.IPacoteHospedagemBuilder builder,
        Builder.HotelDirector director, DateTime entrada, DateTime saida)
    {
        InvalidarCacheDisponibilidade();
        return _real.CriarReserva(nomeHospede, quartoBase, builder, director, entrada, saida);
    }

    public bool RealizarCheckIn(string idReserva) => _real.RealizarCheckIn(idReserva);

    public bool RealizarCheckOut(string idReserva)
    {
        var ok = _real.RealizarCheckOut(idReserva);
        if (ok)
            InvalidarCacheDisponibilidade();
        return ok;
    }

    public void ListarReservas() => _real.ListarReservas();

    public void ExibirDisponibilidade(DateTime dataEntrada, DateTime dataSaida) =>
        _real.ExibirDisponibilidade(dataEntrada, dataSaida);

    public List<Models.Reserva> ObterTodasReservas() => _real.ObterTodasReservas();

    public Models.Reserva? ObterReservaPorId(string id) => _real.ObterReservaPorId(id);

    public Models.Reserva? CriarReservaWeb(string nomeHospede, string tipoQuarto, DateTime entrada, DateTime saida, Builder.PacoteHospedagem pacote,
        string metodoPagamento = "Pix", string? numeroCartao = null, string? cvv = null)
    {
        InvalidarCacheDisponibilidade();
        return _real.CriarReservaWeb(nomeHospede, tipoQuarto, entrada, saida, pacote, metodoPagamento, numeroCartao, cvv);
    }

    private bool TryObterCache<T>(string chave, out T valor)
    {
        valor = default!;
        if (!_cache.TryGetValue(chave, out var item) || item.ExpiraEm < DateTime.UtcNow)
            return false;

        valor = (T)item.Valor;
        return true;
    }

    private void ArmazenarCache<T>(string chave, T valor) =>
        _cache[chave] = (DateTime.UtcNow.Add(CacheDuracao), valor!);

    private void InvalidarCacheDisponibilidade() => _cache.Clear();
}
