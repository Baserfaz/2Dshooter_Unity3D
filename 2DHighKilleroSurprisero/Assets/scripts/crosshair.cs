using UnityEngine;
using System.Collections;

public class crosshair : MonoBehaviour {


    void Start()
    {
        Cursor.visible = false;
    }

	void Update () {

        Vector3 mousepos = Input.mousePosition;

        mousepos.z = 0f;

        transform.position = mousepos;

	}
}
