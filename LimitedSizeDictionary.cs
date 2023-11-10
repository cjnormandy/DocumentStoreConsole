namespace DocumentStoreConsoleApp
{
    public class LimitedSizeDictionary<K, V> : Dictionary<K, V> where K : notnull
    {
        private Queue<K> queue = new Queue<K>();
        private int limit;

        public LimitedSizeDictionary(int limit)
        {
            this.limit = limit;
        }

        public new void Add(K key, V value)
        {
            // key already exists update the value and move the key to the end of the queue.
            if (base.ContainsKey(key))
            {
                base[key] = value;
                // remove and re-enqueue the key to update its position in the queue.
                queue = new Queue<K>(queue.Where(k => !k.Equals(key)));
                queue.Enqueue(key);
            }
            else
            {
                // check if the limit has been reached.
                // if so, dequeue an item and remove it from the dictionary.
                if (base.Count >= limit)
                {
                    var oldKey = queue.Dequeue();
                    base.Remove(oldKey);
                }
                
                base.Add(key, value);
                queue.Enqueue(key);
            }
        }

        public bool IsInCache(K key)
        {
            return ContainsKey(key);
        }

    }
}
