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
        left,
        right,
        up,
        down,
        contr_Horiz,
        expand_Horiz,
        contr_Vert,
        expand_Vert,
        expand_Left,
        expand_Right,
        expand_Up,
        expand_Down,
        contr_Left,
        contr_Right,
        contr_Up,
        contr_Down,
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
