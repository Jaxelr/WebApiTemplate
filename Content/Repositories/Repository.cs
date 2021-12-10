using System.Data.Common;

namespace WebApiTemplate.Repositories;

public class Repository : IRepository
{
    private readonly DbConnection connection;

    public Repository(DbConnection connection)
    {
        this.connection = connection;
    }

    public string MakeHello(string name) => $"Hello world: {name}";
}
