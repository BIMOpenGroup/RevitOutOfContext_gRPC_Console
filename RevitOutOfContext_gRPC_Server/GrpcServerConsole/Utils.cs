namespace GrpcServerConsole
{
    class Utils
    {
        public static string ReadLineOrEsc()
        {
            string retString = "";

            int curIndex = 0;
            while (true)
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Esc
                if (readKeyResult.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return null;
                }

                // handle Enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return retString;
                }

                // handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        retString = retString.Remove(retString.Length - 1);
                        Console.Write(readKeyResult.KeyChar);
                        Console.Write(' ');
                        Console.Write(readKeyResult.KeyChar);
                        curIndex--;
                    }
                }
                else
                // handle all other keypresses
                {
                    retString += readKeyResult.KeyChar;
                    Console.Write(readKeyResult.KeyChar);
                    curIndex++;
                }
            }
        }
    }
}
