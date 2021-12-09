using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestDI.Services.Interfaces;

namespace TestDI.Services;

internal class TestCSharp9Service : ITestCSharp9Service
{
    private readonly ILogger<TestCSharp9Service> _logger;

    public TestCSharp9Service(ILogger<TestCSharp9Service> logger)
    {
        _logger = logger;
    }

    public void Test()
    {
        _logger.LogInformation("----------- C# 9 -----------");
        _logger.LogInformation("Step01 Ran");
        Step01.Run();
        _logger.LogInformation("Step02 Ran");
        Step02.Run();
        _logger.LogInformation("Step03 Ran");
        Step03.Run();
        _logger.LogInformation("Step04 Ran");
        Step04.Run();
    }

    private class Step01
    {
        public static void Run()
        {
            foreach (var item in (1, 2, 3, 4, 5))
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine("");
        }
    }
    private class Step02
    {
        public static void Run()
        {
            string x = "test";
            int y = 123;
            object obj = new Random().Next(1) == 0 ? x : y;
            
            if (obj is not int)
            {
                Console.WriteLine("is not int");
            }
            else
            {
                Console.WriteLine("is int");
            }
        }
    }

    private class Step03
    {
        record TestHat(string Name, int Size);
        record TestRecord(string Name, string Surname, int Age, TestHat hat);
        public static void Run()
        {
            TestRecord rec1 = new("Andrey", "Savkov", 20, new("shapka", 5));

            // if (rec1.Name == "Andrey" && rec1.Age == 20 && rec1.hat.Size >= 5 && rec1.hat.Size < 8 && rec1.Surname.Contains("avk"))
            if (rec1 is {Name: "Andrey", Age: 20, hat.Size: >= 5 and < 8} && rec1.Surname.Contains("avk"))
            {
                Console.WriteLine(rec1);
            }
        }
    }

    private class Step04
    {
        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }

        private static bool IsWeekDay(DateTime timeOfToll) =>
            timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true
            };

        private static TimeBand GetTimeBand(DateTime timeOfToll) =>
            timeOfToll.Hour switch
            {
                < 6 or > 19 => TimeBand.Overnight,
                < 10 => TimeBand.MorningRush,
                < 16 => TimeBand.Daytime,
                _ => TimeBand.EveningRush,
            };

        public static decimal PeakTimePremiumFull(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true, TimeBand.Overnight, _) => 0.75m,
                (true, TimeBand.Daytime, _) => 1.5m,
                (true, TimeBand.MorningRush, true) => 2.0m,
                (true, TimeBand.EveningRush, false) => 2.0m,
                _ => 1.0m,
            };
        public static void Run()
        {
            DateTime date = new(2021, 12, 07);
            if (date is {
                    DayOfWeek: DayOfWeek.Friday or DayOfWeek.Sunday,
                    Day: > 5 and <= 6
                })
            {
            }

            Console.WriteLine(PeakTimePremiumFull(date.AddHours(0), true));
            Console.WriteLine(PeakTimePremiumFull(date.AddHours(15), true));
            Console.WriteLine(PeakTimePremiumFull(date.AddHours(8), true));
            Console.WriteLine(PeakTimePremiumFull(date.AddHours(18), false));

            Console.WriteLine(PeakTimePremiumFull(date.AddHours(18), true));
        }
    }
}

public static class MyExtensions
{
    public static IEnumerator<T> GetEnumerator<T>(this ValueTuple<T, T, T, T, T> source)
    {
        yield return source.Item1;
        yield return source.Item2;
        yield return source.Item3;
        yield return source.Item4;
        yield return source.Item5;
    }
}