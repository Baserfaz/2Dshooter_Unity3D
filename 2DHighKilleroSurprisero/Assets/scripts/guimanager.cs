using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class guimanager : MonoBehaviour {

    public GameObject guiGrenade;
    public GameObject guiHealthBar;
    public GameObject guiAmmoCount;

    public void UpdateAmmoCount(int ammo)
    {
        guiAmmoCount.GetComponent<Text>().text = ammo + "";
    }

    public void UpdateHealthBar(float hp, float maxhp)
    {
        guiHealthBar.GetComponent<Slider>().value = hp / maxhp;
    }

}
