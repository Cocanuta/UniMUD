using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UserManager : MonoBehaviour {

    private DatabaseManager dbManager;

    public List<User> OnlineUsers = new List<User>(); //An active list of Online Users for quick access.

    void Awake()
    {
        dbManager = this.GetComponent<DatabaseManager>();
    }
    //Grab the next available ID in the users list.
    //This is isn't needed as the database auto-increments.
    public int GetNextID()
    {
        List<User> users = new List<User>(from u in dbManager.GetAllUsers() select u);
        int id = 0;
        foreach(User u in users)
        {
            if(u.ID >= id)
            {
                id = u.ID + 1;
            }
        }
        return id;
    }
	
	public void AddUser(string username, string password, string email)
    {
        User user = new User {ID = GetNextID(), Name = username, Password = password, Email = email};

        dbManager.AddUser(user);

    }

    public bool isLoggedIn(int id, bool clientId)
    {
        foreach(User u in OnlineUsers)
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

    public bool isLoggedIn(string username)
    {
        foreach (User u in OnlineUsers)
        {
            if (u.Name == username)
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
            List<User> users = new List<User>(from u in dbManager.GetAllUsers() where u.Name == username select u);
            if (users.Count == 1)
            {
                if (users[0].Password == password)
                {
                    error = "Success!";
                    User userToLogIn = users[0];
                    userToLogIn.ClientID = clientId;
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
        List<User> users = new List<User>(from u in dbManager.GetAllUsers() where u.ID == userId select u);
        User userToLogout = null;
        foreach(User u in OnlineUsers)
        {
            if(u.ID == userId)
            {
                userToLogout = u;
            }
        }
        if(userToLogout != null)
        {
            dbManager.UpdateUser(userToLogout);
            OnlineUsers.Remove(userToLogout);
        }
        
    }

    
}
