using System.Runtime.InteropServices;

class UI
{
    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);
    (int Left, int Top) _pos = (0, 10);
    readonly string _serverAddress;
    readonly string _remapKey;

    public UI(string serverAddress, string remapKey)
    {
        _serverAddress = serverAddress;
        _remapKey = remapKey;

        Console.Clear();

        SetConsoleMode(GetStdHandle(-10), 0x0080);
    }

    public void Connected()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write(new string(' ', 86));
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("LiveSplit One connected.");
        Console.ResetColor();
        Console.SetCursorPosition(_pos.Left, _pos.Top);
    }

    public void WaitingConnection()
    {
        Thread t = new Thread(() => Clipboard.SetText(_serverAddress));
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        
        Console.SetCursorPosition(0, 0);
        Console.Write($"Awaiting LiveSplit One connection to server '{_serverAddress}' (copied to clipboard).");
        Console.SetCursorPosition(_pos.Left, _pos.Top);
    }

    public void Mapping()
    {
        Console.SetCursorPosition(0, 8);
        Console.Write("Press any key to map, or ENTER to skip.");
    }

    public void Waiting()
    {
        Console.SetCursorPosition(0, 8);
        Console.Write(new string(' ', 40));
        Console.SetCursorPosition(0, 8);
        Console.Write($"Press {_remapKey} to remap, or ENTER to exit.");
        Console.SetCursorPosition(0, 10);
        
        _pos = (0, 10);
    }

    public void MapKey(int i, string label)
    {
        int top = 2 + i;

        Console.SetCursorPosition(0, top);
        Console.Write(new string(' ', 30));
        Console.SetCursorPosition(0, top);
        Console.Write($"{label}: ");

        _pos = Console.GetCursorPosition();
    }

    public void InitBind(int i, string label, string key)
    {
        key = key == "" ? "None" : key;

        Console.SetCursorPosition(0, 2 + i);
        Console.Write($"{label}: {key}");
    }
}