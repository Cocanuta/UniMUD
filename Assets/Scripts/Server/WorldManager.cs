using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Text;

[XmlRoot("WorldDatabase")]
public class WorldManager
{
    [XmlArray("Planets"), XmlArrayItem("Planet")]
    public List<Planet> Planets = new List<Planet>();

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(WorldManager));
        using (StreamWriter stream = new StreamWriter(Path.Combine(Application.streamingAssetsPath, "WorldDatabase.xml"), false, Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static WorldManager Load()
    {
        var serializer = new XmlSerializer(typeof(WorldManager));
        using (StreamReader stream = new StreamReader(Path.Combine(Application.streamingAssetsPath, "WorldDatabase.xml"), Encoding.GetEncoding("UTF-8")))
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
                    p.Rooms.Add(new Room { ID = GetNextRoomID(), PosX = x, PosY = y, PosZ = z, shortDescription = shortDesc, longDescription = longDesc , RoomItems = new List<RoomItem>()});
                }
            }
        }
    }

    public string ConvertToString(string[] arrayString)
    {
        string s = "";
        foreach(string st in arrayString)
        {
            s += st + ",";
        }
        if(s[s.Length - 1] == ',')
        {
            s.Remove(s.Length - 1);
        }
        return s;
    }

    public void AddRoomDecoration(int roomId, string name, string shortDescription, string[] acceptedNames, string longDescription)
    {
        foreach(Planet p in Planets)
        {
            foreach(Room r in p.Rooms)
            {
                if(r.ID == roomId)
                {
                    r.RoomItems.Add(new RoomItem_Decoration { Name = name, ShortDescription = shortDescription, LongDescription = longDescription, AcceptedNames = ConvertToString(acceptedNames) });
                }
            }
        }
    }

    public void AddRoomMovable(int roomId, string name, string shortDescription, string[] acceptedNames, string longDescription, RoomItem concealedItem)
    {
        foreach (Planet p in Planets)
        {
            foreach (Room r in p.Rooms)
            {
                if (r.ID == roomId)
                {
                    r.RoomItems.Add(new RoomItem_Movable { Name = name, ShortDescription = shortDescription, LongDescription = longDescription, AcceptedNames = ConvertToString(acceptedNames), Concealing = concealedItem });
                }
            }
        }
    }

    public void AddRoomContainer(int roomId, string name, string shortDescription, string[] acceptedNames, string longDescription, List<Item> items, int capacity)
    {
        foreach (Planet p in Planets)
        {
            foreach (Room r in p.Rooms)
            {
                if (r.ID == roomId)
                {
                    r.RoomItems.Add(new RoomItem_Container { Name = name, ShortDescription = shortDescription, LongDescription = longDescription, AcceptedNames = ConvertToString(acceptedNames), Capacity = capacity, Contents = items});
                }
            }
        }
    }

    public void AddRoomDoor(int roomId, string name, string shortDescription, string[] acceptedNames, RoomItem_Door.direction direction, RoomItem_Door.status status, int destinationId)
    {
        foreach (Planet p in Planets)
        {
            foreach (Room r in p.Rooms)
            {
                if (r.ID == roomId)
                {
                    r.RoomItems.Add(new RoomItem_Door { Name = name, ShortDescription = shortDescription, AcceptedNames = ConvertToString(acceptedNames), Direction = direction, Destination = destinationId, Status = status });
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

    [XmlArray("RoomItems"), XmlArrayItem("RoomItem")]
    public List<RoomItem> RoomItems { get; set; }

}

[XmlInclude(typeof(RoomItem_Door)), XmlInclude(typeof(RoomItem_Container)), XmlInclude(typeof(RoomItem_Movable)), XmlInclude(typeof(RoomItem_Door))]
public class RoomItem
{
    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("ShortDescription")]
    public string ShortDescription { get; set; }

    [XmlAttribute("AcceptedNames")]
    public string AcceptedNames { get; set; }
}

public class RoomItem_Decoration : RoomItem
{
    [XmlAttribute("LongDescription")]
    public string LongDescription { get; set; }
}

public class RoomItem_Movable : RoomItem
{
    [XmlAttribute("LongDescription")]
    public string LongDescription { get; set; }

    [XmlAttribute("Concealing")]
    public RoomItem Concealing { get; set; }
}

public class RoomItem_Container : RoomItem
{
    [XmlArray("Contents"), XmlArrayItem("Item")]
    public List<Item> Contents { get; set; }

    [XmlAttribute("LongDescription")]
    public string LongDescription { get; set; }

    [XmlAttribute("Capacity")]
    public int Capacity { get; set; }
}

public class RoomItem_Door : RoomItem
{
    public enum direction { north, east, south, west, northeast, northwest, southeast, southwest, up, down};
    public enum status { open, closed, locked };

    [XmlAttribute("Direction")]
    public direction Direction { get; set; }

    [XmlAttribute("Status")]
    public status Status { get; set; }

    [XmlAttribute("Destination")]
    public int Destination { get; set; }
}
