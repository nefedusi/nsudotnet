using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nefedov.Nsudotnet.NumberGuesser
{
    class Program
    {
        private const int MAX_NUMBER = 100;
        private const int MAX_ATTEMPTS = 1000;
        private static string[] _swearing_formats = new [] {"What a stupid animal you are, {0}!", 
                                                           "{0}, do you know who you are? YOU ARE LOOOOOOOOOOOOOSER!!!",
                                                           "Go home, it's definetely not your day, {0}!"};


        enum result_type { LESS, BIGGER, EQUALS }//NECESSARY ;?
        private struct Attempt
        {
            public int user_num;
            public result_type result;

            public Attempt(int user_num_param, result_type result_param)
            {
                user_num = user_num_param;
                result = result_param;
            }
        }


        static void Main(string[] args)
        {
            Attempt[] history = new Attempt[MAX_ATTEMPTS];
            int history_attempt_num = 0;
            Console.WriteLine("Welcome to the NumberGuesser game!");
            Console.Write("Enter your name, please: ");
            string name = Console.ReadLine();
            Console.WriteLine(String.Format("Hello, {0}!", name));
            Random random = new Random();


            Console.WriteLine(String.Format("Guess a number from 0 to {0}, enter 'q' for quit", MAX_NUMBER));
            int num = random.Next(MAX_NUMBER + 1);
            //Console.WriteLine("num = " + num);


            DateTime time1 = DateTime.Now;
            bool guessed = false;
            int attempts;
            for (attempts = 0; attempts < MAX_ATTEMPTS; attempts++)
            {
                int user_num;
                string s;
                while (!Int32.TryParse(s = Console.ReadLine(), out user_num)) 
                {
                    if (s == "q")
                    {
                        Console.WriteLine("Quiting the program...");
                        return;
                    }
                    Console.WriteLine("ERROR!!! - you should enter an integer count");
                }
                if (user_num < num)
                {
                    Console.WriteLine("Too litte!");
                    history[history_attempt_num] = new Attempt(user_num, result_type.LESS);
                    history_attempt_num++;
                } else if (user_num > num)
                {
                    Console.WriteLine("Too big!");
                    history[history_attempt_num] = new Attempt(user_num, result_type.BIGGER);
                    history_attempt_num++;
                } else
                {
                    guessed = true;
                    Console.WriteLine("Yes, you're right!!!");
                    history[history_attempt_num] = new Attempt(user_num, result_type.EQUALS);
                    history_attempt_num++;
                    attempts++;
                    break;
                }
                if (attempts%4 == 3)//equivalent to if ((attempts+1) % 4 == 0)
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
            for (int i = 0; i < history_attempt_num; i++)
            {
                Console.WriteLine(String.Format("{0}. {1} {2}", i + 1, history[i].user_num, history[i].result));
            }
            Console.WriteLine(String.Format("Time taken: {0} minutes", time.Minutes));


        }
    }
}