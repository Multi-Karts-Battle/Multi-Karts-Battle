using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;

/**
    code from : https://www.youtube.com/watch?v=qGkkaNkq8co
    useful doc:
        Netwokr error code : https://docs.unity3d.com/ScriptReference/Networking.NetworkError.html
*/

/**
    Class player used to save peer information on local machine.
*/
public class Player 
{
    // identification in the global Network
    public string playerID;
    public string IP;
    public int port;
    // unique connectionID used on local machine, returned when NetworkTransport.Connect is called
    public int connectionID; 
    // GameObject
    public GameObject prefab;
};

public class KartServer : MonoBehaviour {

    // for NAT punchthrough
    // turn off when testing locally
    public const bool useNAT = true; 
    NATHelper natHelper;
    string hostExternalIP, hostInternalIP;
    string serverIP;
    GameObject canvas;
    private const int MAX_CONNECTION = 4;
    private int serverPort = 8888;
    private int clientPort;

    private int myHostId;
    private int serverConnectionId;
    private int reliableChannel;
    private int unreliableChannel;
    private bool isServer = false;
    private bool isStarted = false;
    private byte error;

    private ConnectionConfig cc;
    private HostTopology topo;

    private string myPlayerID = "";
    public List<Player> peers = new List<Player>(MAX_CONNECTION);

    public PlayerManager playerManager;
    public bool onBattleScene = false;

    private void Start() {

        // connect to NAT server before we activate canvas
        natHelper = GetComponent<NATHelper>();
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(natHelper.isReady);

        // network init and config
        NetworkTransport.Init();
        cc =  new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);
        topo = new HostTopology(cc, MAX_CONNECTION);
        // playerManager =  GameObject.Find("Player").GetComponent<PlayerManager>();
        
    }

#region NAT setup
    public void OnStartNAT() {
        natHelper.startListeningForPunchthrough(OnHolePunchedServer);
    }

    public void OnConnectNAT() {
        if (natHelper.targetGUID == "") {
            natHelper.targetGUID = GameObject.Find("guidID").GetComponent<InputField>().text;
        }
        natHelper.punchThroughToServer(natHelper.targetGUID, OnHolePunchedClient);
    }

    /**
     * Server received a hole punch from a client
     * Start up a new NATServer listening on the newly punched hole
     */
    void OnHolePunchedServer(int natListenPort)
    {
        // NATServer newServer = new NATServer();
        // bool isListening = newServer.Listen(natListenPort, NetworkServer.hostTopology);
        // if (isListening)
        // {
        //     natServers.Add(newServer);
        // }
        // TODO : CREATE NEW SERVER FOR EACH PUNCHED PORT?
        serverPort = natListenPort;
        OnStartHost();
    }

        void OnHolePunchedClient(int natListenPort, int natConnectPort, string serverAddress)
    {
        // The port on the server that we are connecting to
        int networkPort = natConnectPort;

        // Make sure to connect to the correct IP or things won't work
        // if (hostExternalIP == natHelper.externalIP)
        // {
        //     if (hostInternalIP == Network.player.ipAddress)
        //     {
        //         // Host is running on the same computer as client, two separate builds
        //         networkAddress = "127.0.0.1";
        //     }
        //     else
        //     {
        //         // Host is on the same local network as client
        //         networkAddress = hostInternalIP;
        //     }
        // }
        // else
        // {
        //     // Host is somewhere out on the internet
        //     networkAddress = hostExternalIP;
        // }
        
        // Debug.Log("Attempting to connect to server " + networkAddress + ":" + networkPort);
        Debug.Log("Attempting to connect to server " + ":" + networkPort);

        // reset network configuration
        clientPort = natListenPort;
        serverPort = networkPort;
        serverIP = serverAddress;
        Connect();
        // Magic! Set the client's transport level host ID so that the client will use
        // the host we just started above instead of the one it creates internally when we call Connect.
        // This has to be done so that the connection will be made from the correct port, otherwise
        // Unity will use a random port to connect from and NAT Punchthrough will fail.
        // This is the shit that keeps me up at night.
        // clientIDField.SetValue(client, natListenSocketID);

        // // Tell Unity to use the client we just created as _the_ client so that OnClientConnect will be called
        // // and all the other HLAPI stuff just works. Oh god, so nice.
        // UseExternalClient(client);
    }

# endregion

