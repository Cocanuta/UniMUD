using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UserManager : MonoBehaviour {

    public SimpleSQL.SimpleSQLManager dbManager; //The Database Manager.

    public List<User> OnlineUsers = new List<User>(); //An active list of Online Users for quick access.

    //Grab the next available ID in the users list.
    //This is isn't needed as the database auto-increments.
    public int GetNextID()
    {
        List<User> users = new List<User>(from u in dbManager.Table<User>() select u);
        int id = 0;
        foreach(User u in users)
        {
            if(u.UserID >= id)
            {
                id = u.UserID + 1;
            }
        }
        return id;
    }
	
	public void AddUser(string username, string password, string email)
    {
        User user = new User {UserID = GetNextID(), UserName = username, UserPassword = password, UserEMail = email};

        dbManager.Insert(user);

    }

    public bool isLoggedIn(int id, bool clientId)
    {
        foreach(User u in OnlineUsers)
        {
            if(clientId && u.UserClientID == id)
            {
                return true;
            }
            else if(clientId && u.UserID == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool isLoggedIn(string username)
    {
        foreach (User u in OnlineUsers)
        {
            if (u.UserName == username)
            {
                return true;
            }
        }
        return false;
    }

    public bool Login(int clientId, string username, string password, out string error)
    {
        if (isLoggedIn(username))
        {
            error = "User already logged in currently.";
            return false;
        }
        else
        {
            List<User> users = new List<User>(from u in dbManager.Table<User>() where u.UserName == username select u);
            if (users.Count == 1)
            {
                if (users[0].UserPassword == password)
                {
                    error = "Success!";
                    User userToLogIn = users[0];
                    userToLogIn.UserClientID = clientId;
                    OnlineUsers.Add(userToLogIn);
                    return true;
                }
                else
                {
                    error = "Wrong password.";
                    return false;
                }
            }
            else
            {
                error = "Username not found.";
                return false;
            }
        }
    }

    public void Logout(int userId)
    {
        List<User> users = new List<User>(from u in dbManager.Table<User>() where u.UserID == userId select u);
        User userToLogout = null;
        foreach(User u in OnlineUsers)
        {
            if(u.UserID == userId)
            {
                userToLogout = u;
            }
        }
        if(userToLogout != null)
        {
            dbManager.UpdateTable(userToLogout);
            OnlineUsers.Remove(userToLogout);
        }
        
    }
    /*
    public int[] GetCharacters(int UserID)
    {
        List<User> users = new List<User>(from u in dbManager.Table<User>() where u.UserID == UserID select u);
        int[] characterIDs = users[0].UserCharacters.Split(',').Select(n => System.Convert.ToInt32(n)).ToArray();
        return characterIDs;
    }
    */

    
}
