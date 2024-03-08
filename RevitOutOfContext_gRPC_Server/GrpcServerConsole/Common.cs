namespace GrpcServerConsole
{
    public class Common
    {
        public static ClientCollection clientCollection = new ClientCollection();
        public static string currentCommand = "";

        public static void ChangeCommand(string newCommand)
        {
            if (currentCommand != newCommand)
            {
                var collection = clientCollection.GetCollection();
                foreach (var item in collection)
                {
                    clientCollection.Update(item.Key, false);
                }
            }
            currentCommand = newCommand;
        }
    }
}
