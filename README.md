# PostProcessor

This example .NET Core console application was developed using Visual Studio Code on a Mac.

It shows how one could carry out delayed processing of data in a multithreaded environment using Tasks, a ConcurrentDictionary and a ConcurrentBag.

The console application accepts numbers between 1 and 9, creating a new event for every number received. The events are stored in a ConcurrentDictionary, with a separate list of events per number. Delayed processing of all events for a specific number takes place 5 seconds later.  

Please note if you wish to run this using Visual Studio code, you need to change the "console" setting in launch.json to integratedTerminal or externalTerminal.
