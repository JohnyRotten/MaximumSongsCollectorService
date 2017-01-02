using MaximumSongsCollectorService.Collectors;
using NUnit.Framework;
using SongsCollectorLibrary.Utils;
using System;
using System.Threading;

namespace MaximumSongsCollectorService
{
    [TestFixture]
    class WorkerTest
    {

        private Worker _worker;

        [SetUp]
        public void TestInit()
        {
            _worker = new Worker();
            _worker.AddCollectors(new MaximumCollector());
        }

        [Test]
        public void GettingArtistsTest()
        {
            _worker.UpdateSongs();
            var count = _worker.Artists.Count;
            Assert.AreNotEqual(0, count);
            Thread.Sleep(TimeSpan.FromMinutes(5));
            _worker.UpdateSongs();
            Assert.AreNotEqual(count, _worker.Artists.Count);
            _worker.Save();
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
