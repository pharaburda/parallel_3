﻿using System;
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

        public List<Musician> musicians;
        List<Musician> neighbours;

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

        public List<Musician> findNeigbours(Musician m)
        {
            List<Musician> neighbours = new List<Musician>();
            foreach (Musician musician in musicians)
            {
                if (Math.Abs(musician.x - x) < 3 && Math.Abs(musician.y - y) < 3 && !( musician.x == x && musician.y == y))
                {
                    neighbours.Add(musician);
                }
            }
            return neighbours;
        }

        public void musicianPlay()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Musician {0}{1} is playing", x, y);
            Console.BackgroundColor = ConsoleColor.Black;
            Thread.Sleep(2000);
            foreach(Musician neigbour in neighbours)
            {
                sender.Send(String.Format("{0}{1}", x, y), String.Format("3_{0}{1}", neigbour.x, neigbour.y));
            }
            musicians.Remove(this);
        }

        public void MusicianWork()
        {
            neighbours = findNeigbours(this);
            while (true)
            {
                round++;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Musician {0}{1} in round {2}", x, y, round);
                Console.BackgroundColor = ConsoleColor.Black;
                int seed = 10 * x + y;
                Random rnd = new Random(seed);
                int random = rnd.Next(1000000);
                Console.WriteLine("Musicians {0}{1} random {2}", x, y, random);

                foreach (Musician neigbour in neighbours)
                {
                    sender.Send(String.Format("{0}",random.ToString()), String.Format("1_{0}{1}", neigbour.x, neigbour.y));
                }

                int received_random = Int32.Parse(receiver.Receive(String.Format("1_{0}{1}", x, y)));
                Console.WriteLine("Musician {0}{1} received message {2}", x, y, received_random);
                if (received_random < random)
                {
                    musicianPlay();
                    break;
                }
                
                foreach (Musician neigbour in neighbours)
                {
                    sender.Send(String.Format("1"), String.Format("2_{0}{1}", neigbour.x, neigbour.y));
                    Console.WriteLine("Musician {0}{1} send message to 2_{2}{3}",x, y, neigbour.x, neigbour.y);
                }

                int loser_neighbour_count = Int32.Parse(receiver.Receive(String.Format("2_{0}{1}", x, y)));
                Console.WriteLine("Musician {0}{1} received message {2}", x, y, loser_neighbour_count);
                if (loser_neighbour_count == neighbours.Count())
                {
                    musicianPlay();
                    break;
                }
                
                string neigboursToRemove = receiver.Receive(String.Format("3_{0}{1}", x, y));
                for(int i=0; i < neigboursToRemove.Length / 2; i+=2)
                {
                    int _x = neigboursToRemove[i];
                    int _y = neigboursToRemove[i+1];
                    neighbours.Remove(new Musician(_x, _y, sender, receiver));
                }

                if (neighbours.Count == 0)
                {
                    musicianPlay();
                    break;
                }
                //todo edge case when both neigbours are losers
            }
        }

    }
}
