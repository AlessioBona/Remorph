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

    string dataPath;

    int maxRows = 8;
    int maxCols = 4;

    private void Awake()
    {
        selectIcon = GetComponentInChildren<CustomLevelEditor_SelectIcon>();
        selectIcon.gameObject.SetActive(false);
        BlankLevelInfos();
        InstantiateSquares(maxRows, maxCols);
        ActivateSquares();
        SetDataPath();
        SetSquaresFromLevelInfo();

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

    public void UpdateSquare(int selected, int color)
    {
        Debug.Log("update a Square to " + selected + " " + color);
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

        //for(int i = 0; i < 4; i++)
        //{
        //    for(int e = 0; e < 8; e++)
        //    {
        //        Debug.Log(thisLevelInfos.squareMatrix[i][e]);
        //    }
        //}

        //for (int i = 0; i < 4; i++)
        //{
        //    for (int e = 0; e < 8; e++)
        //    {
        //        Debug.Log(thisLevelInfos.squareColorMatrix[i][e]);
        //    }
        //}

        //Debug.Log(thisLevelInfos.levelName);
        //Debug.Log(thisLevelInfos.playerForm[0][0]);

        

        

        for (int a = 0; a < linesRows.Count; a++)
            {
                for (int b = 0; b < linesRows[a].Count; b++)
                {
                    int thisInt = thisLevelInfos.squareMatrix[a][b];

                Debug.Log(linesRows[a][b]);

                if (thisInt >= 0 && thisInt <= 17)
                {

                    linesRows[a][b].GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = selectIcon.iconSprites[thisInt];
                    linesRows[a][b].GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().color = selectIcon.spriteColors[thisLevelInfos.squareColorMatrix[a][b]];

                }
                else
                {

                    switch (thisInt)
                    {
                        case 999:
                            break;
                        case 666:
                            Debug.Log(666);
                            linesRows[a][b].SetActive(false);
                            // il problema è che se poi si riallarga... si riattiva!!!
                            //quindi bisogna inserire lì un controllo!
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
            

        }
    }

    public void SaveLevelInfos()
    {
        // va connesso!!!
        string serializedNewLevel = JsonConvert.SerializeObject(thisLevelInfos, Formatting.Indented);
        File.WriteAllText(dataPath + "PROVA.txt", serializedNewLevel);
    }



}
