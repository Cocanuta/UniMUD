using UnityEngine.Networking;

public class Data  {

    public static short Msg = MsgType.Highest + 1;
    
    public class Message : MessageBase
    {
        public string message;
    }
    	
}


