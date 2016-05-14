using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nefedov.Nsudotnet.NumberGuesser
{
    class Program
    {
        private const int MaxNumber = 100;
        private const int MaxAttempts = 1000;
        private static string[] _swearing_formats = new[] {"What a stupid animal you are, {0}!", 
                                                           "{0}, do you know who you are? YOU ARE LOOOOOOOOOOOOOSER!!!",
                                                           "Go home, it's definetely not your day, {0}!"};

        private enum ResultType { Less, Bigger, Eqls }
        private struct Attempt
        {
            public int userNum;
            public ResultType result;

            public Attempt(int userNumParam, ResultType resultParam)
            {
                userNum = userNumParam;
                result = resultParam;
            }
        }

        static void Main(string[] args)
        {
            Attempt[] history = new Attempt[MaxAttempts];
            int historyAttemptNum = 0;
            Console.WriteLine("Welcome to the NumberGuesser game!");
            Console.Write("Enter your name, please: ");
            string name = Console.ReadLine();
            Console.WriteLine(String.Format("Hello, {0}!", name));
            Random random = new Random();


            Console.WriteLine(String.Format("Guess a number from 0 to {0}, enter 'q' for quit", MaxNumber));
            int num = random.Next(MaxNumber + 1);
            //Console.WriteLine("num = " + num);


            DateTime time1 = DateTime.Now;
            bool guessed = false;
            int attempts;
            for (attempts = 0; attempts < MaxAttempts; attempts++)
            {
                int userNum;
                string s;
                while (!Int32.TryParse(s = Console.ReadLine(), out userNum))
                {
                    if (s == "q")
                    {
                        Console.WriteLine("Quiting the program...");
                        return;
                    }
                    Console.WriteLine("ERROR!!! - you should enter an integer count");
                }
                if (userNum < num)
                {
                    Console.WriteLine("Too litte!");
                    history[historyAttemptNum] = new Attempt(userNum, ResultType.Less);
                    historyAttemptNum++;
                }
                else if (userNum > num)
                {
                    Console.WriteLine("Too big!");
                    history[historyAttemptNum] = new Attempt(userNum, ResultType.Bigger);
                    historyAttemptNum++;
                }
                else
                {
                    guessed = true;
                    Console.WriteLine("Yes, you're right!!!");
                    history[historyAttemptNum] = new Attempt(userNum, ResultType.Eqls);
                    historyAttemptNum++;
                    attempts++;
                    break;
                }
                if (attempts % 4 == 3)//equivalent to if ((attempts+1) % 4 == 0)
                {
                    Console.WriteLine(String.Format(_swearing_formats[random.Next(_swearing_formats.Length)], name));
                }
            }
            DateTime time2 = DateTime.Now;
            TimeSpan time = time2 - time1;


            if (!guessed)
            {
                Console.WriteLine(String.Format("Your attempts are over! True number is {0}", num));
            }
            Console.WriteLine(String.Format("You used {0} attempts", attempts));
            for (int i = 0; i < historyAttemptNum; i++)
            {
                Console.WriteLine(String.Format("{0}. {1} {2}", i + 1, history[i].userNum, history[i].result));
            }
            //Console.WriteLine(time.TotalSeconds);
            Console.WriteLine(String.Format("Time taken: {0} minutes", Math.Floor(time.TotalMinutes)));
        }
    }
}
