using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerManager : MonoBehaviour {

    public List<DataManager.Player> players = new List<DataManager.Player>(); //The list of Players.
    public List<DataManager.Room> world = new List<DataManager.Room>(); //List of world rooms.
    public int Port = 4444;

    // Use this for initialization
    void Start () {

        NetworkServer.Listen(Port); //Set the servers Listen Port.
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnect); //Register a method to handle Connections.
        NetworkServer.RegisterHandler(DataManager.Type.Message, OnMessage); //Register a method to handle messages from clients.
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnect); //Register a method to handle disconnections.

        //Dirty way to set up the initial room on ID 0 where all players spawn for now.
        int?[] spawn = new int?[4];
        world.Add(new DataManager.Room(0, spawn, "This is the spawn area."));
	
	}

    //The method that is run every time a player connects.
    public void OnConnect(NetworkMessage msg)
    {
        DataManager.Message newMsg = new DataManager.Message(); //Create a new data message.
        newMsg.msg = "SERVER: Connection ID " + msg.conn.connectionId.ToString() + " Connected!"; //Set it's message string.
        NetworkServer.SendToAll(DataManager.Type.Message, newMsg); //Send the message to all connected clients.
        AddPlayer(msg.conn.connectionId); //Run the add player method.
    }

    //The method that is run every time a player disconnects.
    public void OnDisconnect(NetworkMessage msg)
    {
        DataManager.Message newMsg = new DataManager.Message(); //Create a new data message.
        newMsg.msg = "SERVER: Connection ID " + msg.conn.connectionId.ToString() + " Disconnected!"; //Set it's message string.
        NetworkServer.SendToAll(DataManager.Type.Message, newMsg); //Send the message to all connected clients.
        RemovePlayer(msg.conn.connectionId); //Run the remove player method.
    }

    //The method that is called when a client sends a message to the server.
    public void OnMessage(NetworkMessage msg)
    {
        HandleMessage(msg.ReadMessage<DataManager.Message>().msg, msg.conn.connectionId); //Run the handle message method and pass it the string contained in the message.
    }

    //The method that handles all message strings.
    public void HandleMessage(string message, int clientId)
    {
        message.ToLower(); //Convert the string to lower-case.
        string[] msg = message.Split(' '); //split the string into an array of words split by a space.

        if (msg[0] == "look") //if the first word is "look"
        {
            if(msg.Length == 1) //if the message string is only 1 word long.
            {
                DataManager.Room room = GetRoom(GetPlayer(clientId).roomId); //Get the players current room.
                SendMsg(clientId, room.description); //Use the Send Messagge method to send a message to the client.
                string availableDir = ""; //Create a quick and dirty string to hold availble directions.
                //if each of the direction variables have a value, add that direction to the 'availableDir' string.
                if(room.connections[0].HasValue)
                {
                    availableDir += "North,";
                }
                if (room.connections[1].HasValue)
                {
                    availableDir += "East,";
                }
                if (room.connections[2].HasValue)
                {
                    availableDir += "South,";
                }
                if (room.connections[3].HasValue)
                {
                    availableDir += "West,";
                }
                SendMsg(clientId, "Available Directions:" + availableDir);//Send a message to the client with the available directions

                string playersInRoom = ""; //Another dirty string, this time for players.
                //run through each online player and check if theyre in the same room.
                foreach(DataManager.Player p in players)
                {
                    if(p.clientId != clientId) //ignore it if the it's the current player.
                    {
                        if(p.roomId == room.id) //if in the same room as the current player
                        {
                            playersInRoom += p.clientId + ", "; //add the player ID to the 'playersInRoom' string.
                        }
                    }
                }
                SendMsg(clientId, "Players Here: " + playersInRoom); //Send the message of players in the room.

            }
        }
        else if(msg[0] == "walk") //if the message begins with 'walk'
        {
            DataManager.Player player = GetPlayer(clientId); //Get the current player.
            DataManager.Room room = GetRoom(player.roomId); //Get the current players room.
            int dir = 0; //quick int to hold the direction the player wants to head.
            //Set the direction int to the direction dictated by the 2nd word in the message.
            if (msg[1] == "north")
                dir = 0;
            if (msg[1] == "east")
                dir = 1;
            if (msg[1] == "south")
                dir = 2;
            if (msg[1] == "west")
                dir = 3;
            if (room.connections[dir].HasValue)//if the current room has a connecting room in that direction, go there.
            {
                player.roomId = room.connections[dir].Value;
                SendMsg(clientId, "You move to the " + msg[1] + ".");
            }
            else //Otherwise tell the player they can't move that way.
            {
                SendMsg(clientId, "There is nowhere to move in that direction.");
            }
        }
        else if (msg[0] == "who") //the first command added, checks if the first word is 'who'
        {
            SendMsg(clientId, "You're connection ID is " + clientId.ToString() + "."); //sends a message containing the users ID.
        }
        else if(msg[0] == "/admin")//check if '/admin' is the forst word in the message.
        {
            if(msg[1] == "addroom") //checks if the second word is 'addroom'.
            {
                //Checks for the desired direction, and adds a room in that direction (does not current check if a room exists).
                if(msg[2] == "north")
                {
                    AddRoom(GetPlayer(clientId).roomId, 0, "Test");
                    SendMsg(clientId, "Room created to the " + msg[2] + ".");
                }
                else if (msg[2] == "east")
                {
                    AddRoom(GetPlayer(clientId).roomId, 1, "Test");
                    SendMsg(clientId, "Room created to the " + msg[2] + ".");
                }
                else if (msg[2] == "south")
                {
                    AddRoom(GetPlayer(clientId).roomId, 2, "Test");
                    SendMsg(clientId, "Room created to the " + msg[2] + ".");
                }
                else if (msg[2] == "west")
                {
                    AddRoom(GetPlayer(clientId).roomId, 3, "Test");
                    SendMsg(clientId, "Room created to the " + msg[2] + ".");
                }
                else
                {
                    SendMsg(clientId, "SERVER: COMMAND USAGE: '/admin addroom [direction]' (North, East, South, West)"); //if no direction is specified, send a message to specify command usage.
                }
            }

        }
    }

    //Format a string into a message and send it to the client Id provided.
    public void SendMsg(int clientId, string message)
    {
        DataManager.Message newMsg = new DataManager.Message();
        newMsg.msg = message;
        NetworkServer.SendToClient(clientId, DataManager.Type.Message, newMsg);
    }

    //Add a room to the world list using an existing room as a starting point, a direction, and a description.
    public void AddRoom(int currentId, int direction, string desc)
    {
        List<DataManager.Room> roomToAdd = new List<DataManager.Room>(); //Quick/dirty way to avoid modifying the existing world list till the foreach statement is complete.

        foreach(DataManager.Room r in world) //run through each existing room.
        {
            if(r.id == currentId) //if the ID matches our starting position.
            {
                if(r.connections[direction].HasValue == false) //check if there is an existing room in the desired direction.
                {
                    //Create the connection array for the new room, and place the current room into it, so you can move back.
                    int?[] newCon = new int?[4];
                    if (direction == 0)
                        newCon[2] = currentId;
                    if (direction == 1)
                        newCon[3] = currentId;
                    if (direction == 2)
                        newCon[0] = currentId;
                    if (direction == 3)
                        newCon[1] = currentId;
                    int id = GetFreeRoomID();
                    roomToAdd.Add(new DataManager.Room(id, newCon, desc)); //Add the new room to the "rooms to add" list.
                    r.connections[direction] = id; //add the new room to the existing room in the desired direction.
                }
            }
        }
        //run through the rooms to add list and add the room to the world database.
        foreach(DataManager.Room r in roomToAdd)
        {
            world.Add(r);
        }
    }

    //Grab the next available room Id.
    public int GetFreeRoomID()
    {
        int id = 0;
        foreach(DataManager.Room r in world)
        {
            if(r.id >= id)
            {
                id = r.id + 1;
            }
        }
        return id;
    }

    public void AddPlayer(int id)
    {
        players.Add(new DataManager.Player(id, 100, 0));
    }
    public void RemovePlayer(int id)
    {
        DataManager.Player toRemove = null;
        foreach(DataManager.Player p in players)
        {
            if(p.clientId == id)
            {
                toRemove = p;
            }
        }
        players.Remove(toRemove);
    }

    //Get the player from a player Id.
    public DataManager.Player GetPlayer(int id)
    {
        foreach(DataManager.Player p in players)
        {
            if(p.clientId == id)
            {
                return p;
            }
        }
        return null;
    }

    //Get a room from a room Id.
    public DataManager.Room GetRoom(int id)
    {
        foreach(DataManager.Room r in world)
        {
            if(r.id == id)
            {
                return r;
            }
        }
        return null;
    }


}
