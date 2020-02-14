namespace WebApiTemplate.Repositories
{
    public class Repository : IRepository
    {
        public string MakeHello(string name) => $"Hello world: {name}";
    }
}
