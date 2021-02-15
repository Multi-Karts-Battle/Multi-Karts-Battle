using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Class PlayerManager, mainly used to control player information in the scene
*/
public class PlayerManager : MonoBehaviour
{
    public GameObject kartPrefab;
    public GameObject myKart;

    public GameObject startButton;
    public GameObject connectButton;
    public GameObject portBox;
    public GameObject playerBox;

    public KartServer kartServer;

    private Player myPlayer;

    private float nextUpdate=1;

    // Start is called before the first frame update
    void Start()
    {
        kartServer = GameObject.Find("NetServer").GetComponent<KartServer>();
        kartServer.playerManager = this;

        myPlayer = kartServer.peers[0];
        //player prefab
        myPlayer.prefab = Instantiate(kartPrefab);
        myPlayer.prefab.GetComponent<CarController>().enabled = true;
        // peer prefab
        for (int i = 1; i < kartServer.peers.Count; i++) {
            kartServer.peers[i].prefab = Instantiate(kartPrefab);
        }
        kartServer.connectToPeers();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(go.transform.position);
        transform.position = myPlayer.prefab.transform.position;

        if(Time.time >= nextUpdate)
        {
             // Change the next update (current second+1)
             nextUpdate = Mathf.FloorToInt(Time.time) + (float)0.2;
             // UpdateEverySecond();
             if (kartServer.peers.Count > 1)
            {
                GamePacket pkt = kartServer.packPeerPosition(myPlayer.playerID, myPlayer.prefab.transform);
                // kartServer.sendData(pkt, 1, false);
                kartServer.sendDataToPeers(pkt, 0, false);
            }
        }
    }

    public void UpdatePosition(string playerID, Vector3 position, Vector3 rotation)
    {
        for (int i = 0; i < kartServer.peers.Count; i++) // TODO: use connectionID as index to update prefab position
        {
            if (kartServer.peers[i].playerID == playerID) 
            {
                kartServer.peers[i].prefab.transform.position = position;
                kartServer.peers[i].prefab.transform.eulerAngles = rotation;
                return;
            }
        }
    }

    public void OnStart() 
    {
        startButton.SetActive(false);
        connectButton.SetActive(false);
        portBox.SetActive(false);
        playerBox.SetActive(false);
    }

    public GameObject SpawnKart()
    {
        GameObject newKart = Instantiate(kartPrefab);
        return newKart;
    }
}
