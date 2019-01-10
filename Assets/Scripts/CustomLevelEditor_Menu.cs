using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using Newtonsoft.Json;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

public class CustomLevelEditor_Menu : MonoBehaviour {

    [SerializeField]
    Transform buttonPlaceholder;

    [SerializeField]
    GameObject customLevelButton_prefab;

    [SerializeField]
    Transform grid;

    string dataPath;

    [SerializeField]
    TMP_InputField inputNewLevelName;

    int lastCreatedLevelID = 0;

    List<LevelInfo> customLevelsList;

    void Awake()
    {

        if (Application.isPlaying && !Application.isEditor)
        {
            dataPath = Application.persistentDataPath + "/";
        }

        if (Application.isPlaying && Application.isEditor)
        {
            dataPath = Application.dataPath + "/Resources/CustomLevels/";
        }


        DeactivatePlaceholder();

        UpdateSelectMenu();


    }

    private void DeactivatePlaceholder()
    {
        if (buttonPlaceholder.gameObject.activeSelf)
        {
            buttonPlaceholder.gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateSelectMenu()
    {
        customLevelsList = new List<LevelInfo>();

        ImportCustomLevelsFromAssets();

        ImportCustomLevelsFromSavedFiles();

        for (int i = 0; i < customLevelsList.Count; i++)
        {
            GameObject newButton = Instantiate(customLevelButton_prefab, grid);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = customLevelsList[i].levelName;
        }

        //TextAsset[] assetsArray = CreateCustomFromAssetsArray();

        //for(int i = 0; i < assetsArray.Length; i++)
        //{
        //    GameObject newButton = Instantiate(customLevelButton_prefab, grid);
        //    newButton.GetComponentInChildren<TextMeshProUGUI>().text = assetsArray[i].name;
        //}

        //string[] savedFilesArray = CreateSavedFilesArray();

        //for (int i = 0; i < savedFilesArray.Length; i++)
        //{
        //    GameObject newButton = Instantiate(customLevelButton_prefab, grid);
        //    newButton.GetComponentInChildren<TextMeshProUGUI>().text = savedFilesArray[i];
        //}


    }

    private void ImportCustomLevelsFromSavedFiles()
    {
        if (Application.isPlaying && !Application.isEditor)
        {

            var info = new DirectoryInfo(dataPath);

            FileInfo[] fileInfo = info.GetFiles();

            //string[] fileNamesArray = new string[fileInfo.Length];

            for (int i = 0; i < fileInfo.Length; i++)
            {
                string texto = File.ReadAllText(fileInfo[i].FullName);
                LevelInfo thisLevel = JsonConvert.DeserializeObject<LevelInfo>(texto);
                customLevelsList.Add(thisLevel);
            }
        }

    }

    private void ImportCustomLevelsFromAssets()
    {
        TextAsset[] assetsArray = Resources.LoadAll<TextAsset>("CustomLevels");

        for (int i = 0; i < assetsArray.Length; i++)
        {
            LevelInfo thisLevel = JsonConvert.DeserializeObject<LevelInfo>(assetsArray[i].text);
            customLevelsList.Add(thisLevel);
        }

    }

    //private string[] CreateSavedFilesArray()
    //{
    //    var info = new DirectoryInfo(dataPath);

    //    FileInfo[] fileInfo = info.GetFiles();

    //    string[] assetsArray = new string[fileInfo.Length];


    //    for(int i = 0; i < assetsArray.Length; i++)
    //    {
    //        assetsArray[i] = fileInfo[i].ToString();
    //    }

    //    return assetsArray;
    //}

    //private TextAsset[] CreateCustomFromAssetsArray()
    //{
    //    TextAsset[] assetsArray = Resources.LoadAll<TextAsset>("CustomLevels");
    //    return assetsArray;
    //}

    public void CreateNewLevel()
    {

        LevelInfo newLevel = new LevelInfo
        {
            levelID = lastCreatedLevelID + 1,
            levelName = inputNewLevelName.text
        };

        string serializedNewLevel = JsonConvert.SerializeObject(newLevel, Formatting.Indented);

        File.WriteAllText(dataPath + inputNewLevelName.text + ".txt", serializedNewLevel);
        // add to menu
        GameObject newButton = Instantiate(customLevelButton_prefab, grid);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = inputNewLevelName.text;

        GSRequestData parsedJson = new GSRequestData(serializedNewLevel);

        var sparks = new LogEventRequest_STORE_CUSTOM_LEVEL().Set_LEVEL_NAME("some name").Set_LEVEL_DATA(parsedJson);
        sparks.Send(response =>
       {
           //GSData scriptData = response.ScriptData;
       });
    }
}
