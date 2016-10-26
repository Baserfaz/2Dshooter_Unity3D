using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

    public int playerLives = 3;
    public float playerScore = 0f;

    private health myHealth;
    private player_shoot myPlayerShoot;
    private player_movement myPlayerMovement;
    private Animator myAnimator;

    void Awake()
    {
        myHealth = GetComponent<health>();
        myPlayerShoot = GetComponent<player_shoot>();
        myPlayerMovement = GetComponent<player_movement>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if(myHealth.isDead)
        {
            myAnimator.SetBool("isDead", true);
            return;
        }

        myPlayerShoot.Shoot();
        myPlayerMovement.Move();

    }



}
