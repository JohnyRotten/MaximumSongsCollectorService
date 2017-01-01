using NUnit.Framework;
using System;
using System.Threading;

namespace MaximumSongsCollectorService
{
    [TestFixture]
    class WorkerTest
    {
        [Test]
        public void GettingArtistsTest()
        {
            var worker = new Worker();
            worker.UpdateSongs();
            var count = worker.Artists.Count;
            Assert.AreNotEqual(0, count);
            Thread.Sleep(TimeSpan.FromMinutes(5));
            worker.UpdateSongs();
            Assert.AreNotEqual(count, worker.Artists.Count);
            worker.Save();
        }

        [Test]
        public void RestarterTest()
        {
            var i = 0;
            var restarter = new TaskRestarter(() => i++, TimeSpan.FromMilliseconds(1));
            var thread = new Thread(restarter.Start);
            thread.Start();
            Thread.Sleep(TimeSpan.FromMilliseconds(30));
            thread.Abort();
            Assert.AreNotEqual(0, i);
        }

        [Test]
        public void SavingTest()
        {
            var count = Worker.Instance.Artists.Count;
            Thread.Sleep(TimeSpan.FromMinutes(5));
            Worker.Instance.SaveUpdatesSongs();
            Assert.AreNotEqual(count, Worker.Instance.Artists);
        }

    }
}
