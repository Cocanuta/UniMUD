using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : MonoBehaviour {

    public SimpleSQL.SimpleSQLManager dbManager;

    public List<Character> OnlineCharacters = new List<Character>();

    int GetNextID()
    {
        List<Character> characters = new List<Character>(from c in dbManager.Table<Character>() select c);
        int id = 0;
        foreach(Character c in characters)
        {
            if(c.CharacterID >= id)
            {
                id = c.CharacterID + 1;
            }
        }
        return id;
    }

    public Character GetCharacter(int id)
    {
        List<Character> characters = new List<Character>(from c in dbManager.Table<Character>() where c.CharacterID == id select c);
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
        Character character = new Character { CharacterID = GetNextID(), CharacterName = charName, CharacterHP = 100, CharacterLocation = 0 };

        dbManager.Insert(character);

        OnlineCharacters.Add(character);
    }

    public void DeleteCharacter(int charID)
    {
        Character charToDelete = GetCharacter(charID);
        foreach (Character c in OnlineCharacters)
        {
            if(c == charToDelete)
            {
                return;
            }
        }
        dbManager.Delete<Character>(charToDelete);
    }

}
