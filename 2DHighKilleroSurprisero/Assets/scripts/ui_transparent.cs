using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ui_transparent : MonoBehaviour {

    private Image myImage;

    void Awake()
    {
        myImage = GetComponent<Image>();
    }

    public void StartTransparent(float time)
    {
        myImage.color = Color.clear;
        StartCoroutine("transparency", time);
    }

    private IEnumerator transparency(float time)
    {
        float currentTime = 0f;
        float maxTime = time;

        while(currentTime < maxTime) {

            currentTime += Time.deltaTime;

            myImage.color = Color.Lerp(Color.clear, Color.white, currentTime/maxTime);

            yield return null;
        }

        myImage.color = Color.white;

    }

}
