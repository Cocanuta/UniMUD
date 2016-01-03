using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

[XmlRoot("ItemDatabase")]
public class ItemManager {

    [XmlArray("Items"), XmlArrayItem("Item")]
    public List<Item> Items = new List<Item>();

    public enum itemType { Weapon, Consumable, Misc}

    public void Save()
    {
        var serializer = new XmlSerializer(typeof(ItemManager));
        using (StreamWriter stream = new StreamWriter(Path.Combine(Application.streamingAssetsPath, "ItemDatabase.xml"), false, Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static ItemManager Load()
    {
        var serializer = new XmlSerializer(typeof(ItemManager));
        using (StreamReader stream = new StreamReader(Path.Combine(Application.streamingAssetsPath, "ItemDatabase.xml"), Encoding.GetEncoding("UTF-8")))
        {
            return serializer.Deserialize(stream) as ItemManager;
        }
    }

    public int GetNextID()
    {
        int id = 0;
        foreach(Item i in Items)
        {
            if(i.ID >= id)
            {
                id = i.ID + 1;
            }
        }
        return id;
    }

    public void AddItemToDatabase(string name, string[] acceptedNames, string DescInv, string DescRoom, string ExamineInv, string ExamineRoom)
    {
        string names = "";
        foreach(string s in acceptedNames)
        {
            names += s + ",";
        }
        if(names[names.Length - 1] == ',')
        {
            names.Remove(names.Length - 1);
        }
        Items.Add(new Item { ID = GetNextID(), Name = name, AcceptedNames = names, DescriptionInv = DescInv, DescriptionRoom = DescRoom, ExamineInv = ExamineInv, ExamineRoom = ExamineRoom });
    }

    public Item GetItem(int id)
    {
        foreach(Item i in Items)
        {
            if(i.ID == id)
            {
                return i;
            }
        }
        return null;
    }





}

public class Item
{
    [XmlAttribute("ID")]
    public int ID { get; set; }

    [XmlAttribute("Name")]
    public string Name { get; set; }

    [XmlAttribute("AcceptedNames")]
    public string AcceptedNames { get; set; }

    [XmlAttribute("DescriptionInv")]
    public string DescriptionInv { get; set; }

    [XmlAttribute("DescriptionRoom")]
    public string DescriptionRoom { get; set; }

    [XmlAttribute("ExamineInv")]
    public string ExamineInv { get; set; }

    [XmlAttribute("ExamineRoom")]
    public string ExamineRoom { get; set; }

    [XmlAttribute("TakeItemText")]
    public string TakeItemText { get; set; }
}

public class Weapon : Item
{
    [XmlAttribute("WeildText")]
    public string WeildText { get; set; }

}

public class Consumable : Item
{
    [XmlAttribute("EatText")]
    public string EatText { get; set; }
}
