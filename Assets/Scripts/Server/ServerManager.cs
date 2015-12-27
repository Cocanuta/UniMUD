using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerManager : MonoBehaviour {

    public int Port = 4444; //Server port.
    private DatabaseManager dbManager;

    // Use this for initialization
    void Awake () {
        NetworkServer.Listen(Port); //Set the servers Listen Port.
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnect); //Register a method to handle Connections.
        NetworkServer.RegisterHandler(Data.Msg, OnMessage); //Register a method to handle messages from clients.
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnect); //Register a method to handle disconnections.
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

}
