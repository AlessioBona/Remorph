using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class NewRoundManager : MonoBehaviour {

    public Camera activeCamera;
    public float initialCameraSize;
    public float zoomedCameraSize;
    public float zoomTime;
    public float zoomDistance;
    public bool closeCamera = false;
    //three variables are hard coded so far

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

    public int startCol;
    public int startRow;

    public int endCol;
    public int endRow;

    public List<List<int[]>> squaresData = new List<List<int[]>>();

    Vector2 touchPositionStart;

    bool iconsOn = true;
    bool youWon = false;

    private void Awake()
    {
        activeCamera = GameObject.FindGameObjectWithTag("LevelCamera").GetComponent<Camera>();


        if (GameObject.FindGameObjectWithTag("MainCamera"))
        {
            GameObject.FindGameObjectWithTag("LevelCamera").SetActive(false);
            activeCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        initialCameraSize = 18f;
        zoomedCameraSize = 6f;
        zoomTime = .4f;
        activeCamera.gameObject.transform.position = new Vector3(0f, 0f, -10f);
        activeCamera.orthographicSize = initialCameraSize;
    }

    // Use this for initialization
    void Start () {



        levelEditor = LevEdiObject.GetComponent<NewLevelEditor>();
        levelEditor.changeSquaresBG = false;
        levelEditor.resetSymbols = false;
        playerScript = player.GetComponent<NewPlayer>();
        playerScript.squaresParent = squaresParent;

        sprites = levelEditor.sprites;
        spriteColors = levelEditor.spriteColors;
        transparent = levelEditor.transparent;

        rows = levelEditor.rows;
        columns = levelEditor.columns;
        playerScript.columns = columns;
        playerScript.rows = rows;

        startRow = levelEditor.startRow;
        startCol = levelEditor.startCol;
        playerScript.startCol = startCol;
        playerScript.startRow = startRow;

        endCol = levelEditor.endCol;
        endRow = levelEditor.endRow;
        playerScript.endCol = endCol;
        playerScript.endRow = endRow;

        rows = levelEditor.rows;
        columns = levelEditor.columns;
        playerScript.columns = columns;
        playerScript.rows = rows;


        //ComposeLevel(columns, rows);
        reference = GameObject.FindGameObjectWithTag("reference");
        endCopy = GameObject.FindGameObjectWithTag("endCopy");
        Reset();

        // ray at beginning
        rayStart = playerScript.RayCaster.localPosition;
        CreateRayArray();
    }

    Vector3 rayStart;
    List<bool> raySave = new List<bool>();

    public void CreateRayArray()
    {
        playerScript.RayCaster.localPosition = rayStart;

        for (int i = 0; i < 8; i++)
        {
            playerScript.RayCaster.localPosition += new Vector3(+.5f, 0f, 0f);

            for (int a = 0; a < 8; a++)
            {
                RaycastHit hit;

                playerScript.RayCaster.localPosition += new Vector3(0f, +.5f, 0f);
                Debug.DrawRay(playerScript.RayCaster.position + new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 10f), Color.green);

                if (Physics.Raycast(playerScript.RayCaster.position + new Vector3(0f, 0f, -5f), new Vector3(0f, 0f, 2f), out hit))
                {

                    raySave.Add(true);
                }
                else
                {
                    raySave.Add(false);
                }
            }

            playerScript.RayCaster.localPosition += new Vector3(0f, -4f, 0f);
        }
    }

    // for debug
    private void PrintSquareArray(List<bool> array)
    {
        string toPrint = "";
        for(int i = 0; i < array.Count; i++)
        {
            if(array[i])
                toPrint += "1 ";
            if (!array[i])
                toPrint += "0 ";
        }
        Debug.Log(toPrint);
    }

    public void CheckWin()
    {

        List<bool> rayCheck = new List<bool>();

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
                    rayCheck.Add(true);
                }  else
                {
                    rayCheck.Add(false);
                }
            }

            playerScript.RayCaster.localPosition += new Vector3(0f, -4f, 0f);
        }

        int end = 0;
        int pla = 0;

        foreach (RaycastHit hit in hitList)
        {
            if (hit.transform.parent.parent.tag == "endCopy")
                {
                end += 1;
                }
            if (hit.transform.parent.parent.tag == "Player")
                {
                pla += 1;
                }
        }

        Debug.Log("player rays: " + pla + " - end rays: " + end);

        for (int i=0; i<raySave.Count; i++)
        {
            if(raySave[i] != rayCheck[i])
            {
                Debug.Log("IT'S WROOONG!!!");
                return;
            }
        }
        if(end > 0)
        {
            Debug.Log("IT'S WROOONG!!!");
            return;
        }
        // IF IT RETURNS, NOTHING AFTER THIS LINE IS PLAYED!
        Debug.Log("I WOOOOON!!!");

        // THIS IS THE NEW PART

        TurnIconsOn(false);
        zoomDistance = Mathf.Abs(Vector3.Distance(activeCamera.gameObject.transform.position, player.transform.position + new Vector3(0f, 0f, -10f)));
        UIManager.ShowUiElement("YouWon", "MyUI");
        //activeCamera.gameObject.transform.position = player.transform.position + new Vector3(0f, 0f, -10f);
        closeCamera = true;
        youWon = true;

        // THIS ONE

    }

    public void ComposeLevel(int c, int r)
    {
        //set board Dimension
        boardBG.transform.localScale = new Vector3(
            (c * 200f) + 80f,
            (r * 200f) + 80f,
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
        reference.tag = "reference";
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

    private void FixedUpdate()
    {
        ZoomCamera();
    }

    void ZoomCamera()
    {

        // calcola distanza, dividila per coefficiente

        if(closeCamera && activeCamera.orthographicSize > zoomedCameraSize)
        {
            float zoomDiff = Mathf.Abs(initialCameraSize - zoomedCameraSize);
            activeCamera.orthographicSize -= (zoomDiff / zoomTime) * Time.deltaTime;
        }

        if(closeCamera && activeCamera.gameObject.transform.position != player.transform.position + new Vector3(0f, 0f, -10f))
        {
            //float zoomDiff = Mathf.Abs(Vector3.Distance(activeCamera.gameObject.transform.position, player.transform.position + new Vector3(0f, 0f, -10f)));

            activeCamera.gameObject.transform.position = Vector3.MoveTowards(
                activeCamera.gameObject.transform.position, player.transform.position + new Vector3(0f, 0f, -10f), (zoomDistance / zoomTime)* 1f * Time.deltaTime);
        }

        if (!closeCamera && activeCamera.orthographicSize < initialCameraSize)
        {
            float zoomDiff = Mathf.Abs(initialCameraSize - zoomedCameraSize);
            activeCamera.orthographicSize += (zoomDiff / zoomTime) * Time.deltaTime;
        }

        if (!closeCamera && activeCamera.gameObject.transform.position != new Vector3(0f, 0f, -10f))
        {
            //float zoomDiff = Mathf.Abs(Vector3.Distance(player.transform.position + new Vector3(0f, 0f, -10f), new Vector3(0f, 0f, -10f)));

            activeCamera.gameObject.transform.position = Vector3.MoveTowards(
                 activeCamera.gameObject.transform.position, new Vector3(0f, 0f, -10f), (zoomDistance / zoomTime) * 1f * Time.deltaTime);
        }


    }

    public void Reset()
    {

        closeCamera = false;

        Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

        foreach (Square2D square in allSquares)
        {
            if (square.colNumber == startCol && square.rowNumber == startRow)
            {
                playerScript.moveTo = square.transform.position;
                playerScript.prevPosition = square.transform.position;
                player.transform.position = square.transform.position;
                reference.GetComponent<NewPlayer>().moveTo = square.transform.position;
                reference.transform.position = square.transform.position;

                playerScript.xPos = startCol;
                playerScript.yPos = startRow;
                TurnIconsOn(true);
                playerScript.Reset();
                playerScript.MoveUpForRaycast();

                playerScript.canMove = true;
                playerScript.oldTrans = null;
            }
            
        }
    }


    float touchTime;

    void Touch()
    {
        if (Input.touchCount > 0 && !youWon)
        {

            Touch touch_0 = Input.GetTouch(0);
            

            if (touch_0.phase == TouchPhase.Began)
            {
                touchPositionStart = touch_0.position;
                touchTime = Time.time;
            }

            if (
                Input.touchCount == 1 
                && touch_0.phase == TouchPhase.Stationary 
                && Time.time - touchTime > .7f 
                && Mathf.Abs(Vector2.Distance(touchPositionStart,touch_0.position)) < 10f
                && iconsOn
                )
            {
                
                touchTime = Time.time + 100f;
                playerScript.ReverseTransformation();
            }

            if (touch_0.phase == TouchPhase.Ended && Input.touchCount == 1)
            {
                Vector2 touchPositionDifference = touchPositionStart - touch_0.position;

                if (Mathf.Abs(touchPositionDifference.x) > 100f || Mathf.Abs(touchPositionDifference.y) > 100f)
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
                        //if (!iconsOn)
                        //{
                        //    if (touchPositionDifference.x > 0)
                        //    {
                        //        //reference.SetActive(false);
                        //        //theCamera.transform.position = cameraOriginal;
                        //        //theCamera.orthographicSize = originalZoom;
                        //        Reset();
                        //    }
                        //}
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

            if (touch_0.tapCount > 1 && !justDoubleTapped) { 
            Debug.Log("two");
                justDoubleTapped = true;
                StartCoroutine(youCanDoubleTap());
                if (iconsOn)
                    {
                        TurnIconsOn(false);
                        zoomDistance = Mathf.Abs(Vector3.Distance(activeCamera.gameObject.transform.position, player.transform.position + new Vector3(0f, 0f, -10f)));
                        UIManager.ShowUiElement("InGame", "MyUI");
                        //activeCamera.gameObject.transform.position = player.transform.position + new Vector3(0f, 0f, -10f);
                        closeCamera = true;
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
                        closeCamera = false;
                        //instructions.SetActive(false);
                        //updateIcons();
                        //reference.SetActive(false);
                        //theCamera.transform.position = cameraOriginal;
                        //theCamera.orthographicSize = originalZoom;
                    }

            }

        }
    }

    bool justDoubleTapped = false;

    IEnumerator youCanDoubleTap()
    {
        yield return new WaitForSeconds(1f);
        justDoubleTapped = false;
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
            iconsOn = true;
        }

        if (!on) {
            foreach (Square2D square in allSquares)
            {
                square.iconSprite.GetComponent<SpriteRenderer>().color = transparent;
                SwitchReferenceColor(referenceColor);
                reference.transform.position = playerScript.moveTo;
                
            }
            iconsOn = false;

        }
    }
}
