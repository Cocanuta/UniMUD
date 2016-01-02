using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public int Port = 4444; //Server port.

    public static UserManager userDatabase;
    public static WorldManager worldDatabase;

    // Use this for initialization
    void Awake ()
    {
        Debug.Log("Starting Server...");
        userDatabase = UserManager.Load();
        Debug.Log("User/Character Database Loaded.");
        worldDatabase = WorldManager.Load();
        Debug.Log("World Database Loaded.");
        NetworkServer.Listen(Port); //Set the servers Listen Port.
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnect); //Register a method to handle Connections.
        NetworkServer.RegisterHandler(MessageManager.Msg, OnMessage); //Register a method to handle messages from clients.
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnect); //Register a method to handle disconnections.
        if(NetworkServer.active)
        {
            Debug.Log("Server Started On Port " + NetworkServer.listenPort.ToString() + ".");
        }

        worldDatabase.AddPlanet("Earth");

    }

    //The method that is run every time a player connects.
    public void OnConnect(NetworkMessage msg)
    {
        Debug.Log("Client " + msg.conn.connectionId.ToString() + " Connected.");
        MessageManager.SendToClient(MessageManager.messageType.Standard, "Welcome to the server!", msg.conn.connectionId);
    }

    //The method that is run every time a player disconnects.
    public void OnDisconnect(NetworkMessage msg)
    {
        Debug.Log("Client " + msg.conn.connectionId.ToString() + " Disconnected.");
    }

    //The method that is called when a client sends a message to the server.
    public void OnMessage(NetworkMessage msg)
    {
        // Obtain the client ID, full message, and the message prefix.
        int clientID = msg.conn.connectionId;
        string fullMessage = msg.ReadMessage<Message>().message.ToLower();

        MessageManager.ProcessMessage(clientID, fullMessage);
    }

    void OnApplicationQuit()
    {
        Debug.Log("Server Stopping...");
        NetworkServer.Shutdown();
        userDatabase.Save();
        Debug.Log("User/Character Database Saved.");
        worldDatabase.Save();
        Debug.Log("World Database Saved.");
        Debug.Log("Server Shutdown.");
    }

}
