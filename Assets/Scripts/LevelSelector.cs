using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoozyUI;

public class LevelSelector : MonoBehaviour {

    public const string LOADED_LEVEL = "LOADED_LEVEL";
    public const int NO_LEVEL_LOADED = -1;

    [SerializeField]
    private int currentLoadedLevel = NO_LEVEL_LOADED;

    private void Awake()
    {
        currentLoadedLevel = PlayerPrefs.GetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
    }

    private void UpdateCurrentLoadeLevel()
    {
        currentLoadedLevel = PlayerPrefs.GetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
    }

    public void LoadLevel(int levelNumber)
    {
        UIManager.ShowUiElement("LoadScreen", "MyUI");
        UIManager.SendGameEvent("LoadLevel_" + levelNumber);
        PlayerPrefs.SetInt(LOADED_LEVEL, levelNumber);
        PlayerPrefs.Save();

    }

    public void UnloadLevel(int levelNumber)
    {
        UIManager.SendGameEvent("UnloadLevel_" + levelNumber);
        PlayerPrefs.SetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
        PlayerPrefs.Save();
    }

    public void UnloadCurrentLevel()
    {
        int levelNumber = PlayerPrefs.GetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
        if (levelNumber == NO_LEVEL_LOADED)
        {
            return;
        }
        UnloadLevel(levelNumber);

    }


}