#region plain server-client setup

    /**
        function connected to StartGame button in the Lobby scene
    */
    public void OnStartHost() {

        if (isStarted)
            return;

        isServer = true;
        // myPlayerID = "host"; // TODO: custom hostID

        myPlayerID = GameObject.Find("PlayerName").GetComponent<InputField>().text;
        myHostId = NetworkTransport.AddHost(topo, serverPort);
        Debug.Log ("Socket Open. hostId is: " + myHostId);
        
        // Host is the first peer in the network, so we add host to peers
        clientPort = serverPort;
        Player hostPlayer = new Player();
        hostPlayer.playerID = myPlayerID; 
        hostPlayer.IP = "127.0.0.1"; // TODO: change this for nonlocal test
        hostPlayer.port = clientPort;
        hostPlayer.connectionID = 0;
        peers.Add(hostPlayer);
        isStarted = true;
    }

    /**
        function connected to Connect button in the Lobby scene
    */
    public void Connect() {

        if (isStarted)
        {
            return;
        }
            
        if (!useNAT)
        {
            string t = GameObject.Find("PortNum").GetComponent<InputField>().text;
            if (t == "") {
                Debug.Log ("Missing port number. No connection.");
                return;
            }
            clientPort = int.Parse(t);
        }

        myPlayerID = GameObject.Find("PlayerName").GetComponent<InputField>().text;
        if (myPlayerID == "") {
            Debug.Log ("Missing PlayerID. No connection.");
            return;
        }

        myHostId = NetworkTransport.AddHost(topo, clientPort);// "127.0.0.1:clientPort";
        serverConnectionId = NetworkTransport.Connect(myHostId, serverIP, serverPort, 0, out error);
        Debug.Log("connect(hostId = " + myHostId + ", connectionId = "+ serverConnectionId + ", error = " + error.ToString() + ")");
        
        // oneself is the first peer in the network, so we add to peers
        Player hostPlayer = new Player();
        hostPlayer.playerID = myPlayerID; 
        hostPlayer.IP = "127.0.0.1"; // TODO: change this for nonlocal test? not sure do we stil use this address over NAT
        hostPlayer.port = clientPort;
        hostPlayer.connectionID = 0;
        peers.Add(hostPlayer);

        isStarted = true;

        #if UNITY_EDITOR  //not work
            EditorUtility.DisplayDialog("Tip", "You have been connected!", "OK", ""); 
        #endif   
    }
