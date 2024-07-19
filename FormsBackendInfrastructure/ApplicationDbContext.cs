using Microsoft.Data.Sqlite;

namespace FormsBackendInfrastructure;

public class ApplicationDbContext
{
    public SqliteConnection Connection { get; } = new SqliteConnection("FileName=forms.db");

    public ApplicationDbContext()
    {
        Connection.Open();
    }
}
