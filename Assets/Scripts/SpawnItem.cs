using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public float spawnTime = 60f;
    public GameObject[] Items;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnItems", 5f, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnItems()
    {
        int spawnIndex = Random.Range(0, SpawnPoints.Length);
        int ItemIndex = Random.Range(0, Items.Length);
        Instantiate(Items[ItemIndex], SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation);
    }
}