#endregion
    private void Update()
    {        
        if (natHelper.isReady && !onBattleScene){
            canvas.SetActive(natHelper.isReady);
            GameObject.Find("guidText").GetComponent<Text>().text = natHelper.guid;
            // GameObject.Find("guidText").GetComponent<Text>().fontSize = 30;
            // text.text = natHelper.guid;
            // Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            // text.font = ArialFont;
            // text.material = ArialFont.material;
        }
            

        if (!isStarted) {
            return;
        }
        
        int outHostId;
        int outConnectionId;
        int outChannelId;

        int receivedSize;
        byte error;
        byte[] buffer = new byte[256];
        NetworkEventType recData = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);
        
        if ((NetworkError)error != NetworkError.Ok) {
            Debug.Log("Network error on frame update (error code) :" + error.ToString());
        }
        
        switch (recData)
        {
            case NetworkEventType.Nothing:                    
                break;
            case NetworkEventType.ConnectEvent:     
                OnConnectEvent(outHostId, outConnectionId, error);
                break;
            case NetworkEventType.DataEvent: 
                OnDataEvent(outHostId, outConnectionId, error, buffer, receivedSize);           
                break;
            case NetworkEventType.DisconnectEvent:            
                if (outHostId == myHostId && outConnectionId == serverConnectionId)
                {
                    Debug.Log("Connected, error:" + error.ToString());
                }
                break;
            default:
                Debug.Log ("Unknown event.");
                break;
        }
    }

    private void OnConnectEvent(int outHostId, int outConnectionId, byte error) 
    {
        if (outHostId == myHostId && outConnectionId == serverConnectionId && (NetworkError)error == NetworkError.Ok)
        {
            Debug.Log("Connected.");
        }
        else 
        {
            Debug.Log("Received a new connection : hostId =" + outHostId + ", connectionId = "+ outConnectionId);
             // ask info from new peer
            GamePacket packet = packAskInfo();
            sendData(packet, outConnectionId);
            if (isServer)
            {
                // server will notify the new peer of current peerInfo
                for (int i = 0; i < peers.Count; i++) 
                {
                    packet = packPeerInfo(peers[i].playerID, peers[i].IP, peers[i].port);
                    sendData(packet, outConnectionId);
                }
            }       
        }
    }

    private void OnDataEvent(int outHostId, int outConnectionId, byte error, byte[] buffer, int receivedSize) 
    {
        Debug.Log("Received data : hostId =" + outHostId + ", connectionId = "+ outConnectionId);
        if ((NetworkError)error != NetworkError.Ok) {
            Debug.Log("Network error on Data Event (error code) :" + error.ToString());
            return;
        }
        
        GamePacket packet = new GamePacket();
        packet.GeneratePacket(buffer, true);
        switch (packet.gameEvent) 
        {
            case "AskInfo":
                packet = packPeerInfo(myPlayerID, "127.0.0.1", clientPort);
                sendData(packet, outConnectionId);
                break;
            case "PeerInfo": 
                // Spawn player, this is usually happening after building connection
                bool spawned = SpawnPlayer(packet, outConnectionId);
                // Forward packet to other clients
                if (spawned && isServer) {
                    // only server will send new peer information to others
                    // sendData(packet, outConnectionId);
                    sendDataToPeers(packet, outConnectionId);
                }
                break;
            case "PosInfo":
                if (onBattleScene)
                    playerManager.UpdatePosition(packet.playerID, packet.playerPosition, packet.playerRotation);
                break;
        }
    }

    public void sendData(GamePacket packet, int connectionID, bool reliable = true) {
        ByteBuffer byteBuffer = packet.GetBuffer();
        byte[] buffer = byteBuffer.ToArray();
        int bufferSize = byteBuffer.Length();
        byte error;
        int channelID = reliable? reliableChannel : unreliableChannel;
        NetworkTransport.Send(myHostId, connectionID, channelID, buffer, bufferSize, out error);
        if ((NetworkError)error != NetworkError.Ok) {
            Debug.Log("Network error when sending data (error code) :" + error.ToString());
        }
    }

    public void sendDataToPeers(GamePacket packet, int exclusiveID = 0, bool reliable=true) {
        ByteBuffer byteBuffer = packet.GetBuffer();
        byte[] buffer = byteBuffer.ToArray();
        int bufferSize = byteBuffer.Length();
        for (int i = 1; i < peers.Count; i++)
        {
            if (i == exclusiveID) 
            {
                continue;
            }
            int channelID = reliable? reliableChannel : unreliableChannel;
            NetworkTransport.Send(myHostId, peers[i].connectionID, channelID, buffer, bufferSize, out error);
            if ((NetworkError)error != NetworkError.Ok) {
                Debug.Log("Network error when sending data to peers(error code) :" + error.ToString());
            }
        }
    }

    public void connectToPeers() {
        // client connect to other peers
        for (int i = 2; i < peers.Count; i++)
        {
            // TODO: reorganize topology
            if (peers[i].connectionID == -1)
            {
                peers[i].connectionID = NetworkTransport.Connect(myHostId, peers[i].IP, peers[i].port, 0, out error);
                if ((NetworkError)error != NetworkError.Ok) {
                    Debug.LogError("Network error when sending data (error code) :" + error.ToString());
                }
            }
        }
    }

    public GamePacket packAskInfo() 
    {
        GamePacket packet = new GamePacket();
        packet.gameEvent = "AskInfo";
        return packet;
    }

    public GamePacket packPeerInfo(string playerID, string IP, int port) 
    {
        GamePacket packet = new GamePacket();
        packet.gameEvent = "PeerInfo";
        packet.playerID = playerID;
        packet.info = IP + "|" + port;
        return packet;
    }

    public GamePacket packPeerPosition(string playerID, Transform trans) 
    {
        GamePacket packet = new GamePacket();
        packet.gameEvent = "PosInfo";
        packet.playerID = playerID;
        packet.playerPosition.x = trans.position.x;
        packet.playerPosition.y = trans.position.y;
        packet.playerPosition.z = trans.position.z;
        packet.playerRotation.x = trans.eulerAngles.x;
        packet.playerRotation.y = trans.eulerAngles.y;
        packet.playerRotation.z = trans.eulerAngles.z;
        return packet;
    }

    private int findPlayer(Player player)
    {
        for (int i = 0; i < peers.Count; i++)
        {
            if (peers[i].IP == player.IP && peers[i].port == player.port && player.playerID == peers[i].playerID)
                return i;
        }
        return -1;
    }

    private bool SpawnPlayer(GamePacket packet, int connectionID) 
    {
        Player player = new Player();
        player.playerID = packet.playerID;
        if (!isServer && peers.Count >= 2) {
            player.connectionID = -1;
        } else {
            player.connectionID = connectionID;
        }            
        string[] subs = packet.info.Split('|');
        player.IP = subs[0];
        player.port = int.Parse(subs[1]);
        int found = findPlayer(player);
        
        if (isServer && found != -1)
        {
            Debug.Log("Player " +player.playerID + " already added! Now we have " + peers.Count + " peers in the room!");
            Debug.Assert(!isServer || peers[connectionID].IP == player.IP);
            Debug.Assert(!isServer || peers[connectionID].port == player.port);  
            return false;
        } else if (!isServer && found != -1)
        {
            peers[found].connectionID = connectionID;
            Debug.Log("Update Player " +player.playerID + " connectionID to " + connectionID);
            return false;
        }

        peers.Add(player);
        Debug.Log("Player " +player.playerID + " joined! Now we have " + peers.Count + " peers in the room!");
        return true;
    }

    void OnDestroy()
    {
        NetworkTransport.Shutdown();
    }
}
