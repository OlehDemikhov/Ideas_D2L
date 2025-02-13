using System;

namespace TestPackageMinimizer
{
    internal static class ConsoleHelper
    {
        public static void WriteLineWithColor( string text, ConsoleColor color )
        {
            Console.BackgroundColor = color;
            Console.WriteLine( text );
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
