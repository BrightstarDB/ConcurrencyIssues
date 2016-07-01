using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace BrightstarDB.ConcurrencyIssue
{
    class Program
    {
        public static bool Cancelled = false;
        static void Main(string[] args)
        {
            Startup.CreateStore();

            BrightstarDB.Logging.EnableConsoleOutput(true);
            BrightstarDB.Logging.EnableProfiling(true);

            //return;
            AsyncContext.Run(() => RunTasks());
        }

        static async void RunTasks()
        {
            var task1 = Task.Run(() => Counter1Increment());
            var task2 = Task.Run(() => Counter2Increment());
            
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
            Cancelled = true;
            await task1;
            await task2;
        }

        static async void Counter1Increment()
        {
            while (!Cancelled)
            {
                await Task.Delay(2000);

                using (var ctx = new Context1(Startup.ConnectionString))
                {
                    var countup = ctx.ComplexEntity1s.First();
                    countup.Counter += 1;

                    ctx.SaveChanges();
                }
            }
        }

        static async void Counter2Increment()
        {
            while (!Cancelled)
            {
                await Task.Delay(500);
                using (var ctx = new Context1(Startup.ConnectionString))
                {
                    var foo = ctx.ComplexEntity2s.First(e => e.Name.Equals("default"));

                    if (foo != null)
                    {
                        var bar = ctx.ComplexEntity1s.First();
                        bar.Counter += 1;
                        Console.WriteLine("Counter:" + bar.Counter);
                        ctx.SaveChanges();
                    }
                }
            }
        }
    }
}
