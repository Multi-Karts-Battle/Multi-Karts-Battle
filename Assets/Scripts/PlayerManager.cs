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

    // Start is called before the first frame update
    void Start()
    {
        myKart = Instantiate(kartPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(go.transform.position);
        transform.position = myKart.transform.position;
    }

    public void OnStart() 
    {
        startButton.SetActive(false);
        connectButton.SetActive(false);
        portBox.SetActive(false);
        playerBox.SetActive(false);
    }
}
