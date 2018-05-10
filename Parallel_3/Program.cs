using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_3
{
    class Program
    {
        static void Main(string[] args)
        {
            //todo reading from file
            Sender sender = new Sender();
            Receiver receiver = new Receiver();
            List<Musician> musicians = new List<Musician>();
            musicians.Add(new Musician(0, 1, sender, receiver));
            musicians.Add(new Musician(2, 1, sender, receiver));
            musicians.Add(new Musician(3, 4, sender, receiver));
            musicians.Add(new Musician(2, 2, sender, receiver));
            musicians.Add(new Musician(5, 1, sender, receiver));
            musicians.Add(new Musician(4, 3, sender, receiver));

            foreach (Musician m in musicians)
            {
                m.musicians = musicians;
            }

           
            List<Thread> tList = new List<Thread>();
            foreach(Musician m in musicians)
            {
                Thread t = new Thread(new ThreadStart(m.MusicianWork));
                t.Start();
            }
            
        }
    }
}
