using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]


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

    private CustomLevelEditor_Frame levelEditorFrame;
    private int[][] squareMatrix;
    private int[][] squareColorMatrix;


    // Use this for initialization
    void Awake () {

        levelEditorFrame = FindObjectOfType<CustomLevelEditor_Frame>();
        ImportData();
        SetSquares();
        SetSymbols();
        CreatePlayerAndShadow();


	}

    private void ImportData()
    {
        var data = levelEditorFrame.thisLevelInfos;
        columns = data.cols;
        rows = data.rows;

        squareMatrix = data.squareMatrix;
        squareColorMatrix = data.squareColorMatrix;
    }

    private void CreatePlayerAndShadow()
    {
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

        roundManager.ComposeLevel(columns, rows);
    }

    private void SetSymbols()
    {
        Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();
        Color toChange = allSquares[0].GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color;

        foreach (Square2D square in allSquares)
        {
            
            int matrixValue = squareMatrix[square.colNumber][square.rowNumber];
            int colorMatrixValue = squareColorMatrix[square.colNumber][square.rowNumber];

            if (matrixValue >= 0 && matrixValue <= 17)
            {
                square.isOn = true;
                square.icona = (Square2D.Icon)matrixValue;
                square.colore = (Square2D.Colo)colorMatrixValue;
                square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = sprites[matrixValue];
                square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[colorMatrixValue];
            }
            else
            {
                switch (matrixValue)
                {
                    case 999:
                        square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = null;
                        square.isOn = false;
                        break;
                    case 666:
                        square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = null;
                        toChange = square.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color;
                        toChange.a = .3f;
                        square.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color = toChange;
                        square.isOn = false;
                        //invisibile!
                        break;
                    case 100:
                        player.transform.position = square.transform.position;
                        break;
                    case 101:
                        GameObject.FindGameObjectWithTag("endCopy").transform.position = square.transform.position;
                        break;
                    default:

                        break;

                }

            }

            //if (square.colNumber == startCol && square.rowNumber == startRow)
            //{
            //    player.transform.position = square.transform.position;
            //}
            //if (square.colNumber == endCol && square.rowNumber == endRow)
            //{
            //    GameObject.FindGameObjectWithTag("endCopy").transform.position = square.transform.position;
            //}


            //if (square.isOn)
            //{
            //    square.iconSprite.GetComponent<SpriteRenderer>().sprite = sprites[(int)square.icona];
            //    square.iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[(int)square.colore];
            //}
            //if (!square.isOn)
            //{
            //    square.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
            //}
        }
    }

    private void SetSquares()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("square");
        for (int d = 0; d < toDestroy.Length; d++)
        {
            GameObject.Destroy(toDestroy[d]);
        }

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
                    newSquare.transform.localScale *= .1f;
                    linesRow[r].Add(newSquare);
                    newSquare.GetComponent<Square2D>().rowNumber = rows - 1 - r;
                    newSquare.GetComponent<Square2D>().colNumber = c;
                }
            }
    }

    // Update is called once per frame
    void Update () {

        if (changeSquaresBG)
        {
            Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

            foreach (Square2D square in allSquares)
            {
                square.backGround.GetComponent<SpriteRenderer>().sprite = squarePrefab.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().sprite;
                square.backGround.GetComponent<SpriteRenderer>().color = squarePrefab.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color;
            }

        }

        if (resetSquares)
        {
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("square");
            for (int d = 0; d < toDestroy.Length; d++)
            {
                GameObject.DestroyImmediate(toDestroy[d]);
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

            
        }

        if (resetSymbols) 
        {

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

            roundManager.ComposeLevel(columns, rows);

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
