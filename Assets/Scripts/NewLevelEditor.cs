using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]


public class NewLevelEditor : MonoBehaviour {

    public NewRoundManager roundManager;

    public GameObject player;
    public GameObject squaresParent;
    public GameObject squarePrefab;


    public List<List<GameObject>> linesRow = new List<List<GameObject>>();
    public int columns;
    public int rows;

    public int startCol;
    public int startRow;

    public int endCol;
    public int endRow;

    public bool resetSquares = false;
    public bool changeSquaresBG = false;
    public bool resetSymbols = false;

    public Sprite[] sprites;
    public Color[] spriteColors;
    public Color transparent;


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (changeSquaresBG)
        {
            Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

            foreach (Square2D square in allSquares)
            {
                square.backGround.GetComponent<SpriteRenderer>().sprite = squarePrefab.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().sprite;
            }

        }

        if (resetSquares)
        {
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("square");
            for (int d = 0; d < toDestroy.Length; d++)
            {
                GameObject.DestroyImmediate(toDestroy[d]);
            }

            GameObject[] alsoToDestroy = GameObject.FindGameObjectsWithTag("endCopy");
            for (int d = 0; d < alsoToDestroy.Length; d++)
            {
                GameObject.DestroyImmediate(alsoToDestroy[d]);
            }

            GameObject[] alsoToDestroy2 = GameObject.FindGameObjectsWithTag("reference");
            for (int d = 0; d < alsoToDestroy2.Length; d++)
            {
                GameObject.DestroyImmediate(alsoToDestroy2[d]);
            }


            //foreach (List<GameObject> row in linesRow)
            //{
            //    foreach (GameObject sq in row)
            //    {
            //        GameObject.DestroyImmediate(sq);
            //    }
            //}

            linesRow = new List<List<GameObject>>();

            for (int r = 0; r < rows; r++)
            {
                List<GameObject> newRow = new List<GameObject>();
                linesRow.Add(newRow);

                for (int c = 0; c < columns; c++)
                {
                    Vector3 newPosition = new Vector3(0f, 0f, 0f);
                    newPosition.x += (columns-1) * -2f + c * 4f;
                    newPosition.y += (rows-1) * -2f + r * 4f;
                    GameObject newSquare = Instantiate(squarePrefab, newPosition, squarePrefab.transform.rotation, squaresParent.transform);
                    linesRow[r].Add(newSquare);
                    newSquare.GetComponent<Square2D>().rowNumber = r;
                    newSquare.GetComponent<Square2D>().colNumber = c;
                }
            }

            roundManager.ComposeLevel(columns, rows);
        }

        if (resetSymbols) 
        {

            Debug.Log("I reset symbols");
            //foreach (List<GameObject> row in linesRow)
            //{

            //    foreach (GameObject sq in row)
            //    {

            //        Square2D thisSq = sq.GetComponent<Square2D>();
            //        if (thisSq.isOn)
            //        {
            //            thisSq.iconSprite.GetComponent<SpriteRenderer>().sprite = sprites[(int)thisSq.icona];
            //            thisSq.iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[(int)thisSq.colore];
            //        }
            //        if (!thisSq.isOn)
            //        {
            //            thisSq.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
            //        }

            //    }

            //}

            

            Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

            foreach (Square2D square in allSquares)
            {

                if(square.colNumber == startCol && square.rowNumber == startRow)
                {
                    player.transform.position = square.transform.position;
                }
                if (square.colNumber == endCol && square.rowNumber == endRow)
                {
                    GameObject.FindGameObjectWithTag("endCopy").transform.position = square.transform.position;
                }


                if (square.isOn)
                {
                    square.iconSprite.GetComponent<SpriteRenderer>().sprite = sprites[(int)square.icona];
                    square.iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[(int)square.colore];
                }
                if (!square.isOn)
                {
                    square.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
                }
            }

        }
    }
}
