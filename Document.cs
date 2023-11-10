namespace DocumentStoreConsoleApp
{
    public class Document
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public Document(Guid id, string title, string content)
        {
            ID = id;
            Title = title;
            Content = content;
        }

        public override string ToString()
        {
            return $"Document ID: {ID}\nTitle: {Title}\nContent: {Content}";
        }
    }
}
