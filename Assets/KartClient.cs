using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/**
    code from : https://www.youtube.com/watch?v=qGkkaNkq8co
*/

public class KartClient : MonoBehaviour {
    private const int MAX_CONNECTION = 4;
    private int port = 5701;

    private int hostId;
    private int reliableChannel;
    private int unreliableChannel;
    private bool isConnected = false;
    private bool isStarted = false;
    private byte error;

    public void Connect() {

        if (isStarted)
            return;
        NetworkTransport.Init();
        ConnectionConfig cc =  new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);
        hostId = NetworkTransport.AddHost(topo, port, "127.0.0.1");
        int connectId = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        isStarted = true;

        Debug.Log ("connected. hostId is: " + hostId);
    }

    private void Update() {

        if (!isConnected) {
            return;
        }

        int recHostId; 
        int connectionId; 
        int channelId; 
        byte[] recBuffer = new byte[1024]; 
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:                     
                break;
            case NetworkEventType.ConnectEvent:                
                break;
            case NetworkEventType.DataEvent:                   
                break;
            case NetworkEventType.DisconnectEvent:            
                break;
            case NetworkEventType.BroadcastEvent:
                break;
        }
    }
}
