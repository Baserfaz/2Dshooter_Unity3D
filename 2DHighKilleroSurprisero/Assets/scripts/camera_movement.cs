using UnityEngine;
using System.Collections;

public class camera_movement : MonoBehaviour {

    public float cameraSpeed = 1f;
    public float offsetValue = 2f;

    private player_movement myMovement;
    private GameObject myCamera;

    private Vector3 vel;

    void Awake()
    {
        myCamera = Camera.main.gameObject;
        myMovement = GetComponent<player_movement>();
    }

	void Update () {

        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, myCamera.transform.position.z);
        Vector3 offset = Vector3.zero;

        if (myMovement.isMoving)
        {
            if (myMovement.facingRight)
            {
                offset = new Vector3(offsetValue, 0f, 0f);
            }
            else
            {
                offset = new Vector3(-offsetValue, 0f, 0f);
            }
        } else
        {
            offset = Vector3.zero;
        }

        myCamera.transform.position = Vector3.SmoothDamp(myCamera.transform.position, targetPos + offset, ref vel, cameraSpeed);

	}
}
