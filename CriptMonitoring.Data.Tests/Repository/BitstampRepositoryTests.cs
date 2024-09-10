using CriptMonitoring.Data.Repository;
using CriptMonitoring.Domain.Entity;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace CriptMonitoring.Data.Tests.Repository;

public class BitstampRepositoryTests
{
    [Fact]
    public void GetLast_ValidCripto_ReturnsLatestOrderBookEntity()
    {
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockCollection = new Mock<IMongoCollection<OrderBookEntity>>();
        var mockCursor = new Mock<IAsyncCursor<OrderBookEntity>>();
        var orderBookEntity = new OrderBookEntity { cripto = "BTC", Timestamp = new BsonTimestamp(DateTime.UtcNow.Millisecond) };

        mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
        mockDatabase.Setup(d => d.GetCollection<OrderBookEntity>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
        mockCursor.Setup(c => c.Current).Returns(new List<OrderBookEntity> { orderBookEntity });
        mockCollection.Setup(c => c.FindSync(It.IsAny<FilterDefinition<OrderBookEntity>>(),
                It.IsAny<FindOptions<OrderBookEntity, OrderBookEntity>>(), It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        var repository = new BitstampRepository(mockClient.Object, "testDatabase", "testCollection");
        var result = repository.GetLast("BTC");

        Assert.Equal(orderBookEntity, result);
    }

    [Fact]
    public void GetLast_InvalidCripto_ReturnsNull()
    {
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockCollection = new Mock<IMongoCollection<OrderBookEntity>>();
        var mockCursor = new Mock<IAsyncCursor<OrderBookEntity>>();

        mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
        mockDatabase.Setup(d => d.GetCollection<OrderBookEntity>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
        mockCursor.Setup(c => c.Current).Returns(new List<OrderBookEntity>());
        mockCollection.Setup(c => c.FindSync(It.IsAny<FilterDefinition<OrderBookEntity>>(),
                It.IsAny<FindOptions<OrderBookEntity, OrderBookEntity>>(), It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        var repository = new BitstampRepository(mockClient.Object, "testDatabase", "testCollection");
        var result = repository.GetLast("INVALID");

        Assert.Null(result);
    }

    [Fact]
    public void GetByTime_ValidCriptoAndTime_ReturnsOrderBookEntities()
    {
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockCollection = new Mock<IMongoCollection<OrderBookEntity>>();
        var mockCursor = new Mock<IAsyncCursor<OrderBookEntity>>();
        var orderBookEntities = new List<OrderBookEntity>
        {
            new OrderBookEntity { cripto = "BTC", Timestamp = new BsonTimestamp(DateTime.UtcNow.Millisecond) }
        };

        mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
        mockDatabase.Setup(d => d.GetCollection<OrderBookEntity>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
        mockCursor.Setup(c => c.Current).Returns(orderBookEntities);
        mockCollection.Setup(c => c.FindSync(It.IsAny<FilterDefinition<OrderBookEntity>>(),
                It.IsAny<FindOptions<OrderBookEntity, OrderBookEntity>>(), It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        var repository = new BitstampRepository(mockClient.Object, "testDatabase", "testCollection");
        var result = repository.GetByTime("BTC", 1627846261);

        Assert.Equal(orderBookEntities, result);
    }

    [Fact]
    public void GetByTime_InvalidCripto_ReturnsEmptyList()
    {
        var mockClient = new Mock<IMongoClient>();
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockCollection = new Mock<IMongoCollection<OrderBookEntity>>();
        var mockCursor = new Mock<IAsyncCursor<OrderBookEntity>>();

        mockClient.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(mockDatabase.Object);
        mockDatabase.Setup(d => d.GetCollection<OrderBookEntity>(It.IsAny<string>(), null))
            .Returns(mockCollection.Object);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
        mockCursor.Setup(c => c.Current).Returns(new List<OrderBookEntity>());
        mockCollection.Setup(c => c.FindSync(It.IsAny<FilterDefinition<OrderBookEntity>>(),
                It.IsAny<FindOptions<OrderBookEntity, OrderBookEntity>>(), It.IsAny<CancellationToken>()))
            .Returns(mockCursor.Object);

        var repository = new BitstampRepository(mockClient.Object, "testDatabase", "testCollection");
        var result = repository.GetByTime("INVALID", 1627846261);

        Assert.Empty(result);
    }
}