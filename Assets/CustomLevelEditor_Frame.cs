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

    }

    float scaleUnit = 61.5f;
    List<List<GameObject>> linesRows = new List<List<GameObject>>();

    private void InstantiateSquares(int rows, int cols)
    {
        for (int r = 0; r < rows; r++)
        {
            
            List<GameObject> newRow = new List<GameObject>();
            linesRows.Add(newRow);

            for (int c = 0; c < cols; c++)
            {
                Vector3 newPosition = new Vector3(0f, 0f, 0f);
                //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
                //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
                newPosition.x += + c * scaleUnit * 2;
                newPosition.y += - r * scaleUnit * 2;
                GameObject newSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
                newSquare.transform.localPosition = newPosition;
                newSquare.transform.localScale = new Vector3(3f, 3f, 0f);
                linesRows[r].Add(newSquare);
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
        Debug.Log("clicked on: " + clickedSquare.rowNumber + " " + clickedSquare.colNumber);
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
                    linesRows[r][c].SetActive(true);
                } else
                {
                    linesRows[r][c].SetActive(false);
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

    public void ImportLevelInfos()
    {
        // va connesso ai files in json
        LevelInfo thisLevelInfos = JsonConvert.DeserializeObject<LevelInfo>(assetToLoad.text);
    }

    public void SaveLevelInfos()
    {
        // va connesso!!!
        string serializedNewLevel = JsonConvert.SerializeObject(thisLevelInfos, Formatting.Indented);
        File.WriteAllText(dataPath + "PROVA.txt", serializedNewLevel);
    }



}
