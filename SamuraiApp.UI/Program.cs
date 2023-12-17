using SamuraiApp.Data;
using SamuraiApp.Domain;

class Program
{

    private static SamuraiContext _samuraiContext = new SamuraiContext();

    static void Main(string[] args)
    {
        _samuraiContext.Database.EnsureCreated();

        GetSamurais("Before Add:");

        AddSamurai();

        GetSamurais("After Add:");
    }

    static void AddSamurai()
    {
        var samurai = new Samurai() {Name = "Andrey"};
        _samuraiContext.Samurais.Add(samurai);
        _samuraiContext.SaveChanges();
    }

    static void GetSamurais(string text)
    {
        var samurais = _samuraiContext.Samurais.ToList();
        Console.WriteLine($"{text} total samurais: {samurais.Count}");

        foreach (var samurai in samurais)
        {
            Console.WriteLine($"Id: {samurai.Id} Name: {samurai.Name}");
        }
    }

}