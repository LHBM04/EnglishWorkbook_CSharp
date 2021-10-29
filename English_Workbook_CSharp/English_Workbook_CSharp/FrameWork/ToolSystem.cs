using System;

namespace ToolSystem
{
    public class ToolManager
    {
        public static void Assaign_Title(string Title)
        {
            Console.Title = Title;
        }

        public static void Alter_WindowSize(int col, int row)
        {
            Console.SetWindowSize(col, row);
        }

        public static void Press_Key(ConsoleKey key)
        {
            while (true)
            {
                ConsoleKeyInfo Cki = Console.ReadKey(true);
                if (Cki.Key == key)
                    break;
            }

            Console.Clear();
        }

        public static bool isPressKey(ConsoleKey key)
        {
            ConsoleKeyInfo Cki = Console.ReadKey(true);
            if (Cki.Key == key)
                return true;

            return false;
        }

        public static bool isPressKey(ConsoleKey key, ConsoleKey key2)
        {
            ConsoleKeyInfo Cki = Console.ReadKey(true);
            if (Cki.Key == key || Cki.Key == key2)
                return true;

            return false;
        }
    }
}
