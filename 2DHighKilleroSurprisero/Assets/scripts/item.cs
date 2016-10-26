using UnityEngine;
using System.Collections;

public class item : MonoBehaviour {

    public int Amount = 15;

    private SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartPickUp()
    {

        StartCoroutine("pickUp");
    }

    private IEnumerator pickUp()
    {

        float currentTime = 0f;
        float maxTime = 3f;

        Vector3 endPos = transform.position + new Vector3(0f, 1f, 0f);

        while(currentTime < maxTime)
        {

            currentTime += Time.deltaTime;

            mySpriteRenderer.color = Color.Lerp(mySpriteRenderer.color, Color.clear, currentTime/maxTime);
            transform.position = Vector3.Lerp(transform.position, endPos, currentTime/maxTime);

            yield return null;
        }

        mySpriteRenderer.color = Color.clear;
        transform.position = endPos;

    }


}
