using UnityEngine;
using System.Collections;

public class enemyMovement : MonoBehaviour {

    public float aggroRange = 5f;
    public float attackRange = 1f;
    public float movementSpeed = 2f;
    public float tryMoveTime = 0.5f;
    public LayerMask myLayerMask;
    public float enemyDamage = 15f;

    private Animator myAnimator;
    private GameObject player;
    private SpriteRenderer mySpriteRenderer;
    private health myHealth;

    private bool isStuck = false;
    private bool facingRight = false;
    private bool died = false;
    private Vector3 newDirection;
    private float stuckTimer = 0f;
    private float attackTimer = 0f;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myHealth = GetComponent<health>();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

	void Update () {

        if (!myHealth.isDead)
        {
            
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance < aggroRange)
            {
                SeekPlayer(distance);
            }
            else
            {
                myAnimator.SetFloat("horizontalMovement", 0f);
            }
        } else
        {
            if (!died)
            {
                died = true;
                myAnimator.SetFloat("horizontalMovement", 0f);
                myAnimator.SetTrigger("died");

                transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                gameObject.layer = 11;
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<health>().enabled = false;

            }
        }
        
    }

    private void SeekPlayer(float distance)
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        if (!isStuck)
        {
            if (distance < attackRange)
            {
                if (attackTimer < Time.time)
                {

                    attackTimer = Time.time + 1f;

                    //myAnimator.SetFloat("horizontalMovement", 0f);
                    myAnimator.SetTrigger("isAttacking");

                    player.GetComponent<health>().takeDamage(enemyDamage);
                }

                return;

            }

            if (direction.x > 0f)
            {
                facingRight = true;
                myAnimator.SetFloat("horizontalMovement", 1f);
            }
            else
            {
                facingRight = false;
                myAnimator.SetFloat("horizontalMovement", -1f);
            }

            if (facingRight)
            {
                mySpriteRenderer.flipX = true;
            }
            else
            {
                mySpriteRenderer.flipX = false;
            }
            transform.position += direction * movementSpeed * Time.deltaTime;
        } else
        {
            if (stuckTimer < Time.time)
            {
                newDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
                stuckTimer = Time.time + 0.5f;
            }
            
            transform.position += newDirection * movementSpeed * Time.deltaTime;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100f, myLayerMask);
            Debug.Log(hit.transform.name);

            if(hit.transform.tag == "Player")
            {
                isStuck = false;
            }

            Debug.DrawRay(transform.position, direction, Color.red, 100f);

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == 12)
        {
            isStuck = true;
        }
    }

}
