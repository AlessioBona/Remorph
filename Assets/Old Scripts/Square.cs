using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

    public bool isOn;
    public int squareColor;
    public int icon;

    public enum Icon : int {
        left,
        right,
        up,
        down,
        contrHoriz,
        expandHoriz,
        contrVert,
        expandVert
        };
    public Icon icona;

    public enum Colo : int
    {
        red,
        blue,
        yellow
    };
    public Colo colore;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
