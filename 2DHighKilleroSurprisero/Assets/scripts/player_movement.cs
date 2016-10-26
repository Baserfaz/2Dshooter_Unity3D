using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {

    public float movementSpeedHorizontal = 2f;
    public float movementSpeedWhileShooting = 1f;
    public float movementSpeedVertical = 1f;

    private float movementMagnitude = 0f;

    public bool facingRight;
    public bool isMoving = false;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private player_shoot myPlayerShoot;
    private health myPlayerHealth;
    private GameObject GameMaster;

    private float lastHorizontalMovement;

    private float horizontalMovement;
    private float verticalMovement;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myPlayerShoot = GetComponent<player_shoot>();
        myPlayerHealth = GetComponent<health>();
        GameMaster = GameObject.Find("GameMaster");
    }

    public void Move()
    {

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontalMovement) == 0f && Mathf.Abs(verticalMovement) == 0f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (myAnimator != null)
        {
            myAnimator.SetFloat("HorizontalMove", horizontalMovement);
            myAnimator.SetFloat("verticalMove", verticalMovement);
        }

        // calculate speed
        if (myPlayerShoot.isShooting)
        {
            movementMagnitude = movementSpeedWhileShooting;
        }
        else
        {
            if (Mathf.Abs(horizontalMovement) > 0f)
            {
                movementMagnitude = movementSpeedHorizontal;
            }
            else if (Mathf.Abs(verticalMovement) > 0f)
            {
                movementMagnitude = movementSpeedVertical;
            }
        }

        // move player.
        transform.position += (new Vector3(horizontalMovement, verticalMovement, 0f)).normalized * movementMagnitude * Time.deltaTime;

        if (!myPlayerShoot.isShooting)
        {
            // remember which way we were last moving towards.
            if (Mathf.Abs(horizontalMovement) > 0f)
                lastHorizontalMovement = horizontalMovement;
        }

        if (!myPlayerShoot.isShooting)
        {
            // flip sprites.
            if (lastHorizontalMovement < 0f)
            {
                mySpriteRenderer.flipX = true;
                facingRight = false;
            }
            else
            {
                mySpriteRenderer.flipX = false;
                facingRight = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        // wall
        if(coll.gameObject.layer == 12)
        {
            return;
        }

        if(coll.transform.tag == "Ammo")
        {

            if (myPlayerShoot.currentAmmo < myPlayerShoot.maxAmmo)
            {
                // TODO: play cheese KAZING/ammobelt sound here.

                myPlayerShoot.AddAmmo(coll.transform.GetComponent<item>().Amount);

                GameMaster.GetComponent<guimanager>().UpdateAmmoCount(myPlayerShoot.currentAmmo);

                coll.transform.GetComponent<item>().StartPickUp();

                Destroy(coll.gameObject, 5f);
            }
            {
                kickThing(coll);
            }

        } else if(coll.transform.tag == "Health")
        {
            if (myPlayerHealth.currentHealth < myPlayerHealth.maxHealth)
            {
                // TODO: play "uugh" sound effect here :D

                myPlayerHealth.healDamage(coll.transform.GetComponent<item>().Amount);

                coll.transform.GetComponent<item>().StartPickUp();

                Destroy(coll.gameObject, 5f);
            }
            {
                kickThing(coll);
            }


        } else if(coll.transform.tag == "Points")
        {

            // TODO: play kazing/GOLD sound here.

            GetComponent<player>().playerScore += coll.transform.GetComponent<item>().Amount;

            coll.transform.GetComponent<item>().StartPickUp();

            Destroy(coll.gameObject, 5f);

        } else if(coll.transform.tag == "Props")
        {

            kickThing(coll);

        }
    }

    private void kickThing(Collision2D coll)
    {
        coll.transform.GetComponent<kickable>().yLevel = transform.position.y + Random.Range(-1f, 1f);
        coll.transform.GetComponent<BoxCollider2D>().enabled = false;

        Rigidbody2D Rb = coll.transform.GetComponent<Rigidbody2D>();
        Rb.gravityScale = 1f;
        Rb.AddForce(new Vector3(horizontalMovement * 1.5f, 0f, 0f) + new Vector3(0f, 1f, 0f) * 5f, ForceMode2D.Impulse);
        Rb.AddTorque(0.5f, ForceMode2D.Impulse);
    }

}
