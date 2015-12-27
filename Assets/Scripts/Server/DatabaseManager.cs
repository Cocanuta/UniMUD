using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

public class DatabaseManager : MonoBehaviour
{

    public string host, database, user, password; //MySQL Database information
    public bool pooling = true; //Enable or disable MySQL pooling

    private string connectionString; //String to hold the connection information
    public MySqlConnection con = null; //Holds the connection
    public MySqlCommand cmd = null; //Holds the commands passed to the MySQL server
    public MySqlDataReader rdr = null; //Used to read the MySQL data retreived

    // Use this for initialization
    void Awake()
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

    //When the application quits, close the connection
    void OnApplicationQuit()
    {
        Debug.Log("Closing Connection");
        if (con != null)
        {
            if (con.State != ConnectionState.Closed)
                con.Close();
            con.Dispose();
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
            query = "INSERT INTO Characters(ID, Name, HP, Location) VALUES (?ID, ?Name, ?HP, ?Location)";

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
}
