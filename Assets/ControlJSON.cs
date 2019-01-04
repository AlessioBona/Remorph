using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
using System.IO;


public class MyData
{
    public int aNumber { get; set; }
    public string aWord { get; set; }
    public List<List<int>> matrix { get; set; }
    
}

public class ControlJSON : MonoBehaviour {

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextAsset asset;
    MyData myData = new MyData
    {
        matrix = new List<List<int>> {
            new List<int> { 1, 2 },
            new List<int> { 3, 4 }
        }
    };

    MyData mySavedData;
    TextAsset[] assetsArray;

    // Use this for initialization
    void Awake()
    {
        string json = JsonConvert.SerializeObject(myData, Formatting.Indented);
        text1.text = json;
        GUIUtility.systemCopyBuffer = json;

        mySavedData = JsonConvert.DeserializeObject<MyData>(asset.text);
       

        Debug.Log(Application.dataPath);

        File.WriteAllText(Application.dataPath + "/Levels/Resources/4.txt", json);

        assetsArray = Resources.LoadAll<TextAsset>("");
        text2.text = assetsArray[0].text;

    }

    //if(Application.isPlaying && !Application.isEditor)
    //    {
    //        dataPath = Application.persistentDataPath;
    //    }
 
    //if(Application.isPlaying && Applicaiton.isEditor)
    //    {
    //         dataPath = Application.dataPath;
    //        // Will get called only when playing in Editor mode
    //    }

}
