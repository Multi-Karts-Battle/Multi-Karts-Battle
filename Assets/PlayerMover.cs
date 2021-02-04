using UnityEngine;
using UnityEngine.Networking;

public class PlayerMover : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0f, vertical);
            transform.position += movement * Time.deltaTime * moveSpeed;
        }
    }
}
