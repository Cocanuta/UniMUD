using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("UserDatabase")]
public class UserManager {

    [XmlArray("Users"), XmlArrayItem("User")]
    public List<User> Users = new List<User>();

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(UserManager));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, "UserDatabase.xml"), FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static UserManager Load()
    {
        var serializer = new XmlSerializer(typeof(UserManager));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, "UserDatabase.xml"), FileMode.Open))
        {
            return serializer.Deserialize(stream) as UserManager;
        }
    }


    public int GetNextID()
    {
        int id = 0;
        foreach(User u in ServerManager.userDatabase.Users)
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
        if(!UserExists(username))
        {
            ServerManager.userDatabase.Users.Add(new User { ID = GetNextID(), Name = username, Password = password, Email = email, Characters = new List<Character>() });
        }
    }

    public bool UserExists(int id)
    {
        foreach (User u in ServerManager.userDatabase.Users)
        {
            if (u.ID == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool UserExists(string Name)
    {
        foreach(User u in ServerManager.userDatabase.Users)
        {
            if(u.Name.ToLower() == Name.ToLower())
            {
                return true;
            }
        }
        return false;
    }

    public User GetUser(int id)
    {
        foreach(User u in ServerManager.userDatabase.Users)
        {
            if(u.ID == id)
            {
                return u;
            }
        }
        return null;
    }

    public User GetUser(string name)
    {
        foreach(User u in ServerManager.userDatabase.Users)
        {
            if(u.Name == name)
            {
                return u;
            }
        }
        return null;
    }

    public bool Login(string username, string password)
    {
        if(UserExists(username))
        {
            foreach(User u in ServerManager.userDatabase.Users)
            {
                if(u.Name == username && u.Password == password)
                {
                    Debug.Log("Logged In: " + u.ID.ToString() + ". " + u.Name);
                    return true;
                }
            }
        }
        return false;
    }
}

public class User
{
    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("Password")]
    public string Password { get; set; }

    [XmlAttribute("Email")]
    public string Email { get; set; }

    [XmlArray("Characters"), XmlArrayItem("Character")]
    public List<Character> Characters { get; set; }

}

