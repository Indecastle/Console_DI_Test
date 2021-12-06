using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestDI.Services.Interfaces;

namespace TestDI.Services;

internal class TestCSharp10Service : ITestCSharp10Service
{
    private readonly ILogger<TestCSharp10Service> _logger;

    public TestCSharp10Service(ILogger<TestCSharp10Service> logger)
    {
        _logger = logger;
    }

    public void Test()
    {
        _logger.LogInformation("Step01 Ran");
        Step01.Run();
        _logger.LogInformation("Step02 Ran");
        Step02.Run();
        _logger.LogInformation("Step03 Ran");
        Step03.Run();
        _logger.LogInformation("Step04 Ran");
        Step04.Run();
        _logger.LogInformation("Step05 Ran");
        Step05.Run();
    }


    private class Step01
    {
        record class TestClass(string Name, string Surname);
        record TestClass2(string Name, string Surname);
        record struct TestStruct(string Name, string Surname);
        readonly record struct TestStructReadOnly(string Name, string Surname);

        static void Test1()
        {
            var firstRecord = new TestStruct("Nick", "Smith");
            var secondRecord = new TestStruct("Robert", "Smith");
            var thirdRecord = new TestStruct("Nick", "Smith");

            Console.WriteLine(firstRecord == secondRecord);
            // False
            Console.WriteLine(firstRecord == thirdRecord);
            // True

            var otherRecord = firstRecord with {Name = "John"};
            Console.WriteLine(firstRecord == thirdRecord);
            // True
        }
        public static void Run()
        {
            Test1();
        }
    }
    private class Step02
    {
        private record TestRec(string name, string surname)
        {
            public override string ToString()
            {
                return $"{name} {surname}";
            }
        }
        private record TestRec2(string name, string surname)
        {
            public sealed override string ToString()
            {
                return $"{name} {surname}";
            }
        }
        private record InheritedRecord(string name, string surname) : TestRec(name, surname);
        private record InheritedRecord2(string name, string surname) : TestRec2(name, surname);

        private static void Test1()
        {
            TestRec myObj = new("Alex", "Johnson");
            Console.WriteLine(myObj.ToString());
            // Alex Johnson

            InheritedRecord mySecObj = new("Thomas", "Brown");
            Console.WriteLine(mySecObj.ToString());
            // inheritedRecord { name = Thomas, surname = Brown}
        }
        private static void Test2()
        {
            TestRec2 myObj = new("Alex", "Johnson");
            Console.WriteLine(myObj.ToString());
            // Alex Johnson

            InheritedRecord2 mySecObj = new("Thomas", "Brown");
            Console.WriteLine(mySecObj.ToString());
            // inheritedRecord { name = Thomas, surname = Brown}
        }
        public static void Run()
        {
            Test1();
            Test2();
        }
    }

    private class Step03
    {
        record TestRec(string name, string surname);

        static string TakeFourSymbols(TestRec obj) => obj switch
        {
            // старый способ:
            //TestRec { name: {Length: > 4} } rec => rec.name.Substring(0,4),

            // новый способ:
            TestRec { name.Length: > 4 } rec => rec.name.Substring(0, 4),
            TestRec rec => rec.name,
        };
        public static void Run()
        {
            var result = TakeFourSymbols(new("John123", "Snow"));
            Console.WriteLine(result);
        }
    }

    private class Step04
    {
        const string constStrFirst = "FirstStr";
        const string summaryConstStr = $"SecondStr {constStrFirst}";
        public static void Run()
        {
            Console.WriteLine(summaryConstStr);
        }
    }

    private class Step05
    {
        record Car(string model, string color);
        public static void Run()
        {
            Car car = new("VAZ 2114", "Blue");

            {
                var (model, color) = car;
                // Инициализация
            }
            {
                string model = string.Empty;
                string color = string.Empty;
                (model, color) = car;
                // Присваивание
            }
            {
                string model = string.Empty;
                (model, var color) = car;
                // Инициализация и присваивание
            }
        }
    }
}