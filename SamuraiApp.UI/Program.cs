using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

class Program
{
    private static SamuraiContext _samuraiContext = new SamuraiContext();
    private static SamuraiContextNoTracking _samuraiContextNT = new SamuraiContextNoTracking();

    static void Main(string[] args)
    {
        _samuraiContext.Database.EnsureCreated();

        //GetSamurais("Before Add:");

        //AddSamurai();

        //GetSamurais("After Add:");

        //QueryFilters();
        
        //QueryAggregates();
        
        //RetrieveAndUpdateSamurai();
        
        //RetrieveAndUpdateMultipleSamurais();
        
        //RetrieveAndDeleteSamurais();
        
        QueryAndUpdateBattles_Disconnected();
        
        GetSamurais(string.Empty);
    }

    private static void QueryAndUpdateBattles_Disconnected()
    {
        List<Battle> disconnectedBattles;
        using (var context1 = new SamuraiContextNoTracking())
        {
            disconnectedBattles = _samuraiContext.Battles.ToList();
        }
        
        disconnectedBattles.ForEach(b =>
        {
            b.StartDate = new DateTime(1570, 1, 1);
            b.EndDate = new DateTime(1570, 1, 1);
        });

        using (var context2 = new SamuraiContextNoTracking())
        {
            context2.UpdateRange(disconnectedBattles);
            context2.SaveChanges();
        }
    }

    private static void RetrieveAndDeleteSamurais()
    {
        var samurai = _samuraiContext.Samurais.FirstOrDefault();
        _samuraiContext.Samurais.Remove(samurai);
        _samuraiContext.SaveChanges();
    }

    private static void RetrieveAndUpdateMultipleSamurais()
    {
        var samurais = _samuraiContext.Samurais.Skip(1).Take(4).ToList();
        samurais.ForEach(s => s.Name += "San");
        _samuraiContext.SaveChanges();
    }

    private static void RetrieveAndUpdateSamurai()
    {
        var samurai = _samuraiContext.Samurais.FirstOrDefault();

        samurai.Name += "Sir ";

        _samuraiContext.SaveChanges();
    }

    private static void QueryFilters()
    {
        var samuraisCount = _samuraiContext.Samurais
            .Where(s => EF.Functions.Like(s.Name, "A%")).Count();
        
        Console.WriteLine($"Samurais count: {samuraisCount}");
    }

    private static void QueryAggregates()
    {
        var name = "Andrey1";
        var samurai = _samuraiContext.Samurais.FirstOrDefault(s => s.Name == name);
    }

    static void AddSamurai()
    {
        _samuraiContext.AddRange(
            new Samurai() {Name = "Andrey1"},
            new Samurai() {Name = "Andrey2"},
            new Samurai() {Name = "Andrey3"},
            new Samurai() {Name = "Andrey4"}
        );

        _samuraiContext.SaveChanges();
    }

    static void GetSamurais(string text)
    {
        var samurais = _samuraiContext.Samurais.TagWith("Get samurais method").ToList();
        Console.WriteLine($"{text} total samurais: {samurais.Count}");

        foreach (var samurai in samurais)
        {
            Console.WriteLine($"Id: {samurai.Id} Name: {samurai.Name}");
        }
    }
}