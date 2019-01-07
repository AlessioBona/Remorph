using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPlayerEditor : MonoBehaviour {

    bool dragging = false;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && dragging)
        {

            Touch touch_0 = Input.GetTouch(0);


            if (touch_0.phase == TouchPhase.Moved)
            {
                gameObject.transform.position += new Vector3(touch_0.deltaPosition.x, touch_0.deltaPosition.y, 0f);
            }


        }
    }

    public void dragOn()
    {
        dragging = true;
        Debug.Log("drag on");
    }

    public void dragOff()
    {
        dragging = false;
        Debug.Log("drag off");
    }

}
