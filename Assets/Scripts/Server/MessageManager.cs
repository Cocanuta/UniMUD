using UnityEngine.Networking;

public class MessageManager {

    public static short Msg = MsgType.Highest + 1;
    public enum messageType { Standard, Error };

    public enum commandType { None, Engine, Movement, Action, Combat, Inventory };

    // All possible movement commands.
    public static string movementCommands = ",n,ne,e,se,s,sw,w,nw,north,northeast,east,southeast,south,southwest,west,northwest,move,walk,run,sneak,travel,u,d,up,down,en,ex,enter,exit,";

    public static void ProcessMessage(int clientID, string fullMessage)
    {
        commandType messageType = commandType.None;
        string[] splitMessage = fullMessage.Split(' ');

        // Find out what kind of command has been entered.
        if (movementCommands.Contains(("," + splitMessage[0] + ",").ToString())) { messageType = commandType.Movement; }

        // If the command type is still not assigned, quit out and tell the client the command is invalid.
        if (messageType.Equals(commandType.None))
        {
            SendToClient(MessageManager.messageType.Error, "Invalid command.", clientID);
            return;
        }

        // Process movement commands.
        if (messageType.Equals(commandType.Movement)) { Movement.ProcessCommand(fullMessage, clientID); }

    }

    // Sends a message to the client with a specified type.
    public static void SendToClient(messageType type, string message, int id)
    {
        Message clientMessage = new Message();
        clientMessage.type = type;
        clientMessage.message = message;
        NetworkServer.SendToClient(id, Msg, clientMessage);
    }
}

public class Message : MessageBase
{
    public string message;
    public MessageManager.messageType type;
}