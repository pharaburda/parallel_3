using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_3
{
    class Musician
    {
        public int x;
        public int y;
        public bool canPlay;
        public List<Musician> musicians;
        public Sender sender;
        public Receiver receiver;
        int round = 0;
        public Musician(int _x, int _y, Sender _sender, Receiver _receiver)
        {
            x = _x;
            y = _y;
            sender = _sender;
            receiver = _receiver;
        }

        public void MusicianWork()
        {
            bool satisfied = false;
            while (!satisfied)
            {
                round++;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Musician {0}{1} in round {2}", x, y, round);
                Console.BackgroundColor = ConsoleColor.Black;
                canPlay = true;
                Random rnd = new Random();
                int random = rnd.Next(1000000);
                Console.WriteLine("Musicians {0}{1} random {2}", x, y, random);
                List<Musician> neighbours = new List<Musician>();

                foreach (Musician musician in musicians)
                {
                    if (Math.Abs(musician.x - x) < 3 && Math.Abs(musician.y - y) < 3)
                    {
                        neighbours.Add(musician);
                    }
                }

                foreach (Musician neigbour in neighbours)
                {
                    sender.Send(String.Format("1_{0}",random.ToString()), String.Format("{0}{1}", neigbour.x, neigbour.y));
                }

                int received_random = Int32.Parse(receiver.Receive(String.Format("{0}{1}", x, y)));
                Console.WriteLine("Musician {0}{1} received message {2}", x, y, received_random);
                if (received_random > random) canPlay = false;
                if (canPlay)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Musician {0}{1} is playing", x, y);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Thread.Sleep(2000);
                    satisfied = true;
                    musicians.Remove(this);
                }
                //todo different queues for different purposes
            }
        }

        //public void ChoosePlayers()
        //{
        //    isPlaying = true;
        //    Random rnd = new Random();
        //    int random = rnd.Next(1000000);
        //    List<Musician> neighbours = new List<Musician>();
        //    Console.WriteLine("Musicians {0}{1} random {2}", x, y, random);

        //    foreach (Musician musician in musicians)
        //    {
        //        if(Math.Abs(musician.x - x) < 3 && Math.Abs(musician.y - y) < 3)
        //        {
        //            neighbours.Add(musician);
        //        }
        //    }

        //    foreach (Musician neigbour in neighbours)
        //    {
        //        sender.Send(random.ToString(), String.Format("{0}{1}", neigbour.x, neigbour.y));
        //        Console.WriteLine("Musician {0}{1} sent message {2} to neigbour {3}{4}",x, y, random, neigbour.x, neigbour.y);
        //    }

        //    int received_random = receiver.Receive(String.Format("{0}{1}", x, y));
        //    Console.WriteLine("Musician {0}{1} received message {2}", x, y, received_random);
        //    if (received_random > random) isPlaying = false;

        //}
    }
}
