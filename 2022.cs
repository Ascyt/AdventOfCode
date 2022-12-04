﻿using System.Windows.Forms;

class Program
{
    const string INPUT_FILE = @"C:\Users\filip\Downloads\input";
    const bool ASK_FOR_INPUT = false;
    static Method ToRun = d4v2;


    delegate int Method(string[] input);

    private static int lastStep;

    [STAThread]
    public static void Main()
    {
        int? output = null;

        try
        {
            List<string> args = new List<string>();

            if (ASK_FOR_INPUT)
            {
                Console.WriteLine("Paste in each lines, type in \">>\" to start code: ");

                string arg;

                do
                {
                    arg = Console.ReadLine();
                    args.Add(arg);
                }
                while (arg.ToLower() != ">>");

                args.RemoveAt(args.Count - 1);
            }

            string[] input = ASK_FOR_INPUT ? args.ToArray() : System.IO.File.ReadAllLines(INPUT_FILE);

            output = ToRun(input);
        }
        catch (Exception ex)
        {
            WriteLineColor(ex.ToString(), ConsoleColor.Red);
        }

        WriteColor("\nResult: ", ConsoleColor.Black, ConsoleColor.White);

        string outputString = output == null ? "[no result]" : output.ToString();

        WriteColor(outputString, ConsoleColor.Black, ConsoleColor.Yellow);
        WriteLineColor("", ConsoleColor.White);

        Clipboard.SetText(outputString);
        Console.WriteLine("Result copied to clipboard.");
    }

    public static void WriteColor(string arg, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;
        Console.Write(arg);
        Console.ResetColor();
    }
    public static void WriteLineColor(string arg, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black) =>
        WriteColor(arg + '\n', foregroundColor, backgroundColor);

    public struct Variable
    {
        public string name;
        public string value;

        public Variable(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
        public Variable(string name, int value)
        {
            this.name = name;
            this.value = value.ToString();
        }
    }
    private static Variable Var(string name, string value)
        => new Variable(name, value);
    private static Variable Var(string name, int value)
        => new Variable(name, value);

    public static void PrintStep(string line, int result, params Variable[] variables)
    {
        int delta = result - lastStep;
        lastStep = result;
        WriteColor("\"", ConsoleColor.Gray);
        WriteColor(line, ConsoleColor.White);
        WriteColor("\": ", ConsoleColor.Gray);
        WriteColor(result.ToString(), ConsoleColor.Cyan);
        WriteColor(" [", ConsoleColor.Gray);
        WriteColor((delta > 0 ? "+" : "") + delta.ToString(), delta >= 0 ? 
            (delta == 0 ? ConsoleColor.White : ConsoleColor.Green) : ConsoleColor.Red);
        WriteColor("]", ConsoleColor.Gray);

        foreach (Variable variable in variables)
        {
            WriteColor(" | ", ConsoleColor.Gray);
            WriteColor(variable.name, ConsoleColor.Magenta);
            WriteColor(": ", ConsoleColor.Gray);
            WriteColor(variable.value, ConsoleColor.Blue);
        }

        Console.WriteLine();
    }

