using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class CharacterManager {

    public static int GetNextID()
    {
        int id = 0;
        foreach(User u in ServerManager.userDatabase.Users)
        {
            foreach(Character c in u.Characters)
            {
                if(c.ID >= id)
                {
                    id = c.ID + 1;
                }
            }
        }
        return id;
    }

    public static void AddCharacter(int userID, string name)
    {
        if (!CharacterExists(name))
        {
            if (ServerManager.userDatabase.UserExists(userID))
            {
                ServerManager.userDatabase.GetUser(userID).Characters.Add(new Character { ID = GetNextID(), Name = name, HP = 100, Location = 0, Inventory = new List<Item>() });
            }
        }
    }

    public static bool CharacterExists(string name)
    {
        foreach(User u in ServerManager.userDatabase.Users)
        {
            foreach(Character c in u.Characters)
            {
                if(c.Name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Character GetCharacter(int id)
    {
        foreach (User u in ServerManager.userDatabase.Users)
        {
            foreach (Character c in u.Characters)
            {
                if (c.ID == id)
                {
                    return c;
                }
            }
        }
        return null;
    }

    public Character GetCharacter(string name)
    {
        foreach (User u in ServerManager.userDatabase.Users)
        {
            foreach (Character c in u.Characters)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
        }
        return null;
    }

    public bool IsOnline(Character character)
    {
        foreach(Character c in ServerManager.userDatabase.OnlineCharacters)
        {
            if(c.ID == character.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void Logout(int clientID)
    {
        foreach(Character oc in ServerManager.userDatabase.OnlineCharacters)
        {
            if(oc.ClientID == clientID)
            {
                foreach(User u in ServerManager.userDatabase.Users)
                {
                    foreach(Character c in u.Characters)
                    {
                        if(c.ID == oc.ID)
                        {
                            c.Name = oc.Name;
                            c.HP = oc.HP;
                            c.Location = oc.Location;
                            c.Inventory = oc.Inventory;
                        }
                    }
                }
            }
        }
    }
}

public class Character
{
    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("HP")]
    public float HP { get; set; }

    [XmlAttribute("Location")]
    public int Location { get; set; }

    [XmlArray("Inventory"), XmlArrayItem("Item")]
    public List<Item> Inventory { get; set; }

    [XmlIgnore]
    public int? ClientID { get; set; }

}
