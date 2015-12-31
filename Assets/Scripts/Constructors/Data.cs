using UnityEngine.Networking;

public class Data  {

    public static short Msg = MsgType.Highest + 1;
    public enum messageType { Standard,Error };
    
    public class Message : MessageBase
    {
        public string message;
        public messageType type;
    }
    	
}


