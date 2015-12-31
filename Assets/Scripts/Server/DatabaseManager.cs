using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

public class DatabaseManager
{
    private string connectionString; //String to hold the connection information
    public MySqlConnection con = null; //Holds the connection
    public MySqlCommand cmd = null; //Holds the commands passed to the MySQL server
    public MySqlDataReader rdr = null; //Used to read the MySQL data retreived

    // Use this for initialization
    public DatabaseManager(string host, string database, string user, string password, bool pooling)
    {

        //construt the connection string
        connectionString = "Server=" + host + ";Database=" + database + ";UiD=" + user + ";Pwd=" + password + ";Pooling=";
        //Toggle pooling
        if (pooling)
        {
            connectionString += "true;";
        }
        else
        {
            connectionString += "false;";
        }

        //Open a connection to the database
        try
        {
            con = new MySqlConnection(connectionString);
            con.Open();
            Debug.Log("MySQL State: " + con.State);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

    }

    //A method to update all users from a list.
    public void UpdateAllUsers(List<User> users)
    {
        string query = string.Empty;
        try
        {
            query = "UPDATE Users SET Name=?Name, Password=?Password, Email=?Email, Characters=?Characters WHERE ID=?ID";
            if (con.State != ConnectionState.Open)
                con.Open();
            using (con)
            {
                foreach (User u in users)
                {
                    using (cmd = new MySqlCommand(query, con))
                    {
                        MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                        oParam.Value = u.Name;
                        MySqlParameter oParam1 = cmd.Parameters.Add("?Password", MySqlDbType.VarChar);
                        oParam1.Value = u.Password;
                        MySqlParameter oParam2 = cmd.Parameters.Add("?Email", MySqlDbType.VarChar);
                        oParam2.Value = u.Email;
                        MySqlParameter oParam3 = cmd.Parameters.Add("?Characters", MySqlDbType.VarChar);
                        oParam3.Value = u.Characters;
                        MySqlParameter oParam4 = cmd.Parameters.Add("?ID", MySqlDbType.Int64);
                        oParam4.Value = u.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void UpdateUser(User u)
    {
        string query = string.Empty;
        try
        {
            query = "UPDATE Users SET Name=?Name, Password=?Password, Email=?Email, Characters=?Characters WHERE ID=?ID";
            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam.Value = u.Name;
                    MySqlParameter oParam1 = cmd.Parameters.Add("?Password", MySqlDbType.VarChar);
                    oParam1.Value = u.Password;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Email", MySqlDbType.VarChar);
                    oParam2.Value = u.Email;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?Characters", MySqlDbType.VarChar);
                    oParam3.Value = u.Characters;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?ID", MySqlDbType.Int64);
                    oParam4.Value = u.ID;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void AddUser(User u)
    {
        string query = string.Empty;
        try
        {
            query = "INSERT INTO Users(Name, Password, Email, Characters) VALUES (?Name, ?Password, ?Email, ?Characters)";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam1 = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam1.Value = u.Name;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Password", MySqlDbType.VarChar);
                    oParam2.Value = u.Password;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?Email", MySqlDbType.VarChar);
                    oParam3.Value = u.Email;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?Characters", MySqlDbType.VarChar);
                    oParam4.Value = u.Characters;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public List<User> GetAllUsers()
    {
        string query = string.Empty;
        List<User> users = new List<User>();
        try
        {
            query = "SELECT * FROM Users";
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                        while (rdr.Read())
                        {
                            User u = new User();
                            u.ID = int.Parse(rdr["ID"].ToString());
                            u.Name = rdr["Name"].ToString();
                            u.Password = rdr["Password"].ToString();
                            u.Email = rdr["Email"].ToString();
                            u.Characters = rdr["Characters"].ToString();
                            users.Add(u);
                        }
                    rdr.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        return users;
    }

    public User GetUser(int id)
    {
        string query = "SELECT * FROM Users WHERE ID = " + id.ToString();
        User u = new User();
        try
        {
            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if(rdr.HasRows)
                        while(rdr.Read())
                        {
                            u.ID = int.Parse(rdr["ID"].ToString());
                            u.Name = rdr["Name"].ToString();
                            u.Password = rdr["Password"].ToString();
                            u.Email = rdr["Email"].ToString();
                            u.Characters = rdr["Characters"].ToString();
                        }
                    rdr.Dispose();
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        return u;

    }

    public User GetUser(string name)
    {
        string query = "SELECT * FROM Users WHERE Name = " + name;
        User u = new User();
        try
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                        while (rdr.Read())
                        {
                            u.ID = int.Parse(rdr["ID"].ToString());
                            u.Name = rdr["Name"].ToString();
                            u.Password = rdr["Password"].ToString();
                            u.Email = rdr["Email"].ToString();
                            u.Characters = rdr["Characters"].ToString();
                        }
                    rdr.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        return u;

    }

    public void UpdateAllCharacters(List<Character> characters)
    {
        string query = string.Empty;
        try
        {
            query = "UPDATE Characters SET Name=?Name, HP=?HP, Location=?Location WHERE ID=?ID";
            if (con.State != ConnectionState.Open)
                con.Open();
            using (con)
            {
                foreach (Character c in characters)
                {
                    using (cmd = new MySqlCommand(query, con))
                    {
                        MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                        oParam.Value = c.Name;
                        MySqlParameter oParam1 = cmd.Parameters.Add("?HP", MySqlDbType.Float);
                        oParam1.Value = c.HP;
                        MySqlParameter oParam2 = cmd.Parameters.Add("?Location", MySqlDbType.VarChar);
                        oParam2.Value = c.Location;
                        MySqlParameter oParam3 = cmd.Parameters.Add("?ID", MySqlDbType.Int64);
                        oParam3.Value = c.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void UpdateCharacter(Character c)
    {
        string query = string.Empty;
        try
        {
            query = "UPDATE Characters SET Name=?Name, HP=?HP, Location=?Location WHERE ID=?ID";
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam.Value = c.Name;
                    MySqlParameter oParam1 = cmd.Parameters.Add("?HP", MySqlDbType.Float);
                    oParam1.Value = c.HP;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Location", MySqlDbType.VarChar);
                    oParam2.Value = c.Location;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?ID", MySqlDbType.Int64);
                    oParam3.Value = c.ID;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void AddCharacter(Character c)
    {
        string query = string.Empty;
        try
        {
            query = "INSERT INTO Characters(Name, HP, Location) VALUES (?Name, ?HP, ?Location)";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam.Value = c.Name;
                    MySqlParameter oParam1 = cmd.Parameters.Add("?HP", MySqlDbType.Float);
                    oParam1.Value = c.HP;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Location", MySqlDbType.VarChar);
                    oParam2.Value = c.Location;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public List<Character> GetAllCharacters()
    {
        string query = string.Empty;
        List<Character> characters = new List<Character>();
        try
        {
            query = "SELECT * FROM Characters";
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                        while (rdr.Read())
                        {
                            Character c = new Character();
                            c.ID = int.Parse(rdr["ID"].ToString());
                            c.Name = rdr["Name"].ToString();
                            c.HP = float.Parse(rdr["HP"].ToString());
                            c.Location = int.Parse(rdr["Location"].ToString());
                            characters.Add(c);
                        }
                    rdr.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        return characters;
    }

    public Character GetCharacter(int id)
    {
        string query = "SELECT * FROM Characters WHERE ID = " + id.ToString();
        Character c = new Character();
        try
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                        while (rdr.Read())
                        {
                            c.ID = int.Parse(rdr["ID"].ToString());
                            c.Name = rdr["Name"].ToString();
                            c.HP = float.Parse(rdr["HP"].ToString());
                            c.Location = int.Parse(rdr["Location"].ToString());
                        }
                    rdr.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        return c;

    }

    public Character GetCharacter(string name)
    {
        string query = "SELECT * FROM Characters WHERE Name = " + name;
        Character c = new Character();
        try
        {
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                        while (rdr.Read())
                        {
                            c.ID = int.Parse(rdr["ID"].ToString());
                            c.Name = rdr["Name"].ToString();
                            c.HP = float.Parse(rdr["HP"].ToString());
                            c.Location = int.Parse(rdr["Location"].ToString());
                        }
                    rdr.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }

        return c;

    }

    public void AddPlanet(string name, string description)
    {
        string query = string.Empty;

        try
        {
            query = "INSERT INTO Planets(Name, Description) VALUES (?Name, ?Description)";

            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam.Value = name;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Description", MySqlDbType.VarChar);
                    oParam2.Value = description;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void UpdatePlanet(Planet p)
    {
        string query = string.Empty;
        try
        {
            query = "UPDATE Planets SET Name=?Name, Description=?Description WHERE ID=?ID";
            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?Name", MySqlDbType.VarChar);
                    oParam.Value = p.name;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?Description", MySqlDbType.VarChar);
                    oParam2.Value = p.description;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam3.Value = p.id;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public Planet GetPlanet(int id)
    {
        string query = string.Empty;
        Planet p = new Planet();
        try
        {
            query = "SELECT * FROM Planets WHERE ID = " + id.ToString();

            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if(rdr.HasRows)
                    {
                        while(rdr.Read())
                        {
                            p.id = int.Parse(rdr["ID"].ToString());
                            p.name = rdr["Name"].ToString();
                            p.description = rdr["Description"].ToString();
                            //Add Rooms here

                        }
                        rdr.Dispose();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        return p;
    }

    public void AddRoom(int planetId, string shortDescription, string longDescription, int posX, int posY, int posZ, List<RoomItem> contents)
    {
        string query = string.Empty;

        try
        {
            query = "INSERT INTO Rooms(PlanetID, ShortDescription, LongDescription, PosX, PosY, PosZ, Contents) VALUES (?PlanetID, ?ShortDescription, ?LongDescription, ?PosX, ?PosY, ?PosZ, ?Contents)";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?PlanetID", MySqlDbType.Int32);
                    oParam.Value = planetId;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?ShortDescription", MySqlDbType.VarChar);
                    oParam2.Value = shortDescription;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?LongDescription", MySqlDbType.VarChar);
                    oParam3.Value = longDescription;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?PosX", MySqlDbType.Int32);
                    oParam4.Value = posX;
                    MySqlParameter oParam5 = cmd.Parameters.Add("?PosY", MySqlDbType.Int32);
                    oParam5.Value = posY;
                    MySqlParameter oParam6 = cmd.Parameters.Add("?PosZ", MySqlDbType.Int32);
                    oParam6.Value = posZ;
                    MySqlParameter oParam7 = cmd.Parameters.Add("?Contents", MySqlDbType.VarChar);
                    string roomItems = "";
                    foreach(RoomItem ri in contents)
                    {
                        roomItems += ri.id;
                        roomItems += ",";
                    }
                    roomItems.Remove(roomItems.Length - 1);
                    oParam7.Value = roomItems;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void UpdateRoom(Room r)
    {
        string query = string.Empty;
        try
        {
            query = "UPDATE Rooms SET PlanetID=?PlanetID, ShortDescription=?ShortDescription, LongDescription=?LongDescription, PosX=?PosX, PosY=?PosY, PosZ=?PosZ, Contents=?Contents WHERE ID=?ID";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?PlanetID", MySqlDbType.Int32);
                    oParam.Value = r.planetId;
                    MySqlParameter oParam2 = cmd.Parameters.Add("?ShortDescription", MySqlDbType.VarChar);
                    oParam2.Value = r.shortDescription;
                    MySqlParameter oParam3 = cmd.Parameters.Add("?LongDescription", MySqlDbType.VarChar);
                    oParam3.Value = r.longDescription;
                    MySqlParameter oParam4 = cmd.Parameters.Add("?PosX", MySqlDbType.Int32);
                    oParam4.Value = r.PosX;
                    MySqlParameter oParam5 = cmd.Parameters.Add("?PosY", MySqlDbType.Int32);
                    oParam5.Value = r.PosY;
                    MySqlParameter oParam6 = cmd.Parameters.Add("?PosZ", MySqlDbType.Int32);
                    oParam6.Value = r.PosZ;
                    MySqlParameter oParam7 = cmd.Parameters.Add("?Contents", MySqlDbType.VarChar);
                    string roomItems = "";
                    foreach(RoomItem ri in r.contents)
                    {
                        roomItems += ri.id;
                        roomItems += ",";
                    }
                    roomItems.Remove(roomItems.Length - 1);
                    oParam7.Value = roomItems;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public Room GetRoom(int id)
    {
        string query = string.Empty;
        Room r = new Room();
        try
        {
            query = "SELECT * FROM Rooms WHERE ID = " + id.ToString();

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            r.id = int.Parse(rdr["ID"].ToString());
                            r.shortDescription = rdr["ShortDescription"].ToString();
                            r.longDescription = rdr["LongDescription"].ToString();
                            r.planetId = int.Parse(rdr["PlanetID"].ToString());
                            r.PosX = int.Parse(rdr["PosX"].ToString());
                            r.PosY = int.Parse(rdr["PosY"].ToString());
                            r.PosZ = int.Parse(rdr["PosZ"].ToString());
                            string[] roomItems = rdr["Contents"].ToString().Split(',');
                            foreach(String s in roomItems)
                            {
                                r.contents.Add(GetRoomItem(int.Parse(s)));
                            }                           

                        }
                        rdr.Dispose();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        return r;
    }

    public void AddRoomItem(int id)
    {
        string query = string.Empty;

        try
        {
            query = "INSERT INTO RoomItems(ID) VALUES (?ID)";

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    MySqlParameter oParam = cmd.Parameters.Add("?ID", MySqlDbType.Int32);
                    oParam.Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public RoomItem GetRoomItem(int id)
    {
        string query = string.Empty;
        RoomItem ri = new RoomItem();
        try
        {
            query = "SELECT * FROM RoomItems WHERE ID = " + id.ToString();

            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            using (con)
            {
                using (cmd = new MySqlCommand(query, con))
                {
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            ri.id = int.Parse(rdr["ID"].ToString());
                        }
                        rdr.Dispose();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        return ri;
    }





}
