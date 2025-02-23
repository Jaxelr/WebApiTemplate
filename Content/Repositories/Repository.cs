using System.Data.Common;

namespace WebApiTemplate.Repositories;

public class Repository(DbConnection connection) : IRepository
{
    private readonly DbConnection connection = connection;

    public string MakeHello(string name) => $"Hello world: {name}";
}
