using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sn.KeySound
{
    internal static class ConsoleUtils
    {
        public static int Choose(string prompt, params string[] options)
        {
            (int x, int y) cursorStartPos = (Console.CursorLeft, Console.CursorTop);

            int choice = 0;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                Console.SetCursorPosition(cursorStartPos.x, cursorStartPos.y);

                sb.Clear();
                sb.AppendLine($"$ {prompt}");
                sb.AppendLine();

                for (int i = 0; i < options.Length; i++)
                {
                    string marker = choice == i ? "->" : "  ";
                    sb.AppendLine($"  {marker}  {i}. {options[i]}");
                    sb.AppendLine();
                }

                sb.AppendLine("$ Use Up/Down arrow to move, Enter to choose.");

                Console.WriteLine(sb.ToString());
                var key = Console.ReadKey(true).Key;

                choice = key switch
                {
                    ConsoleKey.UpArrow => Math.Max(choice - 1, 0),
                    ConsoleKey.DownArrow => Math.Min(choice + 1, options.Length - 1),
                    _ => choice
                };

                if (key == ConsoleKey.Enter)
                    return choice;
            }
        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("$ PressAnyKeyToContinue");
            Console.ReadKey(true);
        }
    }
}
