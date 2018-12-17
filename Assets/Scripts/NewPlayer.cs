using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour {

    public int startCol;
    public int startRow;

    public int endCol;
    public int endRow;

    public GameObject squaresParent;
    public int columns;
    public int rows;
    public GameObject[] parts;
    public Transform RayCaster;

    public bool isPlayer;

    public Vector3[] partsPos;
    public Vector3[] partsDim;
    public int[] partsRot;

    public int xPos = 0;
    public int yPos = 0;
    private int oldXPos = 0;
    private int oldYPos = 0;

    public Vector3 moveTo;
    public Vector3 prevPosition;
    public float tranSpeed;
    public float shiftSpeed;
    public float shapeSpeed;
    public float rotaSpeed;

    public bool canMove;
    public bool checkIfIWon;

    public Transform oldTrans;
    Vector3 newTransPos;
    Vector3 newTransSca;
    Quaternion newTransRot;
    int colorToTransform = 99;
    Vector3 prevTransPos;
    Vector3 prevTransSca;
    int[] prevRotArray;
    Quaternion prevTransRot;
    int transformedColor = 99;

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
        partsRot = new int[] { 0, 0, 0 };
    }

    public void MoveUpForRaycast()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].transform.localPosition += new Vector3(0f, 0f, -2f);
        }
    }

    public void Reset()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            parts[i].transform.localPosition = partsPos[i];
            parts[i].transform.localScale = partsDim[i];
            parts[i].transform.parent.localRotation = Quaternion.Euler(0f, 0f, 0f);
            oldXPos = startCol;
            oldYPos = startRow;
            colorToTransform = 99;
            partsRot = new int[] { 0, 0, 0 };
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
                if (checkIfIWon)
                {
                    FindObjectOfType<NewRoundManager>().CheckWin();
                }
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

            if (oldTrans != null && oldTrans.parent.localRotation != newTransRot && gameObject.transform.position == moveTo)
            {
                parts[colorToTransform].transform.parent.localRotation = Quaternion.RotateTowards(parts[colorToTransform].transform.parent.localRotation, newTransRot, rotaSpeed);
            }

        }
    }

    public void Move(int x, int y)
    {
        if (canMove)
        {
            int newX = xPos + x;
            int newY = yPos + y;
            
            if (newX >= 0 && newX < columns && newY >= 0 && newY < rows)
            {

                Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

                foreach (Square2D square in allSquares)
                {
                    if (square.colNumber == newX && square.rowNumber == newY)
                    {
                        prevPosition = transform.position;
                        moveTo = square.transform.position;

                        oldXPos = xPos;
                        oldYPos = yPos;
                        
                        xPos = newX;
                        yPos = newY;

                        justWentBack = false;
                        

                        if (square.isOn)
                        {
                            TransformParts((int)square.colore, (int)square.icona);
                        }

                        if(square.colNumber == endCol && square.rowNumber == endRow) // TO SUBSTITUTE
                        {
                            checkIfIWon = true;
                        } else
                        {
                            checkIfIWon = false;
                        }

                    }

                }
            }
        }
    }

    // so you can just go back once
    // a more elegant way would be an array of movement and transformations... but, do I want it?
    bool justWentBack = false;

    public void ReverseTransformation()
    {
        if(colorToTransform != 99 && !justWentBack)
        {
            

            Square2D[] allSquares = squaresParent.GetComponentsInChildren<Square2D>();

            foreach (Square2D square in allSquares)
            {
                if (square.colNumber == xPos && square.rowNumber == yPos)
                {
                    if (square.isOn)
                    {
                        
                        newTransPos = prevTransPos;
                        newTransSca = prevTransSca;

                        newTransRot = prevTransRot;
                        partsRot = prevRotArray;
                        Debug.Log(newTransRot);
                        Debug.Log(prevTransRot);
                        Debug.Log(oldTrans.rotation);
                    }
                }
            }

                xPos = oldXPos;
                yPos = oldYPos;


            moveTo = prevPosition;

            justWentBack = true;
        }
    }

    public void TransformParts(int color, int icon)
    {
        float scaleUnit = 50f;
        float scaleMax = 800f;
        oldTrans = parts[color].transform;

        newTransPos = oldTrans.localPosition;
        newTransSca = oldTrans.localScale;
        newTransRot = oldTrans.parent.localRotation;
        colorToTransform = color;

        //saved to reverse
        prevTransPos = newTransPos;
        prevTransSca = newTransSca;
        prevTransRot = newTransRot;

        prevRotArray = new int[] { partsRot[0], partsRot[1], partsRot[2] };

        if(icon <= 3)
        {
            icon += partsRot[color];
            if (icon >= 4)
                icon -= 4;
        }

        if(icon == 4 || icon == 6)
        {
            if (partsRot[color] == 1 || partsRot[color] == 3)
                icon += 1;
        }

        if (icon == 5 || icon == 7)
        {
            if (partsRot[color] == 1 || partsRot[color] == 3)
                icon -= 1;
        }

        if (icon >= 8 && icon <= 11)
        {
            icon += partsRot[color];
            if (icon >= 12)
                icon -= 4;
        }

        if (icon >= 12 && icon <= 15)
        {
            icon += partsRot[color];
            if (icon >= 16)
                icon -= 4;
        }

        switch (icon)
        {
            case 0:
                //up
                newTransPos += new Vector3(0f, .5f, 0f);
                break;
            case 1:
                //right
                newTransPos += new Vector3(+0.5f, 0f, 0f);
                break;
            case 2:
                //down
                newTransPos += new Vector3(0f, -.5f, 0f);
                break;
            case 3:
                //left
                newTransPos += new Vector3(-0.5f, 0f, 0f);
                break;

            case 4:
                //conH
                if (newTransSca.x > scaleUnit)
                {
                    newTransSca += new Vector3(-scaleUnit, 0f, 0f);
                }
                break;
            case 5:
                //conV
                if (newTransSca.y > scaleUnit)
                {
                    newTransSca += new Vector3(0f, -scaleUnit, 0f);
                }
                break;

            case 6:
                //exHo
                if (newTransSca.x < scaleMax)
                {
                    newTransSca += new Vector3(scaleUnit, 0f, 0f);
                }
                break;
            case 7:
                //exVe
                if (newTransSca.y < scaleMax)
                {
                    newTransSca += new Vector3(0f, scaleUnit, 0f);
                }
                break;

            case 8:
                //exUp
                if (newTransSca.y < scaleMax)
                {
                    newTransPos += new Vector3(0f, 0.25f, 0f);
                    newTransSca += new Vector3(0f, scaleUnit/2, 0f);
                }
                break;
            case 9:
                //exRight
                if (newTransSca.x < scaleMax)
                {
                    newTransPos += new Vector3(0.25f, 0f, 0f);
                    newTransSca += new Vector3(scaleUnit/2, 0f, 0f);
                }
                break;
            case 10:
                //exDown
                if (newTransSca.y < scaleMax)
                {
                    newTransPos += new Vector3(0f, -0.25f, 0f);
                    newTransSca += new Vector3(0f, scaleUnit / 2, 0f);
                }
                break;
            case 11:
                //exLeft
                if (newTransSca.x < scaleMax)
                {
                    newTransPos += new Vector3(-0.25f, 0f, 0f);
                    newTransSca += new Vector3(scaleUnit / 2, 0f, 0f);
                }
                break;

            case 12:
                //contrUp
                if (newTransSca.y > scaleUnit / 2)
                {
                    newTransPos += new Vector3(0f, -0.25f, 0f);
                    newTransSca += new Vector3(0f, -scaleUnit / 2, 0f);
                }
                break;
            case 13:
                //contrRight
                if (newTransSca.x > scaleUnit/2)
                {
                    newTransPos += new Vector3(-0.25f, 0f, 0f);
                    newTransSca += new Vector3(-scaleUnit / 2, 0f, 0f);
                }
                break;
            case 14:
                //contrDown
                if (newTransSca.y > scaleUnit / 2)
                {
                    newTransPos += new Vector3(0f, 0.25f, 0f);
                    newTransSca += new Vector3(0f, -scaleUnit / 2, 0f);
                }
                break;
            case 15:
                //contrLeft
                if (newTransSca.x > scaleUnit / 2)
                {
                    newTransPos += new Vector3(0.25f, 0f, 0f);
                    newTransSca += new Vector3(-scaleUnit / 2, 0f, 0f);
                }
                break;

            case 16:
                //turnLeft
                Debug.Log("turn left");
                newTransRot = Quaternion.Euler(newTransRot.eulerAngles + new Vector3(0f, 0f, 90f));
                partsRot[color] += 1;
                if (partsRot[color] == 4)
                    partsRot[color] = 0;
                break;
            case 17:
                //turnRight
                Debug.Log("turn right");
                newTransRot = Quaternion.Euler(newTransRot.eulerAngles + new Vector3(0f, 0f, -90f));
                partsRot[color] -= 1;
                if (partsRot[color] == -1)
                    partsRot[color] = 3;
                break;



            default:
                // nothing
                break;

        }
    }
}
