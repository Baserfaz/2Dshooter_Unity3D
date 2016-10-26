using UnityEngine;
using System.Collections;

public class health : MonoBehaviour {

    public float currentHealth;
    public float maxHealth = 100f;

    public bool isDead = false;
    public bool isPlayer = false;

    private Color StartColor;
    private SpriteRenderer mySpriteRenderer;
    private GameObject GameMaster;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        GameMaster = GameObject.Find("GameMaster");
    }

    void Start()
    {
        currentHealth = maxHealth;
        StartColor = mySpriteRenderer.color;
    }

    public void healDamage(float amount)
    {
       if(isDead)
        {
            return;
        }

        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(isPlayer)
        {
            GameObject.Find("GameMaster").GetComponent<guimanager>().UpdateHealthBar(currentHealth, maxHealth);
        }

    }

    public void takeDamage(float amount)
    {
        if(isDead)
        {
            return;
        }

        StartCoroutine("hitEffect");

        currentHealth -= amount;

        if(currentHealth <= 0f)
        {
            isDead = true;
            currentHealth = 0f;
        }

        if (isPlayer)
        {
            GameObject.Find("GameMaster").GetComponent<guimanager>().UpdateHealthBar(currentHealth, maxHealth);
        }

    }

    private void SpawnBlood()
    {
        GameObject[] bloods = GameMaster.GetComponent<GameObjectMaster>().bloods;

        for (int i = 0; i < Random.Range(0, 3); i++)
        {
            GameObject bloodInst = (GameObject)Instantiate(bloods[Random.Range(0, bloods.Length)], transform.position, Quaternion.identity);

            bloodInst.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);

            bloodInst.AddComponent<kickable>();
            bloodInst.GetComponent<kickable>().yLevel = transform.position.y + Random.Range(-1f, 1f);
            bloodInst.GetComponent<kickable>().isSticky = true;

            bloodInst.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-1f, 1f), 2f, 0f) * 5f, ForceMode2D.Impulse);
            bloodInst.GetComponent<Rigidbody2D>().AddTorque(0.05f, ForceMode2D.Impulse);
        }
    }

    // pulse some color.
    private IEnumerator hitEffect()
    {
        float currentTime = 0f;
        float maxTime = 1f;

        mySpriteRenderer.color = Color.red;

        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;

            mySpriteRenderer.color = Color.Lerp(mySpriteRenderer.color, StartColor, currentTime/maxTime);

            yield return null;
        }

        mySpriteRenderer.color = StartColor;
    }


}
