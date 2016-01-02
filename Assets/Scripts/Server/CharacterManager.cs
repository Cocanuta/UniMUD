using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class CharacterManager {

    public List<Character> OnlineCharacters;

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
                ServerManager.userDatabase.GetUser(userID).Characters.Add(new Character { ID = GetNextID(), Name = name, HP = 100, Location = 0 });
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
        foreach(Character c in OnlineCharacters)
        {
            if(c.ID == character.ID)
            {
                return true;
            }
        }
        return false;
    }

    public void Login(Character character)
    {
        if(IsOnline(character))
        {
            foreach(User u in ServerManager.userDatabase.Users)
            {
                foreach(Character c in u.Characters)
                {
                    if(c.ID == character.ID)
                    {
                        c.Name = character.Name;
                        c.HP = character.HP;
                        c.Location = c.Location;
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

    [XmlIgnore]
    public int ClientID { get; set; }

}
