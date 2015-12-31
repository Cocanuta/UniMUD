using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room {

    public int id { get; set; }
    public int planetId { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public int PosZ { get; set; }
    public string shortDescription { get; set; }
    public string longDescription { get; set; }
    public List<RoomItem> contents { get; set; }
}
