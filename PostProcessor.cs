using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostProcessor
{
    public class PostProcessor
    {
        // Store a list of events for a specific number in a ConcurrentDictionary
        private readonly ConcurrentDictionary<int, List<Event>> _entries;
        
        // Use a ConcurrentBag to keep track of all incomplete Tasks.
        private readonly ConcurrentBag<Task> _processorTasks;

        public PostProcessor()
        {
            _entries = new ConcurrentDictionary<int, List<Event>>();
            _processorTasks = new ConcurrentBag<Task>();
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
                    
                    // Wait for all tasks in the ConcurrentBag to complete
                    Task.WaitAll(_processorTasks.ToArray(), TimeSpan.FromSeconds(10));

                }
                else
                {
                    // Get the number entered at the console
                    int value = int.Parse(keyInfo.KeyChar.ToString());

                    // Add an event to the matching list in the dictionary for every number entered
                    _entries.AddOrUpdate(value,
                    newId =>
                    {
                        // If the number does not exist as a key in the dictionary, create a list for it and add an event.
                        var list = new List<Event>() { new Event {Timestamp = DateTime.UtcNow}};

                        // Run a task that will call ProcessEvents in 5 seconds time
                        Task task = Task.Run(async delegate
                        {
                            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
                            ProcessEvents(value);
                        }).ContinueWith(cw =>
                        {
                            // When ProcessEvents has completed, remove the task from the ConcurrentBag. 
                            _processorTasks.TryTake(out task);
                        });

                        // Add the task to the ConcurrentBag.
                        _processorTasks.Add(task);

                        return list;
                    },
                    (existingId, existingIdList) =>
                    {
                        // If the number exists as a key in the dictionary, just add a new Event.
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
                    Console.WriteLine($"Post Processing {id}:{e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff")}");

                    // Do post processing here...
                }
            }
        }
    }
}
