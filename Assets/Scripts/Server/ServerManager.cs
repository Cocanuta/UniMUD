using UnityEngine;
using UnityEngine.Networking;
using System.Data;
using System.Collections.Generic;
using WebSocketSharp.Server;

public class ServerManager : MonoBehaviour {

    public int Port = 4444; //Server port.

    public static DatabaseManager dbManager;
    public static List<User> OnlineUsers = new List<User>();
    public static List<Character> OnlineCharacters = new List<Character>();


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
