using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager {

    public Character GetCharacter(int id)
    {
        List<Character> characters = new List<Character>(from c in ServerManager.dbManager.GetAllCharacters() where c.ID == id select c);
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
        Character character = new Character { Name = charName, HP = 100, Location = 0 };

        ServerManager.dbManager.AddCharacter(character);

        User user = UserManager.GetUser(userID);

        if(user.Characters.Length == 0)
        {
            user.Characters += ServerManager.dbManager.GetCharacter(charName);
        }
        else
        {
            user.Characters += "," + ServerManager.dbManager.GetCharacter(charName);
        }

    }

}
    