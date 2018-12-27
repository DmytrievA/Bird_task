using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2_TRSPO
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.Write("Input bird number: ");
            int N = Convert.ToInt32(Console.ReadLine());
            var fam = new BirdFamily(N);
        }
       
    }


    public class BirdFamily
    {
        static int food = 0;
        object locker = new object();
        Thread[] birds;

        public BirdFamily(int count)
        {
            birds = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                birds[i] = new Thread(Eat);
                birds[i].Name = $"Bird{i}";
            }

            Thread mom = new Thread(MakeFood);
            mom.Start();

            foreach (Thread b in birds)
                b.Start(mom);
        }

        public void Eat(object mom)
        {
            Thread mother = (Thread)mom;
            while (true)
            {
                if (food <= 0)
                {
                    lock (locker)
                    {
                        if (food <= 0)
                        {
                            lock (locker)
                            {
                                Console.WriteLine($"{Thread.CurrentThread.Name} is calling mom");
                                mother = new Thread(new ThreadStart(MakeFood));
                                mother.Start();
                                mother.Join();
                            }
                        }
                    }
                }
                lock (locker)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} is eating");
                    food -= Math.Abs((new Random()).Next() % 15) + 1;
                    Console.WriteLine($"Food: {food}");
                }
                Thread.Sleep(500);
                
            }
        }

        void MakeFood()
        {
            Console.WriteLine("Мать полетела за едой");
            Thread.Sleep(1000);
            Console.WriteLine("Мать вернулась с едой");
            food += Math.Abs((new Random()).Next() % 100) + 100;
        }
    }
}
