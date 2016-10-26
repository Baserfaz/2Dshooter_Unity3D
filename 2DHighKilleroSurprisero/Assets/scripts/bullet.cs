using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    public float bulletSpeed = 1f;
    public float timeToLive = 5f;
    public float bulletDamage = 25f;
    public Vector3 target;
    public bool isPlayersBullet = false;

    private Vector3 dir;
    private float timer;

	void Start () {
        dir = target.normalized;
        timer = 0f;
	}

    void Update()
    {

        if (timer < timeToLive)
        {
            timer += Time.deltaTime;
            transform.position += dir * Time.deltaTime * bulletSpeed;
        } else
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(isPlayersBullet)
        {
            // player self
            if(col.gameObject.layer == 8)
            {
                return;
            }

            // enemy
            if(col.gameObject.layer == 14)
            {
                col.transform.GetComponent<health>().takeDamage(bulletDamage);
            }

        }

        Destroy(this.gameObject);

    }


}
