using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestDI.Services.Interfaces;

namespace TestDI.Services
{
    internal class TestParallel : ITestParallel
    {
        public async Task Test()
        {
            await Step01.Run();
        }

        private class Step01
        {
            static int Factorial(int x)
            {
                int result = 1;

                for (int i = 1; i <= x; i++)
                {
                    result *= i;
                }
                Console.WriteLine($"Факториал числа {x} равен {result}");
                return x;
            }

            static async Task Test1()
            {
                int[] numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, };
                var factorials = from n in numbers.AsParallel().WithDegreeOfParallelism(2)
                                 select Factorial(n);

                await Task.Delay(3000);

                var x = Environment.ProcessorCount;

                foreach (var n in factorials)
                {
                    Console.WriteLine(n);
                    // await Task.Delay(1000);
                }

                await Task.Delay(3000);

                foreach (var n in factorials)
                {
                    Console.WriteLine(n);
                    // await Task.Delay(1000);
                }
                Console.ReadLine();
            }
            public static async Task Run()
            {
                await Test1();
            }
        }
    }
}
