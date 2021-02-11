using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
    code from : https://www.youtube.com/watch?v=qGkkaNkq8co
*/

public class KartServer : MonoBehaviour {
    private const int MAX_CONNECTION = 4;
    private int port = 8888;

    private int hostId;
    private int reliableChannel;
    private int unreliableChannel;
    private bool isStarted = false;
    private byte error;

    private void Start() {
        NetworkTransport.Init();
        ConnectionConfig cc =  new ConnectionConfig();
        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);
        hostId = NetworkTransport.AddHost(topo, port);
        

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        
        Debug.Log ("Socket Open. hostId is: " + hostId);

        isStarted = true;
    }

    private void Update()
    {
        // if (!isStarted) {
        //     return;
        // }
        // int recHostId; 
        // int connectionId; 
        // int channelId; 
        // byte[] recBuffer = new byte[1024]; 
        // int bufferSize = 1024;
        // int dataSize;
        // byte error;
        // NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        NetworkTransport.Init();
        int outHostId;
        int outConnectionId;
        int outChannelId;

        int receivedSize;
        byte error;
        byte[] buffer = new byte[256];
        NetworkEventType recData = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:                     
                break;
            case NetworkEventType.ConnectEvent:                
                Debug.Log ("Connect" + hostId);
                break;
            case NetworkEventType.DataEvent:                   
                break;
            case NetworkEventType.DisconnectEvent:            
                break;
            default:
                Debug.Log ("??" + hostId);
                break;
        }
        NetworkTransport.Shutdown();
    }


    void OnDestroy()
    {
        NetworkTransport.Shutdown();
    }
}