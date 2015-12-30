using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class UserManager {
	
	public static void AddUser(string username, string password, string email)
    {

        User user = new User { Name = username, Password = password, Email = email };

        ServerManager.dbManager.AddUser(user);

    }

    public static bool UsernameAvailable(string username)
    {
        List<User> users = ServerManager.dbManager.GetAllUsers();
        foreach(User u in users)
        {
            if(u.Name.ToLower() == username.ToLower())
            {
                return false;
            }
        }
        return true;
    }

    public static bool isLoggedIn(int id, bool clientId)
    {
        foreach(User u in ServerManager.OnlineUsers)
        {
            if(clientId && u.ClientID == id)
            {
                return true;
            }
            else if(clientId && u.ID == id)
            {
                return true;
            }
        }
        return false;
    }

    public static bool isLoggedIn(string username)
    {
        foreach (User u in ServerManager.OnlineUsers)
        {
            if (u.Name == username)
            {
                return true;
            }
        }
        return false;
    }

    public static bool Login(int clientId, string username, string password)
    {
        if (isLoggedIn(username))
        {
            return false;
        }
        else
        {
            List<User> users = new List<User>(from u in ServerManager.dbManager.GetAllUsers() where u.Name == username select u);
            if (users.Count == 1)
            {
                if (users[0].Password == password)
                {
                    User userToLogIn = users[0];
                    userToLogIn.ClientID = clientId;
                    ServerManager.OnlineUsers.Add(userToLogIn);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public static void Logout(int userId)
    {
        User userToLogout = null;
        foreach(User u in ServerManager.OnlineUsers)
        {
            if(u.ID == userId)
            {
                userToLogout = u;
            }
        }
        if(userToLogout != null)
        {
            ServerManager.dbManager.UpdateUser(userToLogout);
            ServerManager.OnlineUsers.Remove(userToLogout);
        }
        
    }

    public static User GetUser(int id)
    {
        List<User> users = ServerManager.dbManager.GetAllUsers();
        foreach(User u in users)
        {
            if(u.ID == id)
            {
                return u;
            }
        }
        return null;
    }

    public static User GetUser(string name)
    {
        List<User> users = ServerManager.dbManager.GetAllUsers();
        foreach(User u in users)
        {
            if(u.Name == name)
            {
                return u;
            }
        }
        return null;
    }

    public static int[] GetCharacters(User user)
    {
        int[] characters = user.Characters.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
        return characters;
    }

    
}
