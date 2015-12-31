//-----
// Handles all movement commands sent from the client, moves the character as appropriate, and updates the client.
//-----

using UnityEngine;
using System.Collections;

public class Movement {

    public static string direction;   // The direction to travel in.
    public static string speed;       // The speed to travel.

    // The command will either be simple or complex, either consisting of just a direction or a direction with additional instruction.
    private static string simpleMove = ",n,ne,e,se,s,sw,w,nw,north,northeast,east,southeast,south,southwest,west,northwest,u,d,up,down,enter,exit," ;
    private static string complexMove = ",move,walk,run,sneak,travel," ;

    // Processes the movement command to determine how to move the character.
    public static void ProcessCommand(string command, int id)
    {
        string[] commands = command.Split(' ');

        if (simpleMove.Contains(","+commands[0]+","))
        {
            speed = "walk"; // For now, a simple move will assume the standard speed.
            if (commands[0].Equals("n") | commands[0].Equals("north")) { direction = "north"; }
            if (commands[0].Equals("ne") | commands[0].Equals("northeast")) { direction = "northeast"; }
            if (commands[0].Equals("e") | commands[0].Equals("east")) { direction = "east"; }
            if (commands[0].Equals("se") | commands[0].Equals("southeast")) { direction = "southeast"; }
            if (commands[0].Equals("s") | commands[0].Equals("south")) { direction = "south"; }
            if (commands[0].Equals("sw") | commands[0].Equals("southwest")) { direction = "southwest"; }
            if (commands[0].Equals("w") | commands[0].Equals("west")) { direction = "west"; }
            if (commands[0].Equals("nw") | commands[0].Equals("northwest")) { direction = "northwest"; }
        }

        if (complexMove.Contains(","+commands[0]+","))
        {

        }

        // Move the character the specified direction and speed.
        MoveCharacter(id, direction, speed);
    }
	
	// Move the character with the given direction and movement speed.
    static void MoveCharacter(int id, string direction, string speed)
    {
        string clientMessage = "";
        if (speed.Equals("walk")) { clientMessage = "You walk " + direction; } 
        ServerManager.SendToClient(Data.messageType.Standard, clientMessage, id);
    }
}
