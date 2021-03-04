using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        if (gameObject.name == "Heart") {
            Health health = GameObject.Find("Player").GetComponent<Health>();
            health.health++;
        }
    }
}
