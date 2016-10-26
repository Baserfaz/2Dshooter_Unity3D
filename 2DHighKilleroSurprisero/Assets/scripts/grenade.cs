using UnityEngine;
using System.Collections;

public class grenade : MonoBehaviour {

    [HideInInspector]
    public float yLevel = 0f;

    [Header("Settings")]
    public float timeToLive = 2f;
    public float explosionRadius = 3f;
    public float explosionBaseDamage = 50f;

    [Header("Color")]
    public Color explosionColor;

    [Header("Sprites")]
    public Sprite spr_poolOfBlood;

    private GameObject blood_particleEffect;
    private GameObject dirt_particleEffect;

    private GameObject crater;
    private GameObject[] bloods;
    private GameObject[] rocks;

    private float explosionTimer;
    private float gravityTimer;

    private bool exploded = false;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private GameObject GameMaster;

    private bool hitGround = false;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        GameMaster = GameObject.Find("GameMaster");

        blood_particleEffect = GameMaster.GetComponent<GameObjectMaster>().blood_particle;
        dirt_particleEffect = GameMaster.GetComponent<GameObjectMaster>().dirt_particle;

        crater = GameMaster.GetComponent<GameObjectMaster>().crater;
        bloods = GameMaster.GetComponent<GameObjectMaster>().bloods;
        rocks = GameMaster.GetComponent<GameObjectMaster>().rocks;
    }

    void Start()
    {
        explosionTimer = Time.time + timeToLive;
        gravityTimer = Time.time + 0.1f;
        GetComponent<BoxCollider2D>().enabled = false;
    }

	void Update () {
	
        if(explosionTimer < Time.time)
        {
            if (!exploded)
            {
                exploded = true;
                Explode();
            }
        }

        if (gravityTimer < Time.time)
        {
            if (transform.position.y <= yLevel)
            {
                if (!hitGround)
                {
                    hitGround = true;
                    myRigidbody.gravityScale = 0f;
                    myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, 0f, 0f);
                    GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
	}

    private void Explode()
    {

        myRigidbody.velocity = Vector3.zero;
        myRigidbody.angularVelocity = 0f;

        GetComponent<SpriteRenderer>().color = explosionColor;

        myAnimator.SetTrigger("explosion");

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach(Collider2D col in colls)
        {
            if (col.tag == "blood")
            {
                SpawnBlood(col);
                continue;
            } else if (col.tag == "rock") {

                SpawnRocks(col);
                continue;

            } else if (col.transform.tag == "Enemy") { 

                if(col.gameObject.GetComponent<health>().isDead)
                {
                    Destroy(col.transform.GetComponent<Animator>());
                    col.transform.GetComponent<SpriteRenderer>().sprite = spr_poolOfBlood;

                    Destroy(col.transform.GetComponent<Rigidbody2D>());
                    Destroy(col.transform.GetComponent<BoxCollider2D>());

                    GameObject bloodInst = (GameObject)Instantiate(blood_particleEffect, col.transform.position, blood_particleEffect.transform.rotation);

                    Destroy(bloodInst, bloodInst.GetComponent<ParticleSystem>().duration);

                }

            } else
            { 
                if(col.GetComponent<Rigidbody2D>() != null && col.tag != "Player")
                {

                    Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
                    kickable colKickable = col.GetComponent<kickable>();

                    if (colKickable != null)
                    {
                        colKickable.yLevel = col.transform.position.y + Random.Range(-1f, 1f);
                    }

                    col.GetComponent<BoxCollider2D>().enabled = false;

                    colRigidbody.gravityScale = 1f;

                    colRigidbody.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 2f, 0f) * 5f, ForceMode2D.Impulse);
                    colRigidbody.AddTorque(0.5f, ForceMode2D.Impulse);
                }
            }

            float damageMultiplier = Vector3.Distance(col.transform.position, transform.position);

            if (damageMultiplier != 0f)
            {
                if (col.transform.GetComponent<health>() != null)
                {
                    col.transform.GetComponent<health>().takeDamage(explosionBaseDamage / damageMultiplier);
                    SpawnBlood(col);
                }
            } else
            {
                if (col.transform.GetComponent<health>() != null)
                {
                    col.transform.GetComponent<health>().takeDamage(explosionBaseDamage);
                    SpawnBlood(col);
                }
            }
        }

        Vector3 craterPos = new Vector3(transform.position.x, transform.position.y, crater.transform.position.z);

        GameObject craterInst = (GameObject)Instantiate(crater, craterPos, Quaternion.identity);

        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(this.gameObject, 0.2f);
    }

    // spawnRocks & spawnBlood are similar.
    // --> maybe merge them together?
    private void SpawnRocks(Collider2D col)
    {
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            GameObject rockInst = (GameObject)Instantiate(rocks[Random.Range(0, rocks.Length)], col.transform.position, Quaternion.identity);

            rockInst.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, 1f);

            rockInst.AddComponent<kickable>();
            rockInst.GetComponent<kickable>().yLevel = transform.position.y + Random.Range(-1f, 1f);
            rockInst.GetComponent<kickable>().isSticky = true;

            rockInst.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f), 2f, 0f) * 5f, ForceMode2D.Impulse);
            rockInst.GetComponent<Rigidbody2D>().AddTorque(0.05f, ForceMode2D.Impulse);
        }
    }

    private void SpawnBlood(Collider2D col)
    {
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            GameObject bloodInst = (GameObject)Instantiate(bloods[Random.Range(0, bloods.Length)], col.transform.position, Quaternion.identity);

            bloodInst.transform.position = new Vector3(col.transform.position.x, col.transform.position.y, 1f);

            bloodInst.AddComponent<kickable>();
            bloodInst.GetComponent<kickable>().yLevel = transform.position.y + Random.Range(-1f, 1f);
            bloodInst.GetComponent<kickable>().isSticky = true;

            if(bloodInst.GetComponent<Rigidbody2D>() == null)
            {
                Rigidbody2D rb = bloodInst.AddComponent<Rigidbody2D>();
                rb.gravityScale = 0f;
                rb.drag = 0.5f;
            }

            bloodInst.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f), 2f, 0f) * 5f, ForceMode2D.Impulse);
            bloodInst.GetComponent<Rigidbody2D>().AddTorque(0.05f, ForceMode2D.Impulse);
        }
    }

}
