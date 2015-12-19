using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

/// <summary>
/// Quick and dirty class to hold all data types.
/// </summary>
public class DataManager : MonoBehaviour {

    //Our own custom network data type holder
    public class Type
    {
        public static short Message = MsgType.Highest + 1;
    };

    //Our message data type.
    public class Message : MessageBase
    {
        public string msg;
        public int clientId;
    }

    //Player class
    public class Player
    {
        public int clientId;
        public int health;
        public int roomId;
        public Player(int newId, int newHealth, int newRoomId)
        {
            clientId = newId;
            health = newHealth;
            roomId = newRoomId;
        }
    }

    //Room class
    public class Room
    {
        public int id;
        public int?[] connections;
        public string description;
        public Room(int newId, int?[] newConnections, string newDescription)
        {
            id = newId;
            connections = newConnections;
            description = newDescription;
        }
    }
}
