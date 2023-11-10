using System.Text.Json;
using System.IO;

namespace DocumentStoreConsoleApp
{
    public class DocumentStore
    {
        private Dictionary<Guid, Document> documents = new Dictionary<Guid, Document>();
        private LimitedSizeDictionary<Guid, Document> cache = new LimitedSizeDictionary<Guid, Document>(10);

        public void AddDocument(Document doc)
        {
            documents[doc.ID] = doc;
            cache.Add(doc.ID, doc);
        }

        public Document GetDocument(Guid id)
        {
            if (cache.TryGetValue(id, out Document? cachedDoc))
            {
                return cachedDoc!;
            }

            if (documents.TryGetValue(id, out Document? doc))
            {
                cache.Add(id, doc);
                return doc!;
            }

            throw new KeyNotFoundException("Document not found.");
        }

        public bool RemoveDocument(Guid id)
        {
            bool removedFromDocuments = documents.Remove(id);
            bool removedFromCache = cache.Remove(id);
            return removedFromDocuments || removedFromCache;
        }

        public IEnumerable<Document> GetAllDocuments()
        {
            return documents.Values;
        }

        public bool IsCached(Guid id)
        {
            return cache.ContainsKey(id);
        }

        // Mark: Data persistance
        public void Save(string filename)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(documents, options);
            File.WriteAllText(filename, json);
        }

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                string json = File.ReadAllText(filename);
                var result = JsonSerializer.Deserialize<Dictionary<Guid, Document>>(json);

                if (result != null)
                {
                    documents = result;
                }
                else
                {
                    documents = new Dictionary<Guid, Document>();
                }
            }
            else
            {
                documents = new Dictionary<Guid, Document>();
            }
        }

        public bool IsInCache(Guid id)
        {
            return cache.ContainsKey(id);
        }
    }
}
