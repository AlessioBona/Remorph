using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoundManager : MonoBehaviour {

    public Camera activeCamera;
    public GameObject boardBG;

    public GameObject player;
    public GameObject endCopy;
    public GameObject reference;
    public NewPlayer playerScript;
    public GameObject LevEdiObject;
    NewLevelEditor levelEditor;
    Sprite[] sprites;
    Color[] spriteColors;
    Color transparent;
    public Color endCopyColor;
    public Color referenceColor;


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

        sprites = levelEditor.sprites;
        spriteColors = levelEditor.spriteColors;
        transparent = levelEditor.transparent;
        rows = levelEditor.rows;
        columns = levelEditor.columns;
        playerScript.columns = columns;
        playerScript.rows = rows;

        Reset();
        ComposeLevel();
    }

    void ComposeLevel()
    {
        //set board Dimension
        boardBG.transform.localScale = new Vector3(
            (columns * 200f)*1.2f,
            (rows * 200f)*1.2f,
            0f);

        //create EndCopy
        endCopy = Instantiate(player);
        endCopy.GetComponent<NewPlayer>().isPlayer = false;
        SpriteRenderer[] endRend = endCopy.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer rend in endRend)
        {
            rend.color = endCopyColor;
            rend.sortingOrder = rend.sortingOrder - 1;

        }

        Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

        foreach (Square2D square in allSquares)
        {
            if (square.colNumber == columns-1 && square.rowNumber == rows-1)
            {
                endCopy.transform.position = square.transform.position;
                endCopy.GetComponent<NewPlayer>().moveTo = square.transform.position;

            }

        }

        //create Reference
        reference = Instantiate(player);
        reference.GetComponent<NewPlayer>().isPlayer = false;
        SpriteRenderer[] refRend = reference.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in refRend)
        {
            rend.color = transparent;
            rend.sortingOrder = rend.sortingOrder - 2;
        }
        


    }

    void SwitchReferenceColor(Color color)
    {
        SpriteRenderer[] refRend = reference.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer rend in refRend)
        {
            rend.color = color;
        }
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
                player.GetComponent<NewPlayer>().moveTo = square.transform.position;
                player.transform.position = square.transform.position;
                player.GetComponent<NewPlayer>().moveTo = square.transform.position;
                player.GetComponent<NewPlayer>().canMove = true;
                player.GetComponent<NewPlayer>().oldTrans = null;


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

            if (Input.touchCount > 1)
            {
                Touch touch_1 = Input.GetTouch(1);
                if (touch_1.phase == TouchPhase.Began)
                {
                    Debug.Log("two");
                    if (iconsOn)
                    {
                        ToggleZoom(false);
                        //TurnIconsOff();
                        //instructions.SetActive(true);
                        //reference.transform.position = player.transform.position;
                        //reference.SetActive(true);
                        //theCamera.transform.position = new Vector3(player.transform.position.x, theCamera.transform.position.y, player.transform.position.z);
                        //theCamera.orthographicSize = 6f;
                    }
                    else
                    {
                        ToggleZoom(true);
                        //instructions.SetActive(false);
                        //updateIcons();
                        //reference.SetActive(false);
                        //theCamera.transform.position = cameraOriginal;
                        //theCamera.orthographicSize = originalZoom;
                    }
                    iconsOn = !iconsOn;


                }

            }

        }
    }

    void ToggleZoom(bool on)
    {
        Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

        if (on)
        {
            SwitchReferenceColor(transparent);
            foreach (Square2D square in allSquares)
            {
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
        if (!on) {
            foreach (Square2D square in allSquares)
            {
                square.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
                SwitchReferenceColor(referenceColor);
                reference.transform.position = playerScript.moveTo;
                
            }

        }
    }
}
