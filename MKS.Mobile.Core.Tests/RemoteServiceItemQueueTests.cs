using MKS.Mobile.Core.Interfaces;
using MKS.Mobile.Core.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MKS.Mobile.Core.Tests
{
    public class RemoteServiceItemQueueTests
    {
        [Fact]
        public async Task EnqueueItemGetsPushedIntoStorage()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            var mockReachability = new Mock<IReachability>();
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object);

            await queue.Enqueue("test");
            mockQueueStorage.Verify();
        }

        [Fact]
        public async Task EnqueueItemWithAutoStartEnabledAndTimerDisabledWithCarrierNetworkConnectivityStartsTimerAfterEnqueue()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            var mockReachability = new Mock<IReachability>();
            mockReachability.Setup(q => q.RemoteHostStatus()).Returns(NetworkStatus.ReachableViaCarrierDataNetwork);
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object)
            {
                AutoStartQueueProcessing = true
            };
            await queue.Enqueue("test");
            mockQueueTimer.VerifySet(t => t.IsEnabled = true, Times.Once);
        }

        [Fact]
        public async Task EnqueueItemWithAutoStartEnabledAndTimerDisabledWithWifiNetworkConnectivityStartsTimerAfterEnqueue()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            var mockReachability = new Mock<IReachability>();
            mockReachability.Setup(q => q.RemoteHostStatus()).Returns(NetworkStatus.ReachableViaCarrierDataNetwork);
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object)
            {
                AutoStartQueueProcessing = true
            };
            
            await queue.Enqueue("test");
            mockQueueTimer.VerifySet(t => t.IsEnabled = true, Times.Once);
        }

        [Fact]
        public async Task EnqueueItemWithAutoStartEnabledAndTimerDisabledWithoutNetworkConnectivityDoesNotStartTimerAfterEnqueue()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            
            var mockReachability = new Mock<IReachability>();
            mockReachability.Setup(q => q.RemoteHostStatus()).Returns(NetworkStatus.NotReachable);
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object)
            {
                AutoStartQueueProcessing = true
            };
            await queue.Enqueue("test");
            mockQueueTimer.VerifySet(t => t.IsEnabled = true, Times.Never);
        }

        [Fact]
        public async Task EnqueueItemWithAutoStartDisabledAndTimerDisabledDoesNotStartTimerAfterEnqueue()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            var mockReachability = new Mock<IReachability>();
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object)
            {
                AutoStartQueueProcessing = false
            };
            await queue.Enqueue("test");
            mockQueueTimer.VerifySet(t => t.IsEnabled = true, Times.Never);
        }

        [Fact]
        public async Task EnqueueItemWithAutoStartDisabledAndTimerEnabledDoesNotDisableTimerAfterEnqueue()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            mockQueueTimer.SetupProperty(t => t.IsEnabled);
            var mockReachability = new Mock<IReachability>();
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object)
            {
                AutoStartQueueProcessing = false
            };
            queue.StartQueueProcessing();
            await queue.Enqueue("test");
            Assert.True(queue.IsQueueProcessingEnabled);
        }

        [Fact]
        public async Task EnqueueItemCountIncremented()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.Add(It.Is<string>(v => v == "test"))).ReturnsAsync(new FakeObjectItem());
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            var mockQueueTimer = new Mock<ITimer>();
            var mockReachability = new Mock<IReachability>();
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, Enumerable.Empty<IQueueItemProcessor>(), mockQueueTimer.Object, mockReachability.Object);

            await queue.Enqueue("test");
            Assert.Equal(1, queue.Count);
        }

        [Fact]
        public async Task CompletedProcessItemDequeuesItemAndCallsRemoveFromStorage(){
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            mockQueueStorage.Setup(q => q.Add(It.IsAny<string>())).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.Remove(It.IsAny<IQueueItem>())).Verifiable();

            var mockProcessor = new Mock<IQueueItemProcessor>();
            mockProcessor.Setup(p => p.CanProcessItem(It.IsAny<string>())).Returns(true);
            mockProcessor.Setup(p => p.ProcessItem(It.IsAny<object>())).Returns(Task.FromResult(true));

            var mockQueueTimer = new Mock<ITimer>();
            Action<DateTime> timerAction = null;
            mockQueueTimer.SetupSet(t => t.Elapsed = It.IsAny<Action<DateTime>>())
                            .Callback<Action<DateTime>>(act => timerAction = act)
                            .Verifiable();
            mockQueueTimer.SetupGet(t => t.Elapsed)
                          .Returns(() => timerAction);

            var timer = mockQueueTimer.Object;
            
            var mockReachability = new Mock<IReachability>();
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, new []{ mockProcessor.Object}, timer, mockReachability.Object);
            
            await queue.Enqueue("test");

            mockQueueTimer.Verify();

            timer.Elapsed(DateTime.Now);
            Task.Delay(1000);
            mockQueueStorage.Verify();
        }

        [Fact(Timeout=2000)]
        public async Task CompletedProcessItemDequeuesItemAndCountIsDecremented()
        {
            var mockQueueStorage = new Mock<IItemQueueStorage>();
            mockQueueStorage.Setup(q => q.EntireQueue()).ReturnsAsync(new List<IQueueItem>());
            mockQueueStorage.Setup(q => q.Add(It.IsAny<string>())).ReturnsAsync(new FakeObjectItem()).Verifiable();
            mockQueueStorage.Setup(q => q.Remove(It.IsAny<IQueueItem>()))
                .Returns(Task.FromResult(true))
                .Verifiable();

            var mockProcessor = new Mock<IQueueItemProcessor>();
            mockProcessor.Setup(p => p.CanProcessItem(It.IsAny<string>())).Returns(true);
            mockProcessor.Setup(p => p.ProcessItem(It.IsAny<object>())).Returns(Task.FromResult(true));

            var mockQueueTimer = new Mock<ITimer>();
            Action<DateTime> timerAction = null;
            mockQueueTimer.SetupSet(t => t.Elapsed = It.IsAny<Action<DateTime>>())
                            .Callback<Action<DateTime>>(act => timerAction = act)
                            .Verifiable();
            mockQueueTimer.SetupGet(t => t.Elapsed)
                          .Returns(() => timerAction);

            var timer = mockQueueTimer.Object;

            var mockReachability = new Mock<IReachability>();
            mockReachability.Setup(q => q.InternetConnectionStatus()).Returns(NetworkStatus.NotReachable);
            var queue = new RemoteServiceItemQueue(mockQueueStorage.Object, new[] { mockProcessor.Object }, timer, mockReachability.Object);
            await queue.Enqueue("test");
            timer.Elapsed(DateTime.Now);
            await Task.Delay(1000);
            Assert.Equal(0, queue.Count);
        }

        internal class FakeObjectItem : IQueueItem
        {

            public Guid Id
            {
                get { return Guid.NewGuid(); }
            }

            public DateTime TimeStampUtc
            {
                get { return DateTime.UtcNow; }
            }

            public string SerialisedItem
            {
                get { return "test"; }
            }

            public string SerialisedItemType
            {
                get { return "test"; }
            }

            public object Item
            {
                get { return "test"; }
            }
        }
    }
}
