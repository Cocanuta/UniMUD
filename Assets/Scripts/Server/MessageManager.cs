using UnityEngine;
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

    public static void ProcessConsole(string cmd)
    {
        string[] command = cmd.ToLower().Split(' ');
        switch(command[0])
        {
            case "shutdown":
                Debug.Log("Server Stopping...");
                NetworkServer.Shutdown();
                ServerManager.userDatabase.Save();
                Debug.Log("User/Character Database Saved.");
                ServerManager.itemDatabase.Save();
                Debug.Log("Item Database Saved.");
                ServerManager.worldDatabase.Save();
                Debug.Log("World Database Saved.");
                Debug.Log("Server Shutdown.");
                Application.Quit();
                break;
            case "online":
                string characters = "";
                if(ServerManager.userDatabase.OnlineCharacters.Count > 0)
                {
                    foreach (Character c in ServerManager.userDatabase.OnlineCharacters)
                    {
                        characters += c.Name + " ";
                    }
                }
                else
                {
                    characters = "None.";
                }
                
                Debug.Log("Online Characters: " + characters);
                break;
            case "save":
                Debug.Log("Save Started...");
                ServerManager.userDatabase.Save();
                Debug.Log("User/Character Database Saved.");
                ServerManager.itemDatabase.Save();
                Debug.Log("Item Database Saved.");
                ServerManager.worldDatabase.Save();
                Debug.Log("World Database Saved.");
                Debug.Log("Saving complete.");
                break;
            default:
                Debug.Log("Invalid Command!");
                Debug.Log("Available Commands: Online, Save, Shutdown");
                break;
        }
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