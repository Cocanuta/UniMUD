//-----
// Handles all movement commands sent from the client, moves the character as appropriate, and updates the client.
//-----

using UnityEngine;
using System.Collections;

public class Movement {

    public static string direction;   // The direction to travel in.
    public static string speed;       // The speed to travel.

    // The command will either be simple or complex, either consisting of just a direction or a direction with additional instruction.
    private static string simpleMove = ",n,ne,e,se,s,sw,w,nw,north,northeast,east,southeast,south,southwest,west,northwest,u,d,up,down,en,ex,enter,exit,";
    private static string complexMove = ",move,walk,run,sneak,travel," ;

    // Processes the movement command to determine how to move the character.
    public static void ProcessCommand(string command, int id)
    {
        string[] commands = command.Split(' ');

        if (simpleMove.Contains(","+commands[0]+","))
        {
            speed = "walk"; // For now, a simple move will assume the standard speed.
            switch (commands[0])
            {
                case "n": case "north": direction = "north"; break;
                case "ne": case "northeast": direction = "northeast"; break;
                case "e": case "east": direction = "east"; break;
                case "se": case "southest": direction = "southeast"; break;
                case "s": case "south": direction = "south"; break;
                case "sw": case "southwest": direction = "southwest"; break;
                case "w": case "west": direction = "west"; break;
                case "nw": case "northwest": direction = "northwest"; break;
                case "u": case "up": direction = "up"; break;
                case "d": case "down": direction = "down"; break;
                case "en": case "enter": direction = "enter"; break;
                case "ex": case "exit": direction = "exit"; break;
            }
        }

        if (complexMove.Contains(","+commands[0]+","))
        {
            switch (commands[0])
            {
                case "move": case "walk": speed = "walk"; break;
                case "run": speed = "run"; break;
                case "sneak": speed = "sneak"; break;
                case "travel": speed = "travel"; break;
                default: break;
            }
        }

        // Move the character the specified direction and speed.
        MoveCharacter(id, direction, speed);
    }
	
	// Move the character with the given direction and movement speed.
    static void MoveCharacter(int id, string direction, string speed)
    {
        string clientMessage = "";
        if (speed.Equals("walk")) { clientMessage = "You walk " + direction; } 
        MessageManager.SendToClient(MessageManager.messageType.Standard, clientMessage, id);
    }
}
