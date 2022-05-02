using GlobalHotKeys;
using GlobalHotKeys.Native.Types;
using WebSocketSharp.Server;

partial class Program
{
    public bool Connected = false;
    public LSO LSO;
    const string _address = "ws://localhost";
    const ConsoleKey _remapKey = ConsoleKey.R;
    readonly UI _ui = new(_address, _remapKey.ToString());
    Dictionary<string, string> _binds;
    readonly (string Label, string Command)[] _controls = {
        ("Start/Split", "splitorstart"),
        ("Reset", "reset"),
        ("Skip Split", "skip"),
        ("Undo Split", "undo"),
        ("Pause", "togglepause"),
    };
    readonly int _count;
    HotKeyManager _hotKeyManager = new();
    IDisposable _subscription;
    List<IRegistration> _registrations = new();
    readonly WebSocketServer _server = new(_address);
    static void Main()
    {
        new Program();
    }

    Program()
    {
        _count = _controls.Count();
        
        var keys = new string[_count];

        string path = @$"{Directory.GetCurrentDirectory()}\\keys.txt";

        if (File.Exists(path))
        {
            keys = System.IO.File.ReadAllLines(path);
            SetBinds(keys);
        }
        else
            Array.Fill(keys, "");

        _ui.WaitingConnection();
        
        for (int i = 0; i < _count; i++)
            _ui.InitBind(i, _controls[i].Label, keys[i]);

        _ui.Waiting();
        
        _server.Start();
        _server.AddWebSocketService<LSO>("/", (LSO lso) => {
            lso.Program = this;
        });

        Loop();
    }

    void Loop()
    {
        ConsoleKey key;

        do
        {
            key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.R)
                Remap();
        } while (key != ConsoleKey.Enter);

        _hotKeyManager.Dispose();
        _server.Stop();
    }

    void SetBinds(string[] keys)
    {
        _binds = new();
        
        for (int i = 0; i < _count; i++)
        {
            var key = keys[i];

            if (key == "")
                continue;
            
            var keyCode = _toKeyCode[key];
            
            _binds[keyCode.ToString()] = _controls[i].Command;

            var registration = _hotKeyManager.Register(keyCode, Modifiers.NoRepeat);

            _registrations.Add(registration);
        }
    }

    void Remap()
    {
        foreach (var registration in _registrations)
            registration.Dispose();

        _registrations = new();

        var keys = new string[_count];

        _ui.Mapping();

        for (int i = 0; i < _count; i++)
        {
            _ui.MapKey(i, _controls[i].Label);

            ConsoleKey key = Console.ReadKey(true).Key;
            
            if (key == ConsoleKey.Enter)
            {
                Console.Write("None");

                keys.SetValue("", i);

                continue;
            }
            
            var keyStr = key.ToString();

            Console.Write(keyStr);

            keys.SetValue(keyStr, i);
        }

        SetBinds(keys);

        File.WriteAllLinesAsync("keys.txt", keys);

        _ui.Waiting();
    }

    void HotKeyPressed(HotKey hotKey)
    {
        LSO.Send(_binds[hotKey.Key.ToString()]);
    }

    public void OnConnect()
    {
        Connected = true;
        
        _subscription = _hotKeyManager.HotKeyPressed.Subscribe(HotKeyPressed);
        
        _ui.Connected();
    }

    public void OnDisconnect()
    {
        Connected = false;
        
        _subscription.Dispose();
        
        _ui.WaitingConnection();
    }
}