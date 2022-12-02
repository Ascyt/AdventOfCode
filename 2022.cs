class Program
{
    const string INPUT_FILE = @"C:\Users\FamZauner\Downloads\input";
    const bool ASK_FOR_INPUT = false;
    static Method ToRun = d2v2;


    delegate int Method(string[] input);

    private static string stepOutput = "";
    private static int lastStep;

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

            string[] input = ASK_FOR_INPUT ? args.ToArray() : File.ReadAllLines(INPUT_FILE);

            output = ToRun(input);
        }
        catch (Exception ex)
        {
            WriteLineColor(ex.ToString(), ConsoleColor.Red);
        }

        WriteColor(stepOutput, ConsoleColor.DarkGray);
        WriteLineColor(output == null ? "[no result]" : output.ToString(), ConsoleColor.Cyan);
    }

    public static void WriteColor(string arg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(arg);
        Console.ResetColor();
    }
    public static void WriteLineColor(string arg, ConsoleColor color) =>
        WriteColor(arg + '\n', color);

    public static void PrintStep(string line, int result)
    {
        int delta = result - lastStep;
        lastStep = result;
        stepOutput += $"\"{line}\": {result} [{(delta > 0 ? "+" : "")}{delta}]\n";
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

            PrintStep(line, total);
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

            PrintStep(line, total);
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