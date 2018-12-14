using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

    public void ResetRound()
    {
        Debug.Log("I should Reset");
        FindObjectOfType<NewRoundManager>().Reset();
    }

}
