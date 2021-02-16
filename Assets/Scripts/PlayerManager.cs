using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using UnityEngine;

/**
    Class PlayerManager, mainly used to control player information in the scene
*/
public class PlayerManager : MonoBehaviour
{
    public GameObject kartPrefab;
    public GameObject myKart;

    public KartServer kartServer;

    private Player myPlayer;

    private float nextUpdate = 1;
    private Vector3 lastPos = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        kartServer = GameObject.Find("NetServer").GetComponent<KartServer>();
        kartServer.playerManager = this;
        kartServer.onBattleScene = true;

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
        float dist = Vector3.Distance(lastPos, transform.position);
        if(Time.time >= nextUpdate && dist > .05)
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
            lastPos = transform.position;
        }
    }

    public void UpdatePosition(string playerID, Vector3 position, Vector3 rotation)
    {
        for (int i = 0; i < kartServer.peers.Count; i++) // TODO: use connectionID as index to update prefab position
        {
            if (kartServer.peers[i].playerID == playerID) 
            {
                //update using interpolation
                // kartServer.peers[i].prefab.transform.position = Vector3.Lerp (kartServer.peers[i].prefab.transform.position, position, 0.5F);
                kartServer.peers[i].prefab.transform.position = position;
                kartServer.peers[i].prefab.transform.eulerAngles = rotation;
                return;
            }
        }
    }

    public GameObject SpawnKart()
    {
        GameObject newKart = Instantiate(kartPrefab);
        return newKart;
    }
}
