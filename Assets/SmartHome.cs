using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using UnityEngine;
using Newtonsoft.Json;

public class SmartHome : MonoBehaviour
{
    public string endpoint = "ws://assistant.apps.cyber.psych0si.is/api/websocket";

    public string access_token = "ABCDEFGHIJKLMNOPQ";
    private bool lastState = false;
    private static WebSocket websocket;
    private bool authenticated = false;
    public Dictionary<string, GameObject> things = new Dictionary<string, GameObject>();

    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SmartHomeConsumer>() != null)
            {
                things[child.name] = child.gameObject;
            }
        }

        Task.Run(SetupWebsocket).Wait();
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    private async void SetupWebsocket()
    {
        websocket = new WebSocket(endpoint);

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
            authenticated = false;
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
            authenticated = false;
        };

        websocket.OnMessage += (bytes) =>
        {
            HandleWebsocketMessage(bytes);
        };

        await websocket.Connect();
    }

    private SmartHomeConsumer GetConsumerByName(string name)
    {
        try
        {
            //lights[name].SetActive(state);
            var thing = things[name];
            return thing.GetComponent<SmartHomeConsumer>();
            //Debug.Log($"Setting {name} to {state}");
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }

    void HandleWebsocketMessage(byte[] bytes)
    {
        char[] msg_bytes = Encoding.UTF8.GetChars(bytes);
        var msg = new string(msg_bytes);
        WsMessage m = JsonConvert.DeserializeObject<WsMessage>(msg);
        Debug.Log(msg);

        switch (m.type)
        {
            case "auth_required":
                Debug.Log(m.type);
                AuthResponse resp = new AuthResponse(access_token);

                SendWebSocketMessage(resp).Wait();
                break;

            case "auth_ok":
                authenticated = true;
                Debug.Log("Authentication Successful!");
                GetInitialState().Wait();
                Subscribe().Wait();
                break;

            case "auth_invalid":
                Debug.Log("Authentication failed!");
                break;

            case "result":
                ResultMessage sr = JsonConvert.DeserializeObject<ResultMessage>(msg);
                HandleResult(sr);
                break;

            case "event":
                EventMessage ev = JsonConvert.DeserializeObject<EventMessage>(msg);
                HandleEvent(ev);
                break;
            default:
                Debug.Log("default case");
                break;

        }
    }

    private void HandleEvent(EventMessage msg)
    {
        Debug.Log($"Handling event of type {msg.eventData.eventType}");
        var pl = msg.eventData;
        var consumer = GetConsumerByName(pl.data.entity_id);
        if (consumer != null) {
            consumer.UpdateFromState(pl.data.new_state);
        }
        //SetSingleLightState(pl.data.entity_id, pl.data.new_state.state == "on");
    }

    private void HandleResult(ResultMessage ev)
    {
        Debug.Log($"Handling result of type {ev.request.type}");
        //ev.eventData.
        switch (ev.request.type)
        {
            case "get_states":
                var data = ev.result;
                //SerialMessage.response_queue[ev.id] = sr;
                foreach (var d in data)
                {
                    //var e = d;
                    //SetSingleLightState(e.entity_id, e.state == "on");
                    var consumer = GetConsumerByName(d.entity_id);
                    if (consumer != null)
                    {
                        consumer.UpdateFromState(d);
                    }
                }
                break;
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    static async Task SendWebSocketMessage(object msg)
    {
        string m = JsonConvert.SerializeObject(msg);
        Debug.Log($"Sending {m}");
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            SerialMessage.increaseCounter();
            await websocket.SendText(m);
        }
        else
        {
            Debug.Log("Socket is closed");
            throw new Exception("Socket is closed");
        }
    }

    private Task GetInitialState()
    {
        var ev = new StatesRequest();
        SerialMessage.sent[ev.id] = ev;
        //Debug.Log($"{ev.id} {SerialMessage.}");
        return SendWebSocketMessage(ev);
    }

    private Task Subscribe()
    {
        var ev = new SubscribeEvent("state_changed");
        SerialMessage.sent[ev.id] = ev;
        return SendWebSocketMessage(ev);
    }
}
