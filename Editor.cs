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
    // command, text, save
    public string Mode = "command";
    public Dictionary<string, string> Buffers = new();
    public string? CurrentBuffer;
    public void Read()
    {
        if (CurrentBuffer == null) Mode = "command";

        if (Mode == "command")
        {
            Console.Write("C");
            var input = Console.ReadLine();
            if (input == "t")
            {
                Mode = "text";
                string newBufferName = Guid.NewGuid().ToString();
                Buffers.Add(newBufferName, "");
                CurrentBuffer = newBufferName;
            }
            else if (input == "q")
            {
                Environment.Exit(0);
            }
        }
        else if (Mode == "text")
        {
            Console.Write("T");
            string input = Console.ReadLine() ?? "";
            if (input.StartsWith(@"\"))
            {
                input = input.Substring(1);
            }

            if (input == "@!c")
            {
                Mode = "command";
            }
            else if (input == "@!s")
            {
                Mode = "save";
            }
            // Buffer ID
            else if (input == "@/bi")
            {
                Console.WriteLine(CurrentBuffer);
            }
            else
            {
                Buffers[CurrentBuffer!] += input + Environment.NewLine;
            }
        }
        else if (Mode == "save")
        {
            Console.Write("S");
            string input = Console.ReadLine()!;
            if (string.IsNullOrEmpty(input)) return;
            string fileName = input;
            string workingDirectory = Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(workingDirectory, fileName);
            string bufferText = Buffers[CurrentBuffer!];
            File.WriteAllText(fullPath, bufferText);
            Mode = "command";
        }
    }
}
