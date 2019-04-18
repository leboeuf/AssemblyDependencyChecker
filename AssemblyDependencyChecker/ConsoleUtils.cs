using System;

namespace AssemblyDependencyChecker
{
    public static class ConsoleUtils
    {
        /// <summary>
        /// Gets or sets the number of spaces per indentation level.
        /// </summary>
        public static int IndentationFactor = 3;

        public static void WriteLine(int indentation, string value, ConsoleColor color = ConsoleColor.White)
        {
            WriteLine($"{GetIndentationString(indentation)}{value}", color);
        }

        public static void WriteLine(string value, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        private static string GetIndentationString(int depth)
        {
            return new string(' ', depth * IndentationFactor);
        }
    }
}
