using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

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

    void Awake()
    {
        DestroyPlaceholder();
        UpdateSelectMenu();

        if (Application.isPlaying && !Application.isEditor)
        {
            dataPath = Application.persistentDataPath;
        }

        if (Application.isPlaying && Application.isEditor)
        {
            dataPath = Application.dataPath;
        }
    }

    private void DestroyPlaceholder()
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
        TextAsset[] assetsArray = CreateAssetArray();

        for(int i = 0; i < assetsArray.Length; i++)
        {
            GameObject newButton = Instantiate(customLevelButton_prefab, grid);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = assetsArray[i].name;
        }
    }

    private TextAsset[] CreateAssetArray()
    {
        TextAsset[] assetsArray = Resources.LoadAll<TextAsset>("CustomLevels");
        return assetsArray;
    }

    public void CreateNewLevel()
    {
        File.WriteAllText(dataPath + "/Resources/CustomLevels/" + inputNewLevelName.text +".txt", "ok");
        // add to menu
        GameObject newButton = Instantiate(customLevelButton_prefab, grid);
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = inputNewLevelName.text;
    }
}
