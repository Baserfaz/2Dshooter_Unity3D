using UnityEngine;
using System.Collections;

public class kickable : MonoBehaviour {

    public float yLevel = 0f;
    public bool isSticky = false;
    private float gravityTimer = 0f;

    private Rigidbody2D myRigidBody;

    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update () {

        if (gravityTimer < Time.time)
        {
            if (transform.position.y <= yLevel)
            {
                if(myRigidBody == null)
                {
                    Destroy(this);
                    return;
                }

                myRigidBody.gravityScale = 0f;
                myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, 0f, 0f);
                GetComponent<BoxCollider2D>().enabled = true;
                gravityTimer = Time.time + 0.1f;

                if(isSticky)
                {
                    myRigidBody.velocity = Vector3.zero;
                    myRigidBody.angularVelocity = 0f;
                }

            }
        }

    }
}
