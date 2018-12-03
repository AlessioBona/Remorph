using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoundManager : MonoBehaviour {

    public GameObject player;
    public NewPlayer playerScript;
    public GameObject LevEdiObject;
    NewLevelEditor levelEditor;
    Sprite[] sprites;
    Color[] spriteColors;
    Color transparent;


    public GameObject squaresParent;
    public GameObject squarePrefab;
    //public List<GameObject> squaresLine;
    //public List<List<GameObject>> linesRow = new List<List<GameObject>>();
    public int columns;
    public int rows;

    public List<List<int[]>> squaresData = new List<List<int[]>>();

    Vector2 touchPositionStart;
    bool iconsOn = true;

    // Use this for initialization
    void Start () {
        levelEditor = LevEdiObject.GetComponent<NewLevelEditor>();
        playerScript = player.GetComponent<NewPlayer>();
        playerScript.squaresParent = squaresParent;
        playerScript.columns = columns;
        playerScript.rows = rows;
        sprites = levelEditor.sprites;
        spriteColors = levelEditor.spriteColors;
        transparent = levelEditor.transparent;
        rows = levelEditor.rows;
        columns = levelEditor.columns;
        ComposeLevel();
        Reset();
	}

    void ComposeLevel()
    {

        //GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("square");
        //for (int d = 0; d < toDestroy.Length; d++)
        //{
        //    GameObject.DestroyImmediate(toDestroy[d]);
        //}

        ////foreach (List<GameObject> row in linesRow)
        ////{
        ////    foreach (GameObject sq in row)
        ////    {
        ////        GameObject.DestroyImmediate(sq);
        ////    }
        ////}

        //linesRow = new List<List<GameObject>>();

        //for (int r = 0; r < rows; r++)
        //{
        //    List<GameObject> newRow = new List<GameObject>();
        //    linesRow.Add(newRow);

        //    for (int c = 0; c < columns; c++)
        //    {
        //        Vector3 newPosition = new Vector3(0f, 0f, 0f);
        //        newPosition.x += (columns - 1) * -2f + c * 4f;
        //        newPosition.y += (rows - 1) * -2f + r * 4f;
        //        GameObject newSquare = Instantiate(squarePrefab, newPosition, squarePrefab.transform.rotation, squaresParent.transform);
        //        linesRow[r].Add(newSquare);
        //    }
        //}


        // THIS WORKS!!!

        //Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

        //foreach (Square2D square in allSquares)
        //{
        //    if (square.isOn)
        //    {

        //        square.iconSprite.GetComponent<SpriteRenderer>().sprite = sprites[(int)square.icona];
        //        square.iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[(int)square.colore];
        //    }
        //    if (!square.isOn)
        //    {
        //        square.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
        //    }
        //}




            //for (int r = 0; r < linesRow.Count; r++)
            //{
            //    for (int c = 0; c < linesRow[r].Count; c++)
            //      {
            //    Square2D thisSq = linesRow[r][c].GetComponent<Square2D>();
            //    if (squaresData[r][c][0] == 1)
            //    {

            //        thisSq.iconSprite.GetComponent<SpriteRenderer>().sprite = sprites[squaresData[r][c][1]];
            //        thisSq.iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[squaresData[r][c][2]];
            //    }
            //    if (squaresData[r][c][0] == 0)
            //    {
            //        thisSq.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
            //    }
            //}

            //}

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

        }

	
	// Update is called once per frame
	void Update () {
        Touch();
	}

    private void Reset()
    {
        Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

        foreach (Square2D square in allSquares)
        {
            if (square.colNumber == 0 && square.rowNumber == 0)
            {
                player.transform.position = square.transform.position;
                player.GetComponent<NewPlayer>().xPos = 0;
                player.GetComponent<NewPlayer>().yPos = 0;
            }
            
        }
    }


    void Touch()
    {
        if (Input.touchCount > 0)
        {

            Touch touch_0 = Input.GetTouch(0);
            if (touch_0.phase == TouchPhase.Began)
            {
                touchPositionStart = touch_0.position;
            }

            if (touch_0.phase == TouchPhase.Ended)
            {
                Vector2 touchPositionDifference = touchPositionStart - touch_0.position;

                if (Mathf.Abs(touchPositionDifference.x) > 200f || Mathf.Abs(touchPositionDifference.y) > 200f)
                {
                    if (Mathf.Abs(touchPositionDifference.x) > Mathf.Abs(touchPositionDifference.y))
                    {
                        if (iconsOn)
                        {
                            // horizontal
                            if (touchPositionDifference.x > 0)
                            {
                                playerScript.Move(-1, 0);
                            }
                            if (touchPositionDifference.x < 0)
                            {
                                playerScript.Move(1, 0);
                            }
                        }
                        if (!iconsOn)
                        {
                            if (touchPositionDifference.x > 0)
                            {
                                //reference.SetActive(false);
                                //theCamera.transform.position = cameraOriginal;
                                //theCamera.orthographicSize = originalZoom;
                                Reset();
                            }
                        }
                    }
                    if (Mathf.Abs(touchPositionDifference.x) < Mathf.Abs(touchPositionDifference.y))
                    {
                        if (iconsOn)
                        {
                            // vertical
                            if (touchPositionDifference.y > 0)
                            {
                                playerScript.Move(0, -1);
                            }
                            if (touchPositionDifference.y < 0)
                            {
                                playerScript.Move(0, 1);
                            }
                        }
                    }
                }

            }
        }
    }
}
