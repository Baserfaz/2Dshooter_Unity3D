using UnityEngine;
using System.Collections;

public class mouse_controller : MonoBehaviour {

    public GameObject swordDrop;

	void Update () {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            mousePos.z = -1f;
            Instantiate(swordDrop, mousePos, Quaternion.identity);
        }

	}
}
