using SimpleSQL;
using System.Collections.Generic;

public class Character {

    [PrimaryKey, AutoIncrement]
    public int CharacterID { get; set; }

    [Indexed, MaxLength(60), NotNull]
    public string CharacterName { get; set; }

    [NotNull]
    public float CharacterHP { get; set; }

    [NotNull]
    public int CharacterLocation { get; set; }

}
