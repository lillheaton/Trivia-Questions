using System;
using System.IO;
using System.Linq;

using TriviaQuestion2.Models;

namespace TriviaQuestion2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var stream = new StreamReader("questions.txt"))
            using (var db = new Trivia2Context())
            {
                int temp = 1;
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    if (line.Split('*').Count() > 1)
                    {
                        Console.WriteLine("Work on {0}", temp);
                        db.Questions.Add(new Question { Text = line.Split('*')[0], Answer = line.Split('*')[1] });
                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Skipped {0}", temp);
                    }
                    
                    temp++;
                }
            }

            Console.ReadLine();
        }
    }
}