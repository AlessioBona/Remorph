using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;

public class CustomLevelEditor_Frame : MonoBehaviour {

    [SerializeField]
    TextAsset assetToLoad;

    LevelInfo thisLevelInfos;

    [SerializeField]
    GameObject squarePrefab;

    [SerializeField]
    public Transform squaresParent;

    [SerializeField]
    CustomLevelEditor_SelectIcon selectIcon;

    GameObject selectedSquare = null;

    string dataPath;

    int maxRows = 8;
    int maxCols = 4;

    private void Awake()
    {
        selectIcon = GetComponentInChildren<CustomLevelEditor_SelectIcon>();
        selectIcon.gameObject.SetActive(false);
        BlankLevelInfos();
        InstantiateSquares(maxRows, maxCols);

        SetDataPath();
        SetSquaresFromLevelInfo();
        ActivateSquares();

    }

    float scaleUnit = 61.5f;
    List<List<GameObject>> linesRows = new List<List<GameObject>>();

    private void InstantiateSquares(int rows, int cols)
    {
        for (int c = 0; c < cols; c++)
        {
            
            List<GameObject> newCol = new List<GameObject>();
            linesRows.Add(newCol);

            for (int r = 0; r < rows; r++)
            {
                Vector3 newPosition = new Vector3(0f, 0f, 0f);
                //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
                //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
                newPosition.x += + c * scaleUnit * 2;
                newPosition.y += - r * scaleUnit * 2;
                GameObject newSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
                newSquare.transform.localPosition = newPosition;
                newSquare.transform.localScale = new Vector3(3f, 3f, 0f);
                linesRows[c].Add(newSquare);
                newSquare.GetComponent<Square2D>().rowNumber = r;
                newSquare.GetComponent<Square2D>().colNumber = c;
                Button thisButton = newSquare.gameObject.AddComponent<Button>();

                thisButton.onClick.AddListener( () => SquareClicked( newSquare.GetComponent<Square2D>() ) );
            }
        }
    }



    public void SquareClicked(Square2D clickedSquare)
    {
        selectedSquare = clickedSquare.gameObject;
        squaresParent.gameObject.SetActive(false);
        selectIcon.gameObject.SetActive(true);
        Debug.Log("clicked on: " + clickedSquare.colNumber + " " + clickedSquare.rowNumber);
    }

    private void ActivateSquares()
    {
        Vector3 xOff = new Vector3(35f, 0f, 0f);

        int cols = thisLevelInfos.cols;
        int rows = thisLevelInfos.rows;

        Vector3 newPosition = new Vector3(0f, 0f, 0f);
        newPosition.x += (cols - 1) * -scaleUnit;
        newPosition.y += (rows - 1) * scaleUnit;

        squaresParent.transform.localPosition = newPosition + xOff;

        for (int r = 0; r < maxRows; r++)
        {
            for (int c = 0; c < maxCols; c++)
            {
                if (r < thisLevelInfos.rows && c < thisLevelInfos.cols)
                {
                    linesRows[c][r].SetActive(true);
                } else
                {
                    linesRows[c][r].SetActive(false);
                }
            }
        }
    }




    private void RefreshLayout()
    {
        ActivateSquares();
        Debug.Log("REFRESHHHH!");
    }

    public void AugmentRows()
    {
        if (thisLevelInfos.rows < maxRows)
        {
            thisLevelInfos.rows++;
            
            RefreshLayout();
        }
    }

    public void AugmentCols()
    {
        if (thisLevelInfos.cols < maxCols)
        {
            thisLevelInfos.cols++;
            
            RefreshLayout();
        }
    }

    public void DiminishRows()
    {
        if(thisLevelInfos.rows > 1)
        {
            thisLevelInfos.rows--;

            RefreshLayout();
        }
    }

    public void DiminishCols()
    {
        if (thisLevelInfos.cols > 1)
        {
            thisLevelInfos.cols--;

            RefreshLayout();
        }
    }

    private void SetDataPath()
    {
        if (Application.isPlaying && !Application.isEditor)
        {
            dataPath = Application.persistentDataPath + "/";
        }

        if (Application.isPlaying && Application.isEditor)
        {
            dataPath = Application.dataPath + "/Resources/CustomLevels/";
        }
    }

    public void BlankLevelInfos()
    {
        thisLevelInfos = new LevelInfo();
    }

    public LevelInfo ImportLevelInfos()
    {
        // va connesso ai files in json
        LevelInfo levelInfos = JsonConvert.DeserializeObject<LevelInfo>(assetToLoad.text);
        return levelInfos;
    }

    public Sprite[] iconSprites;

    public void SetSquaresFromLevelInfo()
    {
        thisLevelInfos = null;

        LevelInfo levelInfo = ImportLevelInfos();
        if (levelInfo != null)
        {
            thisLevelInfos = levelInfo;
        }

        for (int a = 0; a < linesRows.Count; a++)
            {
                for (int b = 0; b < linesRows[a].Count; b++)
                {
                    int thisInt = thisLevelInfos.squareMatrix[a][b];

                    UpdateSquare(thisInt, thisLevelInfos.squareColorMatrix[a][b], linesRows[a][b]);

                
                }
            

        }
    }


    

    public void UpdateSquare(int matrixValue, int colorMatrixValue, GameObject square = null)
    {
        if(square == null)
        {
            square = selectedSquare;
        }

        Color toChange = square.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color;
        toChange.a = 1f;
        square.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color = toChange;

        if (matrixValue >= 0 && matrixValue <= 17)
        {

            square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = selectIcon.iconSprites[matrixValue];
            square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().color = selectIcon.spriteColors[colorMatrixValue];

        }
        else
        {

            switch (matrixValue)
            {
                case 999:
                    break;
                case 666:
                    Debug.Log(666);
                    square.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = null;
                    toChange = square.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color;
                    toChange.a = .3f;
                    square.GetComponent<Square2D>().backGround.GetComponent<SpriteRenderer>().color = toChange;
                    //invisibile!
                    break;
                case 100:
                    break;
                case 101:
                    break;
                default:

                    break;

            }

        }

    }

    public void SaveLevelInfos()
    {
        // va connesso!!!
        string serializedNewLevel = JsonConvert.SerializeObject(thisLevelInfos, Formatting.Indented);
        File.WriteAllText(dataPath + "PROVA.txt", serializedNewLevel);
    }



}
