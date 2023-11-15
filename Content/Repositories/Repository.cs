using System.Data.Common;

namespace WebApiTemplate.Repositories;

public class Repository(DbConnection connection) : IRepository
{
#pragma warning disable RCS1213 // Remove unused member declaration.
    private readonly DbConnection connection = connection;
#pragma warning restore RCS1213 // Remove unused member declaration.

    public string MakeHello(string name) => $"Hello world: {name}";
}
