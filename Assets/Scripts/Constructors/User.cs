using SimpleSQL;
using System.Collections.Generic;

public class User {

    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }

    [Indexed, MaxLength(60), NotNull]
    public string UserName { get; set; }

    [NotNull, MaxLength(100)]
    public string UserPassword { get; set; }

    [NotNull, MaxLength(100)]
    public string UserEMail { get; set; }

    public string UserCharacters { get; set; }

    public int UserClientID;
}
