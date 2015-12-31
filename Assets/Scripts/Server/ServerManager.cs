using UnityEngine;
using UnityEngine.Networking;
using System.Data;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public int Port = 4444; //Server port.

    public static DatabaseManager dbManager;
    public static List<User> OnlineUsers = new List<User>();
    public static List<Character> OnlineCharacters = new List<Character>();
    public static List<Planet> Planets;

    public enum commandType { None, Engine, Movement, Action, Combat, Inventory };

    // All possible movement commands.
    public static string movementCommands = ",n,ne,e,se,s,sw,w,nw,north,northeast,east,southeast,south,southwest,west,northwest,move,walk,run,sneak,travel,u,d,up,down,enter,exit,";

    // Use this for initialization
    void Awake ()
    {
        dbManager = new DatabaseManager("sql4.freemysqlhosting.net", "sql4101156", "sql4101156", "9gveG2W4Lw", true);

        NetworkServer.Listen(Port); //Set the servers Listen Port.
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnect); //Register a method to handle Connections.
        NetworkServer.RegisterHandler(Data.Msg, OnMessage); //Register a method to handle messages from clients.
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnect); //Register a method to handle disconnections.
        NetworkServer.useWebSockets = true;
        UserManager.Login(0, "TestUser3", "Password");
        Debug.Log("Logged In Users:");
        foreach(User u in OnlineUsers)
        {
            Debug.Log(u.ID + ". " + u.Name);
        }
    }

    //The method that is run every time a player connects.
    public void OnConnect(NetworkMessage msg)
    {
        //
        //Do stuff for logging in.
        //
    }

    //The method that is run every time a player disconnects.
    public void OnDisconnect(NetworkMessage msg)
    {
        //
        //Do stuff for logging out.
        //
    }

    //The method that is called when a client sends a message to the server.
    public void OnMessage(NetworkMessage msg)
    {
        //
        //Do stuff when receiving a message from the client.
        //

        // Obtain the client ID, full message, and the message prefix.
        int clientID = msg.conn.connectionId;
        string fullMessage = msg.ReadMessage<Data.Message>().message.ToLower();
        string[] prefixMessage = fullMessage.Split(' ');
        commandType messageType = commandType.None;

        // Find out what kind of command has been entered.
        if (movementCommands.Contains(("," + prefixMessage[0] + ",").ToString())) { messageType = commandType.Movement; }

        // If the command type is still not assigned, quit out and tell the client the command is invalid.
        if (messageType.Equals(commandType.None))
        {
            SendToClient(Data.messageType.Error, "Invalid command.", clientID);
            return;
        }

        // Process movement commands.
        if (messageType.Equals(commandType.Movement)) { Movement.ProcessCommand(fullMessage, clientID); }
    }

    // Sends a message to the client with a specified type.
    public static void SendToClient(Data.messageType type, string message, int id)
    {
        Data.Message clientMessage = new Data.Message();
        clientMessage.type = type;
        clientMessage.message = message;
        NetworkServer.SendToClient(id, Data.Msg, clientMessage);
    }

    void OnApplicationQuit()
    {
        Debug.Log("Closing Connection");
        if (dbManager.con != null)
        {
            if (dbManager.con.State != ConnectionState.Closed)
                dbManager.con.Close();
            dbManager.con.Dispose();
        }
    }

}
