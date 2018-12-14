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
        StartCoroutine(ActuallyLoadLevel(levelNumber));

        //UIManager.SendGameEvent("LoadLevel_" + levelNumber);
        //PlayerPrefs.SetInt(LOADED_LEVEL, levelNumber);
        //PlayerPrefs.Save();

    }

    public void LoadNextLevel()
    {
        UIManager.ShowUiElement("LoadScreen", "MyUI");
        int levelNumber = PlayerPrefs.GetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
        StartCoroutine(ActuallyNextLevel(levelNumber));
    }

    IEnumerator ActuallyNextLevel(int levelNumber)
    {
        yield return new WaitForSeconds(.5f);
        UIManager.SendGameEvent("UnloadLevel_" + levelNumber);
        PlayerPrefs.SetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
        PlayerPrefs.Save();
        UIManager.HideUiElement("YouWon", "MyUI");
        StartCoroutine(ActuallyLoadLevel(levelNumber + 1));
    }


    IEnumerator ActuallyLoadLevel(int levelNumber)
    {
        yield return new WaitForSeconds(.7f);
        UIManager.HideUiElement("MainMenu", "MyUI");
        UIManager.SendGameEvent("LoadLevel_" + levelNumber);
        PlayerPrefs.SetInt(LOADED_LEVEL, levelNumber);
        PlayerPrefs.Save();
    }

    IEnumerator ActuallyUnloadLevel(int levelNumber)
    {
        UIManager.ShowUiElement("LoadScreen", "MyUI");
        yield return new WaitForSeconds(.5f);
        UIManager.HideUiElement("InGame", "MyUI");
        UIManager.HideUiElement("YouWon", "MyUI");
        UIManager.SendGameEvent("UnloadLevel_" + levelNumber);
        PlayerPrefs.SetInt(LOADED_LEVEL, NO_LEVEL_LOADED);
        PlayerPrefs.Save();
        UIManager.ShowUiElement("MainMenu", "MyUI");
        yield return new WaitForSeconds(.7f);
        UIManager.HideUiElement("LoadScreen", "MyUI");

    }

    public void UnloadLevel(int levelNumber)
    {
        StartCoroutine(ActuallyUnloadLevel(levelNumber));
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
