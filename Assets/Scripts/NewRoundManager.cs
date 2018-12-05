using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class NewRoundManager : MonoBehaviour {

    public Camera activeCamera;
    public GameObject boardBG;

    public GameObject player;
    public GameObject endCopy;
    public GameObject reference;
    public GameObject simulacraParent;
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

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera"))
        {
            GameObject.FindGameObjectWithTag("LevelCamera").SetActive(false);
        }
    }

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

        ComposeLevel();
        Reset();

        rayStart = playerScript.RayCaster.localPosition;
    }

    Vector3 rayStart;

    public void CheckWin()
    {
        List<RaycastHit> hitList = new List<RaycastHit>();
        playerScript.RayCaster.localPosition = rayStart;

        for (int i = 0; i < 8; i++)
        {
            playerScript.RayCaster.localPosition += new Vector3(+.5f, 0f, 0f);

            for (int a = 0; a < 8; a++)
            {
                RaycastHit hit;
                
                playerScript.RayCaster.localPosition += new Vector3(0f, +.5f, 0f);
                Debug.DrawRay(playerScript.RayCaster.position + new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 10f), Color.green);
                if (Physics.Raycast(playerScript.RayCaster.position + new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 2f), out hit)){
                    hitList.Add(hit);
                } 
            }

            playerScript.RayCaster.localPosition += new Vector3(0f, -4f, 0f);
        }

        int end = 0;
        int pla = 0;

        foreach (RaycastHit hit in hitList)
        {
            if (hit.transform.parent.tag == "endCopy")
                {
                end += 1;
                }
            if (hit.transform.parent.tag == "Player")
                {
                pla += 1;
                }
        }

        Debug.Log("player rays: " + pla + " - end rays: " + end);

    }

    void ComposeLevel()
    {
        //set board Dimension
        boardBG.transform.localScale = new Vector3(
            (columns * 200f) + 80f,
            (rows * 200f) + 80f,
            0f);

        //create EndCopy
        endCopy = Instantiate(player);
        endCopy.tag = "endCopy";
        endCopy.transform.SetParent(simulacraParent.transform);
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

        endCopy.transform.position += new Vector3(0f, 0f, -1f);

        //create Reference
        reference = Instantiate(player);
        reference.transform.SetParent(simulacraParent.transform);
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

    public void Reset()
    {
        Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

        foreach (Square2D square in allSquares)
        {
            if (square.colNumber == 0 && square.rowNumber == 0)
            {
                playerScript.moveTo = square.transform.position;
                player.transform.position = square.transform.position;
                reference.GetComponent<NewPlayer>().moveTo = square.transform.position;
                reference.transform.position = square.transform.position;
                playerScript.canMove = true;
                playerScript.oldTrans = null;


                playerScript.xPos = 0;
                playerScript.yPos = 0;

                playerScript.Reset();
                playerScript.MoveUpForRaycast();
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
                        TurnIconsOn(false);
                        UIManager.ShowUiElement("InGame", "MyUI");
                        //TurnIconsOff();
                        //instructions.SetActive(true);
                        //reference.transform.position = player.transform.position;
                        //reference.SetActive(true);
                        //theCamera.transform.position = new Vector3(player.transform.position.x, theCamera.transform.position.y, player.transform.position.z);
                        //theCamera.orthographicSize = 6f;
                    }
                    else
                    {
                        TurnIconsOn(true);
                        UIManager.HideUiElement("InGame", "MyUI");
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

    void TurnIconsOn(bool on)
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
