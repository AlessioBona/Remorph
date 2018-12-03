using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour {

    public GameObject squaresParent;
    public int columns;
    public int rows;
    public GameObject red;
    public GameObject blue;
    public GameObject yellow;

    public Vector3[] partsPos;
    public Vector3[] partsDim;

    public int xPos = 0;
    public int yPos = 0;

    // Use this for initialization
    void Start () {
        partsPos = new Vector3[3];
        partsPos[0] = red.transform.localPosition;
        partsPos[1] = blue.transform.localPosition;
        partsPos[2] = yellow.transform.localPosition;
        partsDim = new Vector3[3];
        partsDim[0] = red.transform.localScale;
        partsDim[1] = blue.transform.localScale;
        partsDim[2] = yellow.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move(int x, int y)
    {
        
        int newX = xPos + x;
        int newY = yPos + y;
        Debug.Log(newX);
        if (newX >= 0 && newX < columns && newY >= 0 && newY < rows) {

            xPos = newX;
            yPos = newY;

            Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

            foreach (Square2D square in allSquares)
            {
                if (square.colNumber == xPos && square.rowNumber == yPos)
                {
                    gameObject.transform.position = square.transform.position;
                }

            }
        }
    }
}
