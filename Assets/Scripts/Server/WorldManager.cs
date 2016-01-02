using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

[XmlRoot("WorldDatabase")]
public class WorldManager
{
    [XmlArray("Planets"), XmlArrayItem("Planet")]
    public List<Planet> Planets = new List<Planet>();

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(WorldManager));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, "WorldDatabase.xml"), FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static WorldManager Load()
    {
        var serializer = new XmlSerializer(typeof(WorldManager));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, "WorldDatabase.xml"), FileMode.Open))
        {
            return serializer.Deserialize(stream) as WorldManager;
        }
    }

    public int GetNextPlanetID()
    {
        int id = 0;
        foreach(Planet p in Planets)
        {
            if(p.ID >= id)
            {
                id = p.ID + 1;
            }
        }
        return id;
    }

    public int GetNextRoomID()
    {
        int id = 0;
        foreach(Planet p in Planets)
        {
            foreach(Room r in p.Rooms)
            {
                if(r.ID >= id)
                {
                    id = r.ID + 1;
                }
            }
        }
        return id;
    }

    public Planet GetPlanet(int id)
    {
        foreach(Planet p in Planets)
        {
            if(p.ID == id)
            {
                return p;
            }
        }
        return null;
    }

    public Room GetRoom(int id)
    {
        foreach(Planet p in Planets)
        {
            foreach(Room r in p.Rooms)
            {
                if(r.ID == id)
                {
                    return r;
                }
            }
        }
        return null;
    }

    public bool PlanetExists(int id)
    {
        foreach(Planet p in Planets)
        {
            if(p.ID == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool RoomExists(int id)
    {
        foreach(Planet p in Planets)
        {
            foreach(Room r in p.Rooms)
            {
                if(r.ID == id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AddPlanet(string name)
    {
        int id = GetNextPlanetID();
        Planets.Add(new Planet { ID = id, Name = name, Rooms = new List<Room>() });
        AddRoom(id, 0, 0, 0, "The Spawn Point of " + GetPlanet(id).Name + ".", "The Spawn Point of " + GetPlanet(id).Name + ".");
    }

    public void AddRoom(int planetID, int x, int y, int z, string shortDesc, string longDesc)
    {
        if(PlanetExists(planetID))
        {
            foreach(Planet p in Planets)
            {
                if(p.ID == planetID)
                {
                    foreach(Room r in p.Rooms)
                    {
                        if(r.PosX == x && r.PosY == y && r.PosZ == z)
                        {
                            return;
                        }
                    }
                    p.Rooms.Add(new Room { ID = GetNextRoomID(), PosX = x, PosY = y, PosZ = z, shortDescription = shortDesc, longDescription = longDesc });
                }
            }
        }
    }

}

public class Planet
{
    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlArray("Rooms"), XmlArrayItem("Room")]
    public List<Room> Rooms { get; set; }
}

public class Room
{
    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("PosX")]
    public int PosX { get; set; }

    [XmlAttribute("PosY")]
    public int PosY { get; set; }

    [XmlAttribute("PosZ")]
    public int PosZ { get; set; }

    [XmlAttribute("ShortDescription")]
    public string shortDescription { get; set; }

    [XmlAttribute("LongDescription")]
    public string longDescription { get; set; }
}
