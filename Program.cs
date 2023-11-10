namespace DocumentStoreConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DocumentStore store = new();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Document Store Menu:");
                Console.WriteLine("1. Add Document");
                Console.WriteLine("2. Get Document");
                Console.WriteLine("3. Remove Document");
                Console.WriteLine("4. List All Documents");
                Console.WriteLine("5. Exit");
                Console.Write("Select an option: ");

                string option = Console.ReadLine() ?? string.Empty;

                switch (option)
                {
                    case "1":
                        AddDocumentToStore(store);
                        break;
                    case "2":
                        GetDocumentFromStore(store);
                        break;
                    case "3":
                        RemoveDocumentFromStore(store);
                        break;
                    case "4":
                        ListAllDocuments(store);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        private static void AddDocumentToStore(DocumentStore store)
        {
            Console.Write("Enter document title: ");
            string title = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter document content: ");
            string content = Console.ReadLine() ?? string.Empty;

            Document doc = new Document(Guid.NewGuid(), title, content);
            store.AddDocument(doc);
            Console.WriteLine("Document added successfully.\n");
        }

        private static void GetDocumentFromStore(DocumentStore store)
        {
            Console.Write("Enter document ID: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                // check cache before getting document.
                bool isCached = store.IsCached(id);
                try
                {
                    Document doc = store.GetDocument(id);
                    Console.WriteLine("Document retrieved successfully:");
                    Console.WriteLine(isCached ? "(Retrieved from cache)" : "(Loaded from main store)");
                    Console.WriteLine(doc);
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Document not found.\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid GUID format.\n");
            }
        }



        private static void RemoveDocumentFromStore(DocumentStore store)
        {
            Console.Write("Enter document ID: ");
            if (Guid.TryParse(Console.ReadLine(), out Guid id))
            {
                if (store.RemoveDocument(id))
                {
                    Console.WriteLine("Document removed successfully.\n");
                }
                else
                {
                    Console.WriteLine("Document not found.\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid GUID format.\n");
            }
        }

        private static void ListAllDocuments(DocumentStore store)
        {
            Console.WriteLine("Listing all documents:");
            foreach (var doc in store.GetAllDocuments())
            {
                Console.WriteLine(doc);
                Console.WriteLine();
            }
        }
    }
}
