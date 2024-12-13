using System.Collections.Generic;
using Newtonsoft.Json;

public class WsMessage
{
    //[Serialize]
    public string type;
}

public class AuthResponse : WsMessage
{
    //[Serialize]
    public string access_token;

    public AuthResponse(string token) : base()
    {
        type = "auth";
        access_token = token;
    }
}

public class SerialResponse : WsMessage
{
    //[Serialize]
    public int id;

    //[Serialize]
    public bool success;

    //[Serialize]
    public object result;

    public SerialMessage request
    {
        get => SerialMessage.sent[this.id];
    }
}

public class SerialMessage : WsMessage
{
    //public static Dictionary<int, SerialResponse> response_queue = new();
    public static Dictionary<int, SerialMessage> sent = new();
    private static int counter = 0;

    //[Serialize]
    public int id;

    public SerialMessage() : base()
    {
        //counter++;
        id = counter;
    }

    public static void increaseCounter() {
        counter++;
    }
}

public class SubscribeEvent : SerialMessage
{
    //[Serialize]
    public string event_type;

    public SubscribeEvent(string event_name) : base()
    {
        type = "subscribe_events";
        event_type = event_name;
    }
}

public class StatesRequest : SerialMessage
{
    public StatesRequest() : base()
    {
        type = "get_states";
    }
}

//[System.Serializable]
public class ResultMessage : SerialResponse
{
    public State[] result;
    /*     public T castInner<T>()
        {
            return (T)eventData;
        } */
}

public class EventMessage : SerialResponse
{
    [JsonProperty("event")]
    public EventMessagePayload eventData;
    
}

public class EventMessageData
{
    public string entity_id;
    public State new_state;
    //public object old_state;

    public string event_type;
    public object context;
}

public class EventMessagePayload
{
    [JsonProperty("event_type")]
    public string eventType;
    public EventMessageData data;
}

public class State
{
    public string entity_id;
    public string state;
    public string last_changed;
    public string last_reported;
    public string last_updated;
    public object context;
    public object attributes;
}