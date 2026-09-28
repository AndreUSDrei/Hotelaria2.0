using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SistemaHotelaria.Services.Persistence;

/// <summary>
/// Garante schema em 3ª Forma Normal: recria o SQLite se ainda estiver no modelo antigo
/// (colunas denormalizadas / JSON) e faz seed dos tipos de quarto.
/// </summary>
public static class DatabaseNormalizer
{
    public static void GarantirSchema3FN(ApplicationDbContext db)
    {
        if (SchemaAntigo(db))
            db.Database.EnsureDeleted();

        db.Database.EnsureCreated();
        SeedTiposQuarto(db);
    }

    private static bool SchemaAntigo(ApplicationDbContext db)
    {
        try
        {
            var connection = db.Database.GetDbConnection();
            var wasOpen = connection.State == System.Data.ConnectionState.Open;
            if (!wasOpen)
                connection.Open();

            try
            {
                using var cmd = connection.CreateCommand();

                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Reservas'";
                if (cmd.ExecuteScalar() == null)
                    return false;

                cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('Reservas') WHERE name='EventosReserva'";
                var temJson = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                if (temJson)
                    return true;

                cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Pagamentos'";
                return cmd.ExecuteScalar() == null;
            }
            finally
            {
                if (!wasOpen)
                    connection.Close();
            }
        }
        catch (SqliteException)
        {
            return true;
        }
        catch
        {
            return true;
        }
    }

    private static void SeedTiposQuarto(ApplicationDbContext db)
    {
        string[] tipos = ["Standard", "Luxo", "Suíte Presidencial"];

        foreach (var nome in tipos)
        {
            if (!db.TiposQuarto.Any(t => t.Nome == nome))
                db.TiposQuarto.Add(new Models.TipoQuartoEntity { Nome = nome });
        }

        db.SaveChanges();
    }
}
