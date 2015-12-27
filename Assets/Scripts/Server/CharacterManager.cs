using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : MonoBehaviour {

    private DatabaseManager dbManager;

    public List<Character> OnlineCharacters = new List<Character>();

    void Awake()
    {
        dbManager = this.GetComponent<DatabaseManager>();
    }

    int GetNextID()
    {
        List<Character> characters = new List<Character>(from c in dbManager.GetAllCharacters() select c);
        int id = 0;
        foreach(Character c in characters)
        {
            if(c.ID >= id)
            {
                id = c.ID + 1;
            }
        }
        return id;
    }

    public Character GetCharacter(int id)
    {
        List<Character> characters = new List<Character>(from c in dbManager.GetAllCharacters() where c.ID == id select c);
        if(characters.Count == 1)
        {
            return characters[0];
        }
        else
        {
            return null;
        }

    }

    public void AddCharacter(int userID, string charName)
    {
        Character character = new Character { ID = GetNextID(), Name = charName, HP = 100, Location = 0 };

        dbManager.AddCharacter(character);

        OnlineCharacters.Add(character);
    }

}
