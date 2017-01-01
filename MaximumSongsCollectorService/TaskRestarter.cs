using System;
using System.Threading;

namespace MaximumSongsCollectorService
{
    public class TaskRestarter
    {
        private readonly TimeSpan _delay;
        private volatile bool _running;
        private readonly Action _task;

        public TaskRestarter(Action task, TimeSpan? delay = null)
        {
            _task = task;
            _delay = delay ?? TimeSpan.FromMinutes(1);
        }

        public void Start()
        {
            _running = true;
            while (_running)
            {
                _task();
                Thread.Sleep(_delay);
            }
        }

        public void Stop()
        {
            _running = false;
        }

    }
}
