using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotateSpeed = 50f;
    public float DestroyTime = 60f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up*Time.deltaTime*RotateSpeed, Space.World);
    }
}
