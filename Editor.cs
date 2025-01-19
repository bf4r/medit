namespace Medit;
public class Editor
{
    public void MainLoop()
    {
        while (true)
        {
            Read();
        }
    }
    // text, command
    public string Mode = "command";
    public Dictionary<string, string> Buffers = new();
    public string? CurrentBuffer;
    public void Read()
    {
        if (CurrentBuffer == null)
        {
            Mode = "command";
        }
        if (Mode == "command")
        {
            Console.WriteLine("C");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            switch (key)
            {
                case ConsoleKey.N:
                    Mode = "text";
                    string newBufferName = Guid.NewGuid().ToString();
                    Buffers.Add(newBufferName, "");
                    CurrentBuffer = newBufferName;
                    break;
            }
        }
        else if (Mode == "text")
        {
            string input = Console.ReadLine() ?? "";
            if (input.StartsWith(@"\"))
            {
                input = input.Substring(1);
            }
            if (input == "@C")
            {
                Mode = "command";
            }
            Buffers[CurrentBuffer!] += Environment.NewLine + input;
        }
    }
}
