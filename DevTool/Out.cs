namespace DevTool
{
    using System;

    public static class Out
    {
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public static void WriteLine(int indent, string line)
        {
            Console.WriteLine(new string(' ', indent) + line);
        }

        public static void WriteLine(int indent, string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(new string(' ', indent) + line);
            Console.ResetColor();
        }
    }
}
