public class Editor
{
    public static void MainLoop()
    {
        while (true)
        {
            Read();
        }
    }
    public static void Read()
    {
        var input = Console.ReadLine();
        if (input == "test")
        {
            Console.WriteLine("test");
        }
    }
}
