using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostProcessor
{
    public class PostProcessor
    {
        private readonly ConcurrentDictionary<int, List<Event>> _entries;

        public PostProcessor()
        {
            _entries = new ConcurrentDictionary<int, List<Event>>();
        }

        public void Process()
        {
            bool processing = true;

            Console.WriteLine("Input numbers between 1 and 9...");
            
            while (processing)
            {
                var keyInfo = Console.ReadKey();
                Console.WriteLine();

                if (keyInfo.Key == ConsoleKey.Q)
                {
                    processing = false;
                }
                else
                {
                    int value = int.Parse(keyInfo.KeyChar.ToString());

                    _entries.AddOrUpdate(value,
                    newId =>
                    {
                        var list = new List<Event>() { new Event {Timestamp = DateTime.UtcNow}};

                        Task.Run(async delegate
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
                            ProcessEvents(value);
                        });

                        return list;
                    },
                    (existingId, existingIdList) =>
                    {
                        existingIdList.Add(new Event {Timestamp = DateTime.UtcNow});
                        return existingIdList;
                    });
                }
            }
        }

        private void ProcessEvents(int id)
        {
            List<Event> events;
            if (_entries.TryRemove(id, out events))
            {
                foreach (var e in events)
                {
                    Console.WriteLine($"Post Processing {id}:{e.Timestamp.ToString()}");
                }
            }
        }
    }
}