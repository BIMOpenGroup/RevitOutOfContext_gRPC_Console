namespace GrpcServerConsole
{
    public class ClientCollection: IDisposable
    {
        private Dictionary<string, string> _collection;
        public ClientCollection()
        {
            _collection = new Dictionary<string, string>();
        }

        public void Add(string key, string value) {
            bool addet = _collection.TryAdd(key, value);
            if (!addet)
            {
                _collection[key] = value;
            }
        }

        //public void Get(string key, string value) { }

        public Dictionary<string, string> GetCollection() { 
            return _collection;
        }

        public void Dispose()
        {
            _collection.Clear();
        }
    }
}
