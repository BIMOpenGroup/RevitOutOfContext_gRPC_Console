using RSNiniManager;
using System.Collections.Concurrent;

namespace GrpcServerConsole
{
    public class ClientCollection : IDisposable
    {
        private ConcurrentDictionary<string, ClientCondition> _collection;
        public delegate void OnCommandSendHandler(string message);
        public event OnCommandSendHandler CommandSend;
        public class ClientCondition
        {
            public string? response { get; set; }
            public bool commandCondition { get; set; }
            public string command { get; set; }

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

        public void TryAdd(string key, string res)
        {
            bool addet = _collection.TryAdd(key, new ClientCondition(res, false));
            if (!addet)
            {
                _collection[key].response = res;
            }
        }

        public ClientCondition Get(string key)
        {
            //var cond = new ClientCondition();
            //bool hasValue = _collection.TryGetValue(key, out cond);
            return _collection[key];
        }

        public void Update(string key, bool commandCondition, string command = null)
        {
            if (_collection.ContainsKey(key))
            {
                if (commandCondition)
                {
                    CommandSend.Invoke($"command [bold {Colors.selectionColor}]{command}[/] [{Colors.infoColor}]added[/] to revit: {key}");
                }
                else
                {
                    CommandSend.Invoke($"command [bold {Colors.selectionColor}]{command}[/] [{Colors.infoColor}]send[/] to revit: {key}");

                }
                var cond = Get(key);
                cond.commandCondition = commandCondition;
                if (command != null)
                {
                    cond.command = command;
                }
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
