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
        myPlayer.prefab.GetComponent<TextMesh>().text = myPlayer.playerID;

        // peer prefab
        for (int i = 1; i < kartServer.peers.Count; i++) {
            kartServer.peers[i].prefab = Instantiate(kartPrefab);
            kartServer.peers[i].prefab.GetComponent<TextMesh>().text = kartServer.peers[i].playerID;
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

    IEnumerator LerpPosition(string playerID, Vector3 targetPosition, Vector3 targetRotation, float duration)
    {
        for (int i = 1; i < kartServer.peers.Count; i++) // TODO: use connectionID as index to update prefab position
        {
            if (kartServer.peers[i].playerID == playerID) 
            {
               float time = 0;
               Vector3 startPosition = kartServer.peers[i].prefab.transform.position;
               UnityEngine.Quaternion startRotationQ = kartServer.peers[i].prefab.transform.rotation;
               UnityEngine.Quaternion targetRotationQ = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z);

                while (time < duration)
                {
                    kartServer.peers[i].prefab.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                    kartServer.peers[i].prefab.transform.rotation = Quaternion.Lerp(startRotationQ, targetRotationQ, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
                kartServer.peers[i].prefab.transform.position = targetPosition;
                kartServer.peers[i].prefab.transform.rotation = targetRotationQ;
            }
        }
    }

    void UpdatePosition() {
        for (int i = 1; i < kartServer.peers.Count; i++)
        {
            if (Vector3.Distance(kartServer.peers[i].prefab.transform.position, kartServer.peers[i].targetPosition)>.05)
            {
                kartServer.peers[i].prefab.transform.position = Vector3.Lerp (kartServer.peers[i].prefab.transform.position, kartServer.peers[i].targetPosition, 0.1F);
                kartServer.peers[i].prefab.transform.rotation = Quaternion.Lerp(kartServer.peers[i].prefab.transform.rotation, Quaternion.Euler(kartServer.peers[i].targetRotation.x, kartServer.peers[i].targetRotation.y, kartServer.peers[i].targetRotation.z), 0.1F);
            } else {
                kartServer.peers[i].prefab.transform.position = kartServer.peers[i].targetPosition;
                kartServer.peers[i].prefab.transform.rotation =  Quaternion.Euler(kartServer.peers[i].targetRotation.x, kartServer.peers[i].targetRotation.y, kartServer.peers[i].targetRotation.z);
            }
        }
    }

    public void UpdatePosition(string playerID, Vector3 position, Vector3 rotation)
    {
        //update using interpolation, instead of update the position immediately, we save the target value and update with interpolation
        StartCoroutine(LerpPosition(playerID, position, rotation, (float)0.2));
        // for (int i = 1; i < kartServer.peers.Count; i++) // TODO: use connectionID as index to update prefab position
        // {
        //     if (kartServer.peers[i].playerID == playerID) 
        //     {
        //         // update without interpolation 
        //         kartServer.peers[i].prefab.transform.position = position;
        //         kartServer.peers[i].prefab.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);

        //         return;
        //     }
        // }
    }

    public GameObject SpawnKart()
    {
        GameObject newKart = Instantiate(kartPrefab);
        return newKart;
    }
}
