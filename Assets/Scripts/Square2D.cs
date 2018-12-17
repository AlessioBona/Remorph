using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]

public class Square2D : MonoBehaviour {

    public int rowNumber;
    public int colNumber;

    public GameObject backGround;
    public GameObject iconSprite;

    public bool isOn;

    public enum Icon : int
    {
        up,
        right,
        down,
        left,
        contr_Horiz,
        contr_Vert,
        expand_Horiz,
        expand_Vert,
        expand_Up,
        expand_Right,
        expand_Down,
        expand_Left,
        contr_Up,
        contr_Right,
        contr_Down,
        contr_Left,
        turn_Left,
        turn_Right
    };
    public Icon icona;

    public enum Colo : int
    {
        red,
        blue,
        yellow
    };
    public Colo colore;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