    private static int d4v2(string[] input)
    {
        int amount = 0;
        foreach (string line in input)
        {
            string[] pairs = line.Split(',');
            List<string[]> numbers = new List<string[]>();

            foreach (string pair in pairs)
            {
                numbers.Add(pair.Split('-'));
            }

            if ((int.Parse(numbers[0][1]) >= int.Parse(numbers[1][0])) && (int.Parse(numbers[0][0]) <= int.Parse(numbers[1][1])))
                amount++;

            PrintStep(line, amount);
        }
        return amount;
    }
    private static int d4v1(string[] input)
    {
        int amount = 0;
        foreach (string line in input)
        {
            string[] pairs = line.Split(',');
            List<string[]> numbers = new List<string[]>();

            foreach (string pair in pairs)
            {
                numbers.Add(pair.Split('-'));
            }

            if ((int.Parse(numbers[1][0]) >= int.Parse(numbers[0][0]) && int.Parse(numbers[1][1]) <= int.Parse(numbers[0][1])) ||
                (int.Parse(numbers[0][0]) >= int.Parse(numbers[1][0]) && int.Parse(numbers[0][1]) <= int.Parse(numbers[1][1])))
                amount++;

            PrintStep(line, amount);
        }
        return amount;
    }
    private static int d3v2(string[] input)
    {
        int total = 0;

        List<char>? commonItems = null;

        for (int i = 0; i < input.Length; i++)
        {
            if (i % 3 == 0)
            {
                CheckCommons();
            }

            string line = input[i];

            List<char> newCommonItems = new List<char>();

            for (int ii = 0; ii < line.Length; ii++)
            {
                if (commonItems == null || commonItems.Contains(line[ii]))
                {
                    newCommonItems.Add(line[ii]);
                }
            }

            commonItems = newCommonItems.ToArray().ToList();
        }

        CheckCommons();

        return total;

        void CheckCommons()
        {
            if (commonItems != null)
            {
                total += commonItems[0] >= 'a' && commonItems[0] <= 'z' ? (commonItems[0] - 'a' + 1) : (commonItems[0] - 'A' + 27);
                PrintStep("?", total, Var("commonItems[0]", commonItems[0]));
            }
            commonItems = null;
        }
    }
    private static int d3v1(string[] input)
    {
        int total = 0;

        foreach (string line in input)
        {
            bool exit = false;
            for (int i = 0; i < line.Length / 2 && !exit; i++)
            {
                for (int ii = line.Length / 2; ii < line.Length && !exit; ii++)
                {
                    if (line[i] == line[ii])
                    {
                        total += line[i] >= 'a' && line[i] <= 'z' ? (line[i] - 'a' + 1) : (line[i] - 'A' + 27);
                        exit = true;
                        PrintStep(line, total, Var("line[i]", line[i]), Var("line[ii]", line[ii]));
                    }
                }
            }
        }

        return total;
    }
    private static int d2v2(string[] input)
    {
        int total = 0;
        foreach (string line in input)
        {
            int opponent = line[0] - 'A';
            int player = (line[2] - 'X' + 2 + opponent) % 3;

            total += (opponent == player ? 3 :
                (opponent + 1) % 3 == player % 3 ? 6 : 0)
                + player + 1;

            PrintStep(line, total, Var(nameof(opponent), opponent), Var(nameof(player), player));
        }
        return total;
    }

    private static int d2v1(string[] input)
    {
        int total = 0;
        foreach (string line in input)
        {
            int opponent = line[0] - 'A';
            int player = line[2] - 'X';

            total += (opponent == player ? 3 :
                (opponent + 1) % 3 == player % 3 ? 6 : 0)
                + player + 1;

            PrintStep(line, total, Var(nameof(opponent), opponent), Var(nameof(player), player));
        }
        return total;
    }

    private static int d1v2(string[] input)
    {
        List<int> list = new List<int>();

        int thisTotal = 0;
        foreach (string line in input)
        {
            if (line == "")
            {
                list.Add(thisTotal);
                thisTotal = 0;
            }
            else
            {
                thisTotal += int.Parse(line);
            }
            PrintStep(line, thisTotal);
        }

        int[] topHighest = new int[3];
        int max = int.MaxValue;

        for (int i = 0; i < topHighest.Length; i++)
        {
            int highest = -1;
            foreach (int item in list)
            {
                if (item > highest && item < max)
                {
                    highest = item;
                }
            }
            topHighest[i] = highest;
            max = highest;
        }

        int total = 0;
        foreach (int i in topHighest)
        {
            total += i;
        }

        return total;
    }

    private static int d1v1(string[] input)
    {
        List<int> list = new List<int>();
        
        int thisTotal = 0;
        foreach (string line in input)
        {
            if (line == "")
            {
                list.Add(thisTotal);
                thisTotal = 0;
            }
            else
            {
                thisTotal += int.Parse(line);
            }
            PrintStep(line, thisTotal);
        }

        int highest = -1;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > highest)
            {
                highest = list[i];
            }
        }

        return highest;
    }
}