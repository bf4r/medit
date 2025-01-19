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
    public void ModeIndicator(string modeText)
    {
        Console.Write(modeText + " ~> ");
    }
    public void Read()
    {
        if (CurrentBuffer == null) Mode = "command";

        if (Mode == "command")
        {
            ModeIndicator("C");
            var input = Console.ReadLine();
            if (input == "t")
            {
                Mode = "text";
                if (CurrentBuffer == null)
                {
                    string newBufferName = Guid.NewGuid().ToString();
                    Buffers.Add(newBufferName, "");
                    CurrentBuffer = newBufferName;
                }
            }
            if (input == "n")
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
            else if (input == "s")
            {
                Mode = "save";
            }
            else if (input == "bi")
            {
                // Buffer ID
                Console.WriteLine(CurrentBuffer);
            }
            else if (input == "r")
            {
                // Read buffer
                if (CurrentBuffer != null)
                {
                    Console.WriteLine(Buffers[CurrentBuffer]);
                }
            }
        }
        else if (Mode == "text")
        {
            ModeIndicator("T");
            string input = Console.ReadLine() ?? "";
            if (input.StartsWith(@"\"))
            {
                input = input.Substring(1);
            }
            if (input == "@c")
            {
                Mode = "command";
            }
            else
            {
                Buffers[CurrentBuffer!] += (Buffers[CurrentBuffer!].Length == 0 ? "" : Environment.NewLine) + input;
            }
        }
        else if (Mode == "save")
        {
            ModeIndicator("S");
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
