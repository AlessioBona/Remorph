using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEditor : MonoBehaviour {

    [SerializeField]
    EditorBlock[] blocks;

    EditorDot[][] block_dots;

    Camera mainCamera;

    private Plane gridPlane;

    private void Awake()
    {
        gridPlane = new Plane(-Vector3.forward, Vector3.zero);

        mainCamera = FindObjectOfType<Camera>();
        blocks = GetComponentsInChildren<EditorBlock>();
        block_dots = new EditorDot[blocks.Length][];
        for(int i = 0; i < blocks.Length; i++)
        {
            block_dots[i] = blocks[i].GetComponentsInChildren<EditorDot>();
        }

        
    }

    Vector3 dotsStartPoint = new Vector3(-200f, 200f);
    Vector3 gridStartPoint = new Vector3(-240f, 240f);


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0)
        {
            Touch touch_0 = Input.GetTouch(0);

            if(touch_0.phase == TouchPhase.Began)
            {
                float dist;
                Ray ray = mainCamera.ScreenPointToRay(touch_0.position);
                gridPlane.Raycast(ray, out dist);
                Vector3 position = ray.GetPoint(dist);

                Debug.Log(position);
                int x = 99;
                int y = 99;

                if (position.x >= -7.5f && position.x <= 7.5f &&
                    position.y >= -7.5f && position.y <= 7.5f)
                {
                    // + / - 7.5 for the whole grid, 2.5 every cell
                    for (int i = 0; i < 6; i++)
                    {
                        if (position.x > i * 2.5f - 7.5f)
                            x = i;
                    }

                    for (int i = 0; i < 6; i++)
                    {
                        if (position.y < 7.5f - i * 2.5f)
                            y = i;
                    }

                } // end of "ifOnTheGrid"

                Debug.Log(x + " " + y);

                block_dots[0][0].transform.position = new Vector3(-7.5f+(x*2.5f+1.25f), 7.5f-(y*2.5f+1.25f));

            }
        }
	}


}
