using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class BorderLiner : MonoBehaviour {

    List<GameObject> lines = new List<GameObject>();
    public bool renderIt;


    private int[][] PlayerGrid = new int[][] {
        new int[]{ 1, 0, 0, 0, 0, 1},
        new int[]{ 1, 0, 1, 1, 0, 1},
        new int[]{ 1, 0, 1, 0, 0, 1},
        new int[]{ 0, 0, 1, 0, 1, 1},
        new int[]{ 1, 0, 0, 0, 1, 1},
        new int[]{ 1, 1, 1, 0, 0, 1},
    };

    int[][] CreateKnotGrid (int[][] BWgrid)
    {
        int[][] knotGrid = new int[BWgrid.Length + 1][];

        for (int i = 0; i < knotGrid.Length; i++)
        {
            knotGrid[i] = new int[BWgrid[0].Length + 1];

            for (int e = 0; e < knotGrid[i].Length; e++)
            {
                int leftUp, rightUp, leftDown, rightDown;



                if (i == 0)
                {
                    leftUp = 0;
                    rightUp = 0;
                }
                else
                {
                    if (e == 0)
                    {
                        leftUp = 0;
                    }
                    else
                    {

                        leftUp = BWgrid[i - 1][e - 1];

                    }
                    if (e == knotGrid[i].Length -1)
                    {
                        rightUp = 0;
                    }
                    else
                    {
                        rightUp = BWgrid[i - 1][e];
                    }
                    
                }

                if (i == knotGrid.Length -1)
                {
                    leftDown = 0;
                    rightDown = 0;
                }
                else
                {
                    if (e == 0)
                    {
                        leftDown = 0;
                    }
                    else
                    {
                        leftDown = BWgrid[i][e - 1];
                    }

                    if (e == knotGrid[i].Length -1)
                    {
                        rightDown = 0;
                    }
                    else
                    {
                        rightDown = BWgrid[i][e];
                    }
                }


                int key = 90000 + leftUp * 1000 + rightUp * 100 + leftDown * 10 + rightDown;
                knotGrid[i][e] = key;

            }

        }

        return knotGrid;
    }


	// Use this for initialization
	void Start () {
		
	}
	
    // Update is called once per frame
	void Update () {
        if (renderIt)
        {

            int[][] newKnotGrid = CreateKnotGrid(PlayerGrid);
            List<Vector3[]> contour = CreateLines(newKnotGrid);
            RenderContour(contour);
        }
	}

    List<Vector3[]> CreateLines(int[][] knotGrid)
    {
        Vector3 startPoint = new Vector3(-2f, 2f, 0f);
        float abstand = 0.5f;

        List<Vector3[]> toDraw = new List<Vector3[]>();

        bool lineStarted = false;

        for(int i = 0; i < knotGrid.Length; i++)
        {
            Vector3[] newLine = new Vector3[2];

            for(int e = 0; e < knotGrid[i].Length; e++)
            {
                int aK = knotGrid[i][e];
                if(
                    aK == 90001 ||
                    aK == 90010 ||
                    aK == 90100 ||
                    aK == 91000 ||
                    aK == 91110 ||
                    aK == 91101 ||
                    aK == 91011 ||
                    aK == 90111
                    )
                {
                    if (!lineStarted)
                    {
                        lineStarted = true;
                        newLine = new Vector3[2];
                        newLine[0] = new Vector3(e*abstand, -i*abstand) + startPoint;
                    } else if (lineStarted)
                    {
                        lineStarted = false;
                        newLine[1] = new Vector3(e * abstand, -i * abstand) + startPoint;
                        toDraw.Add(newLine);
                    }
                }
            }

        }

        // VERTICAL

        for (int i = 0; i < knotGrid[0].Length; i++)
        {
            Vector3[] newLine = new Vector3[2];

            for (int e = 0; e < knotGrid.Length; e++)
            {
                int aK = knotGrid[e][i];
                if (
                    aK == 90001 ||
                    aK == 90010 ||
                    aK == 90100 ||
                    aK == 91000 ||
                    aK == 91110 ||
                    aK == 91101 ||
                    aK == 91011 ||
                    aK == 90111
                    )
                {
                    if (!lineStarted)
                    {
                        lineStarted = true;
                        newLine = new Vector3[2];
                        newLine[0] = new Vector3(i * abstand, -e * abstand) + startPoint;
                    }
                    else if (lineStarted)
                    {
                        lineStarted = false;
                        newLine[1] = new Vector3(i * abstand, -e * abstand) + startPoint;
                        toDraw.Add(newLine);
                    }
                }
            }

        }



        return toDraw;
    }

    void RenderContour(List<Vector3[]> toDraw)
    {
        foreach (GameObject line in lines){
            GameObject.DestroyImmediate(line);
        }

        lines = new List<GameObject>();

        foreach (Vector3[] lineToDraw in toDraw)
        {
            RenderLine(lineToDraw[0], lineToDraw[1]);
        }

     
    }

    void RenderLine(Vector3 start, Vector3 stop)
    {
        GameObject gObject = new GameObject("MyGameObject");

        lines.Add(gObject);

        gObject.transform.SetParent(this.transform);
        LineRenderer lRend = gObject.AddComponent<LineRenderer>();

        lRend.startColor = Color.red;
        lRend.endColor = Color.red;
        lRend.numCapVertices = 10;
        lRend.material = new Material(Shader.Find("Particles/Additive"));
        lRend.startWidth = .1f;
        lRend.endWidth = .1f;
        lRend.SetPosition(0, start);
        lRend.SetPosition(1, stop);
    }
}
