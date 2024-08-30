namespace GrpcServerConsole
{
    public class Common
    {
        public static ClientCollection clientCollection = new ClientCollection();
        public static SQLiteHelper dbHelper;
        public static ProgressUpdater progressUpdater;
        //public static string currentCommand = "";

        public static void ChangeCommand(string newCommand)
        {
            //if (currentCommand != newCommand)
            //{
                var collection = clientCollection.GetCollection();
                foreach (var item in collection)
                {
                    clientCollection.Update(item.Key, true, newCommand);
                }
            //}
            //currentCommand = newCommand;
        }

        public class ProgressUpdater
        {
            public int catCount { get; set; }
            public int elemCount { get; set; }

            public event Action<int, string> OnCatCounterUpdated;
            public event Action<int, string> OnElemCounterUpdated;

            //public int catCounter = 1;
            //public int elemCounter = 1;

            //public string _catName;
            //public string _elemName;

            public ProgressUpdater()
            {
            }

            public void UpdateCatCounter(int catCounter, string catName)
            {
                //catCounter = i;
                //_catName = catName;
                OnCatCounterUpdated?.Invoke(catCounter, catName);
            }

            public void UpdateElemCounter(int elemCounter, string elemName)
            {
                //elemCounter = ii;
                //_elemName = elemName;
                OnElemCounterUpdated?.Invoke(elemCounter, elemName);
            }

        }
    }
}
