using WebSocketSharp;
using WebSocketSharp.Server;

class LSO : WebSocketBehavior
{
    public Program Program;
    protected override void OnOpen()
    {
        if (Program.Connected)
        {
            Close();
            
            return;
        }

        Program.LSO = this;

        Program.OnConnect();

        while (ConnectionState == WebSocketState.Open);

        Program.OnDisconnect();
    }

    public new void Send(string message)
    {
        base.Send(message);
    }
}