using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PostProcessor
{
    public class PostProcessor
    {
        private readonly ConcurrentDictionary<uint, List<uint>> _entries;

        public PostProcessor()
        {
            _entries = new ConcurrentDictionary<uint, List<uint>>();
        }

        public void Process()
        {
            bool processing = true;

            Console.WriteLine("Press any key to continue...");
            
            while (processing)
            {
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.Q)
                {
                    processing = false;
                }
                else
                {
                    // _entries.AddOrUpdate(1,
                    // newId =>
                    // {

                    // },
                    // (existingId, existingIdList) =>
                    // {

                    // });
                }
            }
        }

    }
}