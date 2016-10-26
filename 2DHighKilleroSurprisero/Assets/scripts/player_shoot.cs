using UnityEngine;
using System.Collections;

public class player_shoot : MonoBehaviour {

    public bool isShooting = false;
    public float fireRate = 0.1f;
    public float grenadeRefreshRate = 5f;
    public float grenadeThrowPower = 10f;

    public int maxAmmo = 150;
    public int currentAmmo;

    private Vector3 bulletOffset = new Vector3(0.6f, 0f, 0f);
    private Vector3 bulletOffsetTemp;

    private float fireRateTimer;
    private float grenadeTimer;

    private player_movement myPlayerMovement;
    private health myHealth;
    private Animator myAnimator;
    private GameObject GameMaster;

    public GameObject bullet;
    public GameObject grenade;

    void Awake()
    {
        myPlayerMovement = GetComponent<player_movement>();
        myHealth = GetComponent<health>();
        myAnimator = GetComponent<Animator>();
        GameMaster = GameObject.Find("GameMaster");
        fireRateTimer = 0f;
        grenadeTimer = 0f;
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        bullet = GameMaster.GetComponent<GameObjectMaster>().bullet;
        grenade = GameMaster.GetComponent<GameObjectMaster>().grenade;
    }

    public void Shoot()
    {

        if (Input.GetButton("Fire1"))
        {

            if(currentAmmo <= 0)
            {
                // reload?
                // anyway, play click sound.
                // TODO!
                return;
            }

            isShooting = true;

            if (Time.time > fireRateTimer)
            {
                fireRateTimer = Time.time + fireRate;

                currentAmmo--;

                GameMaster.GetComponent<guimanager>().UpdateAmmoCount(currentAmmo);

                if (myPlayerMovement.facingRight)
                {
                    bulletOffsetTemp = bulletOffset;
                }
                else
                {
                    bulletOffsetTemp = -bulletOffset;
                }

                GameObject bulletInst = (GameObject)Instantiate(bullet, transform.position + bulletOffsetTemp, transform.rotation);

                bullet myBullet = bulletInst.GetComponent<bullet>();

                myBullet.isPlayersBullet = true;

                if (myPlayerMovement.facingRight)
                {
                    myBullet.target = CalculateBulletSpray(transform.right);
                    myBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f));
                }
                else
                {
                    myBullet.target = CalculateBulletSpray(-transform.right);
                    myBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 90f));
                }
            }
        }
        else if(Input.GetButtonDown("Fire2"))
        {

            if (grenadeTimer < Time.time)
            {

                grenadeTimer = grenadeRefreshRate + Time.time;

                GameObject.Find("GameMaster").GetComponent<guimanager>().guiGrenade.GetComponent<ui_transparent>().StartTransparent(grenadeRefreshRate);

                myAnimator.SetBool("isThrowingGrenade", true);

                GameObject grenadeInst = (GameObject)Instantiate(grenade, transform.position, Quaternion.identity);

                grenadeInst.GetComponent<grenade>().yLevel = transform.position.y + Random.Range(-0.5f, 0.5f);

                if (myPlayerMovement.facingRight)
                {
                    grenadeInst.GetComponent<Rigidbody2D>().AddForce((transform.right + new Vector3(0f, 1.5f, 0f)).normalized * grenadeThrowPower, ForceMode2D.Impulse);
                }
                else
                {
                    grenadeInst.GetComponent<Rigidbody2D>().AddForce((-transform.right + new Vector3(0f, 1.5f, 0f)).normalized * grenadeThrowPower, ForceMode2D.Impulse);
                }

                grenadeInst.GetComponent<Rigidbody2D>().AddTorque(25f, ForceMode2D.Impulse);
            } else
            {
                // TODO, grenade on cooldown..
                // do something silly here.
            }
        } else
        {
            isShooting = false;
        }
    }

    private Vector3 CalculateBulletSpray(Vector3 root)
    {

        Vector3 offsetVector = new Vector3(0f, Random.Range(-0.05f, 0.05f), 0f);

        root += offsetVector;

        return root;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;

        if(currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}
