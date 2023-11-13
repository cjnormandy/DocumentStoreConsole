namespace DocumentStoreConsoleApp.Tests;

public class DocumentStoreTests
{
    [Fact]
    public void AddDocument_ShouldAddDocument()
    {
        // Arrange
        var store = new DocumentStore();
        var doc = new Document(Guid.NewGuid(), "Title", "Content");

        // Act
        store.AddDocument(doc);

        // Assert
        Assert.Equal(doc, store.GetDocument(doc.ID));
    }

    [Fact]
    public void GetDocument_ShouldReturnDocumentFromCache()
    {
        // Arrange
        var store = new DocumentStore();
        var doc = new Document(Guid.NewGuid(), "Title", "Content");
        store.AddDocument(doc);

        // First retrieval, to ensure the document is cached
        store.GetDocument(doc.ID);

        // Act
        var cachedDoc = store.GetDocument(doc.ID); // should be from cache

        // Assert
        Assert.Equal(doc, cachedDoc);
    }

    [Fact]
    public void AddDocument_ShouldEvictOldDocumentsWhenLimitIsReached()
    {
        // Arrange
        var store = new DocumentStore();
        var limit = 10;
        var docs = new List<Document>();

        // Act
        for (int i = 0; i < limit + 5; i++)
        {
            var doc = new Document(Guid.NewGuid(), $"Title{i}", $"Content{i}");
            docs.Add(doc);
            store.AddDocument(doc);
        }

        // Assert
        for (int i = 0; i < 5; i++)
        {
            // The first five documents should have been kicked from the cache
            Assert.False(store.IsInCache(docs[i].ID));
        }

        for (int i = 5; i < limit + 5; i++)
        {
            // The last ten documents should still be in the cache
            Assert.True(store.IsInCache(docs[i].ID));
        }
    }

}
