using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour {

    public GameObject squaresParent;
    public int columns;
    public int rows;
    public GameObject[] parts;
    public Transform RayCaster;

    public bool isPlayer;

    public Vector3[] partsPos;
    public Vector3[] partsDim;

    public int xPos = 0;
    public int yPos = 0;

    public Vector3 moveTo;
    public float tranSpeed;
    public float shiftSpeed;
    public float shapeSpeed;

    public bool canMove;

    public Transform oldTrans;
    Vector3 newTransPos;
    Vector3 newTransSca;
    int colorToTransform;

    // Use this for initialization
    void Awake () {
        partsPos = new Vector3[3];
        partsPos[0] = parts[0].transform.localPosition;
        partsPos[1] = parts[1].transform.localPosition;
        partsPos[2] = parts[2].transform.localPosition;
        partsDim = new Vector3[3];
        partsDim[0] = parts[0].transform.localScale;
        partsDim[1] = parts[1].transform.localScale;
        partsDim[2] = parts[2].transform.localScale;
    }

    public void Reset()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            parts[i].transform.localPosition = partsPos[i];
            parts[i].transform.localScale = partsDim[i];
        }
 
    }

    // Update is called once per frame
    void Update () {
        if (isPlayer)
        {

            if (gameObject.transform.position != moveTo && canMove)
            {
                canMove = false;
            }

            if (gameObject.transform.position == moveTo && !canMove && (oldTrans == null || (oldTrans.localPosition == newTransPos && gameObject.transform.position == moveTo)))
            {
                canMove = true;
            }

            if (gameObject.transform.position != moveTo)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, moveTo, tranSpeed);
            }

            if (oldTrans != null && oldTrans.localPosition != newTransPos && gameObject.transform.position == moveTo)
            {
                parts[colorToTransform].transform.localPosition = Vector3.MoveTowards(parts[colorToTransform].transform.localPosition, newTransPos, shiftSpeed);

            }

            if (oldTrans != null && oldTrans.localScale != newTransSca && gameObject.transform.position == moveTo)
            {
                parts[colorToTransform].transform.localScale = Vector3.MoveTowards(parts[colorToTransform].transform.localScale, newTransSca, shapeSpeed);
            }
        }
    }

    public void Move(int x, int y)
    {
        if (canMove)
        {
            int newX = xPos + x;
            int newY = yPos + y;
            Debug.Log(newY + " of " + rows);
            if (newX >= 0 && newX < columns && newY >= 0 && newY < rows)
            {

                Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

                foreach (Square2D square in allSquares)
                {
                    if (square.colNumber == newX && square.rowNumber == newY)
                    {
                        moveTo = square.transform.position;
                        xPos = newX;
                        yPos = newY;
                        FindObjectOfType<NewRoundManager>().CheckWin();

                        if (square.isOn)
                        {
                            TransformParts((int)square.colore, (int)square.icona);
                        }

                    }

                }
            }
        }
    }

    public void TransformParts(int color, int icon)
    {
        float scaleUnit = 50f;
        float scaleMax = 800f;
        oldTrans = parts[color].transform;
        newTransPos = oldTrans.localPosition;
        newTransSca = oldTrans.localScale;
        colorToTransform = color;

        switch (icon)
        {
            case 0:
                //left
                newTransPos += new Vector3(-0.5f, 0f, 0f);
                    break;
            case 1:
                //right
                newTransPos += new Vector3(+0.5f, 0f, 0f);
                break;
            case 2:
                //up
                newTransPos += new Vector3(0f, .5f, 0f);
                break;
            case 3:
                //down
                newTransPos += new Vector3(0f, -.5f, 0f);
                break;
            case 4:
                //conH
                if (newTransSca.x > scaleUnit)
                {
                    newTransSca += new Vector3(-scaleUnit, 0f, 0f);
                }
                break;
            case 5:
                //exHo
                if (newTransSca.x < scaleMax)
                {
                    newTransSca += new Vector3(scaleUnit, 0f, 0f);
                }
                break;
            case 6:
                //conV
                if (newTransSca.y > scaleUnit)
                {
                    newTransSca += new Vector3(0f, -scaleUnit, 0f);
                }
                break;
            case 7:
                //exVe
                if (newTransSca.y < scaleMax)
                {
                    newTransSca += new Vector3(0f, scaleUnit, 0f);
                }
                break;
            default:
                // nothing
                break;

        }
    }
}
