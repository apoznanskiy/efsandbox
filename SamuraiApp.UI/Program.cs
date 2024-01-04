using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

class Program
{
    private static SamuraiContext _samuraiContext = new SamuraiContext();
    //private static SamuraiContextNoTracking _samuraiContextNT = new SamuraiContextNoTracking();

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
        
        //QueryAndUpdateBattles_Disconnected();

        //InsertNewSamuraiWithAQuote();

        //EagerLoadSamuraiWithQuotes();

        //ProjectSomeProperties();

        //FilteringWithRelatedData();

        //ModifyingRelatedDataWhenNotTracked();

        //AddingNewSamuraiToAnExistingBattle();

        //ReturenBattleWithSamurais();

        //AddNewSamuraiWithHorse();


        var samurai = _samuraiContext.Samurais.FirstOrDefault();

        //samurai.Horse = new Horse() {Name = "Sorra"};

        //_samuraiContext.SaveChanges();

        GetSamurais(string.Empty);
    }


    private static void AddNewSamuraiWithHorse()
    {
        var samurai = new Samurai() {Name = "Viktor Surkov"};
        samurai.Horse = new Horse() {Name = "Silver"};

        _samuraiContext.Samurais.Add(samurai);
        _samuraiContext.SaveChanges();
    }

    private static void ReturenBattleWithSamurais()
    {
        var battles = _samuraiContext.Battles.Include(b => b.Samurais).ToList();
        var samurais = _samuraiContext.Samurais.ToList();
        foreach (var battle in battles)
        {
            battle.Samurais.AddRange(samurais);
        }

        _samuraiContext.SaveChanges();
    }

    private static void AddingNewSamuraiToAnExistingBattle()
    {
        var battle = _samuraiContext.Battles.FirstOrDefault();
        battle.Samurais.Add(new Samurai() { Name = "Taked Shingen" });
        _samuraiContext.SaveChanges();
    }

    private static void ModifyingRelatedDataWhenNotTracked()
    {
        var samurai = _samuraiContext.Samurais.Include(s => s.Quotes).Where(s => s.Quotes.Any(q => q.Text.Contains("you"))).FirstOrDefault();

        samurai.Quotes[0].Text = "Did you here that";
        var quote = samurai.Quotes[0];

        using var newContext = new SamuraiContext();
        //newContext.Quotes.Update(quote);
        newContext.Entry(quote).State = EntityState.Modified;

        newContext.SaveChanges();
    }

    private static void ModifyingRelatedDataWhenTracked()
    {
        var samurai = _samuraiContext.Samurais.Include(s => s.Quotes).Where(s => s.Quotes.Any(q => q.Text.Contains("you"))).FirstOrDefault();

        samurai.Quotes[0].Text = "Did you here that";

        _samuraiContext.SaveChanges();

    }

    private static void FilteringWithRelatedData()
    {
        var samurais = _samuraiContext.Samurais.Include(s => s.Quotes).Where(s => s.Quotes.Any(q => q.Text.Contains("you"))).ToList();

    }

    private static void ProjectSomeProperties()
    {
        var someProperties = _samuraiContext.Samurais.Select(s => new {Samurai = s , s.Name, NumberOfQuotes = s.Quotes.Count }).ToList();


        var firstSamurai = someProperties.First().Samurai.Name += "Samurai name";


    }

    private static void EagerLoadSamuraiWithQuotes()
    {
        //LEFT JOIN
        var samuraiWithQuotes = _samuraiContext.Samurais.Where(s => s.Name.Contains("Shi")).Include(s => s.Quotes).ToList();


    }

    private static void InsertNewSamuraiWithAQuote()
    {
        var samurai = new Samurai
        {
            Name = "Kambei Shimada",
            Quotes = new List<Quote>
            {
                new Quote {Text = "I've come to save you"}
            }
        };

        _samuraiContext.Samurais.Add(samurai);
        _samuraiContext.SaveChanges();
    }

    //private static void QueryAndUpdateBattles_Disconnected()
    //{
    //    List<Battle> disconnectedBattles;
    //    using (var context1 = new SamuraiContextNoTracking())
    //    {
    //        disconnectedBattles = _samuraiContext.Battles.ToList();
    //    }
        
    //    disconnectedBattles.ForEach(b =>
    //    {
    //        b.StartDate = new DateTime(1570, 1, 1);
    //        b.EndDate = new DateTime(1570, 1, 1);
    //    });

    //    using (var context2 = new SamuraiContextNoTracking())
    //    {
    //        context2.UpdateRange(disconnectedBattles);
    //        context2.SaveChanges();
    //    }
    //}

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