using System.Collections.Concurrent;

namespace GrpcServerConsole
{
    public class ClientCollection : IDisposable
    {
        private ConcurrentDictionary<string, ClientCondition> _collection;
        public delegate void OnCommandSendHandler(string revitUnicId);
        public event OnCommandSendHandler CommandSend;
        public class ClientCondition
        {
            public string? response { get; set; }
            public bool commandCondition { get; set; }

            public ClientCondition()
            {

            }
            public ClientCondition(string res, bool comm)
            {
                response = res;
                commandCondition = comm;
            }
        }
        public ClientCollection()
        {
            _collection = new ConcurrentDictionary<string, ClientCondition>();
        }

        public void Add(string key, string value)
        {
            bool addet = _collection.TryAdd(key, new ClientCondition(value, false));
            if (!addet)
            {
                _collection[key] = new ClientCondition(value, _collection[key].commandCondition);
            }
        }

        public ClientCondition Get(string key)
        {
            var cond = new ClientCondition();
            bool hasValue = _collection.TryGetValue(key, out cond);
            return cond;
        }

        public void Update(string key, bool commandCondition)
        {
            if (_collection.ContainsKey(key))
            {
                if (commandCondition)
                {
                    CommandSend.Invoke($"command send to revit: {key}");
                }
                else
                {
                    CommandSend.Invoke($"command added to revit: {key}");
                }
                var cond = Get(key);
                cond.commandCondition = commandCondition;
                _collection[key] = cond;
            }
        }

        public ConcurrentDictionary<string, ClientCondition> GetCollection()
        {
            return _collection;
        }

        public void Dispose()
        {
            _collection.Clear();
        }
    }
}
