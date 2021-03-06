﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;

/// <summary>
/// The controller for all the client actions.
/// </summary>

public class ClientManager : MonoBehaviour {

    NetworkClient client; //Define the network client.

    public string ipAddress = "127.0.0.1"; //Server IP Address.
    public int port = 4444; // Server Port.
    public string clientVersion = "0.0.4"; // Client version.

    public string inputBox = ""; //Create an empty string for the input box.
    bool pressedEnter = false; //Check if the Enter key has been pressed
    public Vector2 scrollPosition; //Current scroll position for the text box.

    List<string> messages = new List<string>(); //A list containing all received messages.

	// Use this for initialization
	void Start()
    {

        client = new NetworkClient(); //Initialize the network client.
        messages.Add("Connecting to server. (" + ipAddress + ":" + port + ")");
        client.Connect(ipAddress, port); //Connect to the server Ip on this port.
        client.RegisterHandler(MsgType.Connect, OnConnected); //Register method to call on connect.
        client.RegisterHandler(MsgType.Error, OnError); //Called on network error.
        client.RegisterHandler(MessageManager.Msg, OnMessage); //Register method to call when message received from server.
	}

    // Update is called once per frame
    void Update () {

        if(pressedEnter) //if pressedEnter becomes true.
        {
            if(inputBox != "") //if the inputbox string is not empty.
            {
                SendInput(inputBox); //Use the SendInput method to send the inputBox string to the server.
                inputBox = ""; //Clear the input box.
            }
            pressedEnter = false; //Set the pressedEnter bool to false.
        }
	
	}

    //Method called when connection successful.
    public void OnConnected(NetworkMessage netMsg)
    {
        messages.Add("Connected to the server.");
    }

    //Method called on network error (failed connection)/
    public void OnError(NetworkMessage netMsg)
    {
        messages.Add("Failed to connect to the server.");
    }

    //When a message is received from the server.
    public void OnMessage(NetworkMessage netMsg)
    {
        Message clientMessage = netMsg.ReadMessage<Message>();
        string message = "";

        if (clientMessage.type == MessageManager.messageType.Standard) // Normal messages.
        {
            message = clientMessage.message;
        }
        if (clientMessage.type == MessageManager.messageType.Error) // Error messages.
        {
            message = "<b><color=yellow>" + clientMessage.message + "</color></b>";
        }
        messages.Add(message); // Add the message to the display.
    }

    //Send the string to the server.
    public void SendInput(string text)
    {
        Message msg = new Message();
        msg.message = text;
        client.Send(MessageManager.Msg, msg);

        messages.Add(text);
        scrollPosition.y = messages.Count * 100;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, 200));
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("UniMud - v" + clientVersion);
                GUILayout.FlexibleSpace();
                if(client.isConnected)
                {
                    GUI.enabled = false;
                }
                else
                {
                    GUI.enabled = true;
                }
                if(GUILayout.Button("Connect"))
                {
                    client.Connect(ipAddress, port);
                    messages.Add("Connecting to server. (" + ipAddress + ":" + port + ")");
                }
                GUI.enabled = true;
        GUILayout.Space(20);
            GUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(20, 20, Screen.width - 40, Screen.height - 40));
            GUILayout.BeginVertical("box");
                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    foreach(string s in messages)
                    {
                        GUILayout.Label(s);
                    }
                GUILayout.EndScrollView();
                if (Event.current.Equals(Event.KeyboardEvent("return")))
                {
                    pressedEnter = true;
                }
                GUILayout.BeginHorizontal();
                    inputBox = GUILayout.TextField(inputBox);
                    if(GUILayout.Button("Submit", GUILayout.Width(75)))
                    {
                        pressedEnter = true;
                    }
                GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        GUILayout.EndArea();
    }



}
