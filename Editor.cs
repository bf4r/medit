namespace Medit;
using System.Text;
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
    public int CurrentLine;
    public void ModeIndicator(string modeText)
    {
        Console.Write(modeText + " ~> ");
    }
    public string? CustomReadLine()
    {
        var sb = new StringBuilder();
        int startPos = Console.CursorLeft;
        int startLine = Console.CursorTop;
        int position = 0;
        // relative to start of input, how many lines down we are
        int linePosition = 0;
        while (true)
        {
            ConsoleKeyInfo ki = Console.ReadKey(true);
            ConsoleKey key = ki.Key;
            char kc = ki.KeyChar;
            switch (key)
            {
                case ConsoleKey.Enter:
                    Console.WriteLine();
                    return sb.ToString();
                case ConsoleKey.Backspace:
                    if (position > 0)
                    {
                        Console.Write("\b \b");
                        if (position != sb.Length)
                        {
                            var remainingText = sb.ToString().Substring(position);
                            Console.Write(remainingText + ' ');
                            Console.Write(new string('\b', remainingText.Length + 1));
                        }
                        sb.Remove(position - 1, 1);
                        position--;
                    }
                    break;
                case ConsoleKey.Escape:
                    Mode = "command";
                    Console.SetCursorPosition(0, Console.CursorTop);
                    return null;
                case ConsoleKey.LeftArrow:
                    if (position > 0) position--;
                    break;
                case ConsoleKey.RightArrow:
                    if (position < sb.Length) position++;
                    break;
                case ConsoleKey.Home:
                    position = 0;
                    break;
                case ConsoleKey.End:
                    position = sb.Length;
                    break;
                default:
                    sb.Insert(position, kc);
                    if (position != sb.Length)
                    {
                        string remainingText = sb.ToString().Substring(position);
                        Console.Write(remainingText);
                        Console.Write(new string('\b', remainingText.Length));
                    }
                    position++;
                    Console.Write(kc);
                    break;
            }
            Console.SetCursorPosition(startPos + position, startLine + linePosition);
        }
    }
    public void Read()
    {
        if (CurrentBuffer == null) Mode = "command";

        switch (Mode)
        {
            case "command":
                {
                    ModeIndicator("C");
                    var input = Console.ReadLine();
                    switch (input)
                    {
                        case "t":
                            {
                                Mode = "text";
                                if (CurrentBuffer == null)
                                {
                                    string newBufferName = Guid.NewGuid().ToString();
                                    Buffers.Add(newBufferName, "");
                                    CurrentBuffer = newBufferName;
                                }
                            }
                            break;
                        case "n":
                            {
                                Mode = "text";
                                string newBufferName = Guid.NewGuid().ToString();
                                Buffers.Add(newBufferName, "");
                                CurrentBuffer = newBufferName;
                            }
                            break;
                        case "q":
                            Environment.Exit(0);
                            break;
                        case "s":
                            Mode = "save";
                            break;
                        case "bi":
                            // Buffer ID
                            Console.WriteLine(CurrentBuffer);
                            break;
                        case "j":
                            // Jump to buffer
                            Mode = "jump";
                            break;
                        case "w":
                            // Where am I?
                            if (CurrentBuffer != null)
                            {
                                Console.WriteLine(CurrentLine.ToString().PadRight(4) + Buffers[CurrentBuffer].Split(Environment.NewLine)[CurrentLine]);
                            }
                            break;
                        case "r":
                            // Read buffer
                            if (CurrentBuffer != null)
                            {
                                Console.WriteLine(Buffers[CurrentBuffer]);
                            }
                            break;
                        case "rn":
                            // Read buffer with line numbers
                            if (CurrentBuffer != null)
                            {
                                var lines = Buffers[CurrentBuffer].Split(Environment.NewLine);
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    bool onCurrentLine = i == CurrentLine;
                                    if (onCurrentLine) Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(i.ToString().PadRight(4) + lines[i]);
                                    if (onCurrentLine) Console.ResetColor();
                                }
                            }
                            break;
                        default:
                            Console.WriteLine("what");
                            break;
                    }
                }
                break;
            case "text":
                {
                    ModeIndicator("T");
                    string? input = CustomReadLine();
                    if (input == null) return;
                    List<string> lines = Buffers[CurrentBuffer!].Split(Environment.NewLine).ToList();
                    lines.Insert(CurrentLine, input);
                    Buffers[CurrentBuffer!] = string.Join(Environment.NewLine, lines);
                    CurrentLine++;
                }
                break;
            case "save":
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
                break;
            case "jump":
                {
                    ModeIndicator("J");
                    string input = Console.ReadLine()!;
                    if (string.IsNullOrEmpty(input)) return;
                    if (int.TryParse(input, out int desiredLine))
                    {
                        int bufferLineCount = Buffers[CurrentBuffer!].Split(Environment.NewLine).Length;
                        if (desiredLine > Buffers[CurrentBuffer!].Split(Environment.NewLine).Length)
                        {
                            Console.WriteLine("max " + bufferLineCount);
                        }
                        else
                        {
                            CurrentLine = desiredLine;
                        }
                    }
                    else Console.WriteLine("bad number");
                    Mode = "command";
                }
                break;
        }
    }
}
