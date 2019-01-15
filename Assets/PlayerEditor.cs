using System;
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


        for (int i = 0; i < block_dots.Length; i++)
        {
            for (int e = 0; e < block_dots[i].Length; e++)
            {
                SituateDot(block_dots[i][e], block_dots[i][e].coords[0], block_dots[i][e].coords[1]);
            }
        }


    }

    private void SituateDot(EditorDot editorDot, int x, int y)
    {
        editorDot.transform.position = new Vector3(-7.5f + (x * 2.5f + 1.25f), 7.5f - (y * 2.5f + 1.25f));
    }

    Vector3 dotsStartPoint = new Vector3(-200f, 200f);
    Vector3 gridStartPoint = new Vector3(-240f, 240f);


    // Use this for initialization
    void Start () {
		
	}

    EditorDot selectedDot;
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0)
        {
            Touch touch_0 = Input.GetTouch(0);

            if(touch_0.phase == TouchPhase.Began)
            {
                int[] coords;
                bool found = false;
                coords = rayCastForGrid(touch_0);
                if(coords[0] != 99 && coords[1] != 99)
                {
                    selectedDot = null;
                    for(int i = 0; i < block_dots.Length && !found; i++)
                    {
                        for(int e = 0; e < block_dots[i].Length && !found; e++)
                        {
                            if(block_dots[i][e].coords[0] == coords[0] && block_dots[i][e].coords[1] == coords[1])
                            {
                                selectedDot = block_dots[i][e];
                                found = true;
                            }
                        }
                    }
                }
            }
        }
	}

    private int[] rayCastForGrid(Touch touch_0)
    {
        int x = 0;
        int y = 0;
        float dist;
        Ray ray = mainCamera.ScreenPointToRay(touch_0.position);
        gridPlane.Raycast(ray, out dist);
        Vector3 position = ray.GetPoint(dist);

        Debug.Log(position);
        x = 99;
        y = 99;
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

        //block_dots[0][0].transform.position = new Vector3(-7.5f + (x * 2.5f + 1.25f), 7.5f - (y * 2.5f + 1.25f));
        Debug.Log(x + " " + y);

        return new int[] { x, y };
    }
}
