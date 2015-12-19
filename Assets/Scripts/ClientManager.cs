using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;

/// <summary>
/// The controller for all the client actions.
/// </summary>

public class ClientManager : MonoBehaviour {

    NetworkClient client; //Define the network client.

    public string ipAddress = "127.0.0.1";
    public int port = 4444;

    public string inputBox = ""; //Create an empty string for the input box.
    bool pressedEnter = false; //Check if the Enter key has been pressed
    public Vector2 scrollPosition; //Current scroll position for the text box.

    List<string> messages = new List<string>(); //A list containing all received messages.

	// Use this for initialization
	void Start () {

        client = new NetworkClient(); //Initialize the network client.
        client.Connect(ipAddress, port); //Connect to the server Ip on this port.
        client.RegisterHandler(MsgType.Connect, OnConnected); //Register method to call on connect.
        client.RegisterHandler(DataManager.Type.Message, OnMessage); //Register method to call when message received from server.

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
        Debug.Log("Connected to server");
    }

    //When a message is received from the server.
    public void OnMessage(NetworkMessage netMsg)
    {
        messages.Add(netMsg.ReadMessage<DataManager.Message>().msg); //Add the message to the messages list.
    }

    //Send the string to the server.
    public void SendInput(string text)
    {
        DataManager.Message msg = new DataManager.Message();
        msg.msg = text;
        client.Send(DataManager.Type.Message, msg);
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height - 20));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 20));
        foreach(string s in messages)
        {
            GUILayout.Label(s);
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(0, Screen.height - 20, Screen.width, 20));
        GUILayout.BeginHorizontal();
        if (Event.current.Equals(Event.KeyboardEvent("return")))
        {
            pressedEnter = true;
        }
        inputBox = GUILayout.TextField(inputBox, 25, GUILayout.Width(Screen.width));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }



}
