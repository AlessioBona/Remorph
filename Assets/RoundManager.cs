using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

    public GameObject[] parts;
    Vector3 part1;
    Vector3 part2;
    Vector3 part3;
    Vector3 part1d;
    Vector3 part2d;
    Vector3 part3d;

    public Camera theCamera;
    public GameObject instructions;

    public GameObject player;
    public GameObject reference;

    public GameObject[] squares_1;
    public GameObject[] squares_2;
    public GameObject[] squares_3;
    public GameObject[] squares_4;

    public GameObject actualSquare;

    public Color[] squareColors;
    public Color[] spriteColors;
    public Sprite[] sprites;
    public Color transparent;
    public bool iconsOn = true;

    public int xStart;
    public int yStart;
    public int xPresent;
    public int yPresent;

    // Use this for initialization
    void Start () {
        cameraOriginal = theCamera.transform.position;
        originalZoom = theCamera.orthographicSize;
        //squares = GameObject.FindGameObjectsWithTag("square");
        //squares[0].GetComponentInChildren<Image>().sprite = sprites[0];
        transparent = squares_1[0].GetComponentInChildren<Image>().color;
        actualSquare = squares_4[0];
        part1 = parts[0].transform.localPosition;
        part2 = parts[1].transform.localPosition;
        part3 = parts[2].transform.localPosition;
        part1d = parts[0].transform.localScale;
        part2d = parts[1].transform.localScale;
        part3d = parts[2].transform.localScale;
        Reset();

    }

    private void Reset()
    {
        instructions.SetActive(false);
        parts[0].transform.localPosition = part1;
        parts[1].transform.localPosition = part2;
        parts[2].transform.localPosition = part3;
        parts[0].transform.localScale = part1d;
        parts[1].transform.localScale = part2d;
        parts[2].transform.localScale = part3d;
        xPresent = xStart;
        yPresent = yStart;
        actualSquare.GetComponentInChildren<MeshRenderer>().material.color = squareColors[0];
        actualSquare = squares_4[0];
        actualSquare.GetComponentInChildren<MeshRenderer>().material.color = squareColors[1];
        player.transform.position = actualSquare.transform.position;
        updateIcons();
        reference.SetActive(false);
        iconsOn = true;
    }

    Vector2 touchPositionStart;
    Vector2 touchPositionDifference;

    Vector3 cameraOriginal;
    float originalZoom;


    // Update is called once per frame
    void Update () {
        if (Input.touchCount > 0)
        {

                Touch touch_0 = Input.GetTouch(0);
                if (touch_0.phase == TouchPhase.Began)
                {
                    touchPositionStart = touch_0.position;
                }

                if (touch_0.phase == TouchPhase.Ended)
                {
                    touchPositionDifference = touchPositionStart - touch_0.position;

                if (Mathf.Abs(touchPositionDifference.x) > 200f || Mathf.Abs(touchPositionDifference.y) > 200f)
                {
                    if (Mathf.Abs(touchPositionDifference.x) > Mathf.Abs(touchPositionDifference.y))
                    {
                        if (iconsOn)
                        {
                            // horizontal
                            if (touchPositionDifference.x > 0)
                            {
                                moveLeft();
                            }
                            if (touchPositionDifference.x < 0)
                            {
                                moveRight();
                            }
                        }
                        if (!iconsOn)
                        {
                            if (touchPositionDifference.x > 0)
                            {
                                reference.SetActive(false);
                                theCamera.transform.position = cameraOriginal;
                                theCamera.orthographicSize = originalZoom;
                                Reset();
                            }
                        }
                    }
                    if (Mathf.Abs(touchPositionDifference.x) < Mathf.Abs(touchPositionDifference.y))
                    {
                        if (iconsOn)
                        {
                            // vertical
                            if (touchPositionDifference.y > 0)
                            {
                                moveDown();
                            }
                            if (touchPositionDifference.y < 0)
                            {
                                moveUp();
                            }
                        }
                    }
                }

                }


            if (Input.touchCount > 1)
            {
                Touch touch_1 = Input.GetTouch(1);
                if (touch_1.phase == TouchPhase.Began)
                {
                    if (iconsOn)
                    {
                        TurnIconsOff();
                        instructions.SetActive(true);
                        reference.transform.position = player.transform.position;
                        reference.SetActive(true);
                        theCamera.transform.position = new Vector3(player.transform.position.x, theCamera.transform.position.y, player.transform.position.z);
                        theCamera.orthographicSize = 6f;
                    }
                    else
                    {
                        instructions.SetActive(false);
                        updateIcons();
                        reference.SetActive(false);
                        theCamera.transform.position = cameraOriginal;
                        theCamera.orthographicSize = originalZoom;
                    }
                    iconsOn = !iconsOn;


                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            Reset();
        }
        

        if (iconsOn)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveLeft();
                Debug.Log("left");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveRight();
                Debug.Log("right");
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveUp();
                Debug.Log("up");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveDown();
                Debug.Log("down");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (iconsOn)
            {
                TurnIconsOff();
                reference.transform.position = player.transform.position;
                reference.SetActive(true);
            }
            else
            {
                updateIcons();
                reference.SetActive(false);
            }
            iconsOn = !iconsOn;
        }
    }

    public void moveUp()
    {
        if(yPresent < 3)
        {
            yPresent += 1;
            moveToCoordinates();
        }
    }

    public void moveDown()
    {
        if (yPresent > 0)
        {
            yPresent -= 1;
            moveToCoordinates();
        }
    }

    public void moveLeft()
    {
        if (xPresent > 0)
        {
            xPresent -= 1;
            moveToCoordinates();
        }
    }

    public void moveRight()
    {
        if (xPresent < 7)
        {
            xPresent += 1;
            moveToCoordinates();
        }
    }

    public void moveToCoordinates()
    {
        Debug.Log(xPresent + " " + yPresent);
        actualSquare.GetComponentInChildren<MeshRenderer>().material.color = squareColors[0];

        if (yPresent == 0)
        {
            actualSquare = squares_4[xPresent];
        }
        if (yPresent == 1)
        {
            actualSquare = squares_3[xPresent];
        }
        if (yPresent == 2)
        {
            actualSquare = squares_2[xPresent];
        }
        if (yPresent == 3)
        {
            actualSquare = squares_1[xPresent];
        }

        player.transform.position = actualSquare.transform.position;
        actualSquare.GetComponentInChildren<MeshRenderer>().material.color = squareColors[1];
        if (actualSquare.GetComponent<Square>().isOn)
        {
            TransformPlayer();
        }

    }

    public void TransformPlayer()
    {
        int thisColor = actualSquare.GetComponent<Square>().squareColor;
        GameObject toChange = parts[thisColor];
        int toDo = actualSquare.GetComponent<Square>().icon;
        if(toDo == 0)
        {
            ShiftLeft(toChange);
        }
        if (toDo == 1)
        {
            ShiftRight(toChange);
        }
        if (toDo == 2)
        {
            ShiftUp(toChange);
        }
        if (toDo == 3)
        {
            ShiftDown(toChange);
        }
        if (toDo == 4)
        {
            SmallerHorizontal(toChange);
        }
        if (toDo == 5)
        {
            SmallerVertical(toChange);
        }
        if (toDo == 6)
        {
            BiggerHorizontal(toChange);
        }
        if (toDo == 7)
        {
            BiggerVertical(toChange);
        }
    }

    public void ShiftUp(GameObject oggetto)
    {
        oggetto.transform.position += new Vector3(0, 0, .5f);
    }

    public void ShiftDown(GameObject oggetto)
    {
        oggetto.transform.position += new Vector3(0, 0, -.5f);
    }

    public void ShiftLeft(GameObject oggetto)
    {
        oggetto.transform.position += new Vector3(-0.5f, 0, 0);
    }

    public void ShiftRight(GameObject oggetto)
    {
        oggetto.transform.position += new Vector3(0.5f, 0, 0);
    }

    public void BiggerHorizontal(GameObject oggetto)
    {
        oggetto.transform.localScale += new Vector3(1f, 0, 0);
    }

    public void SmallerHorizontal(GameObject oggetto)
    {
        if (oggetto.transform.localScale.x >= 2f)
        {
            oggetto.transform.localScale += new Vector3(-1f, 0, 0);
        }
    }

    public void BiggerVertical(GameObject oggetto)
    {
        oggetto.transform.localScale += new Vector3(0, 0, 1f);
    }

    public void SmallerVertical(GameObject oggetto)
    {
        if (oggetto.transform.localScale.z >= 2f)
        {
            oggetto.transform.localScale += new Vector3(0, 0, -1f);
        }
    }


    public void updateIcons()
    {
        for(int i = 0; i < squares_1.Length; i++)
        {
            if (squares_1[i].GetComponent<Square>().isOn)
            {
                squares_1[i].GetComponentInChildren<Image>().sprite = sprites[squares_1[i].GetComponent<Square>().icon];
                squares_1[i].GetComponentInChildren<Image>().color = spriteColors[squares_1[i].GetComponent<Square>().squareColor];
                

            }
        }
        for (int i = 0; i < squares_2.Length; i++)
        {
            if (squares_2[i].GetComponent<Square>().isOn)
            {
                squares_2[i].GetComponentInChildren<Image>().sprite = sprites[squares_2[i].GetComponent<Square>().icon];
                squares_2[i].GetComponentInChildren<Image>().color = spriteColors[squares_2[i].GetComponent<Square>().squareColor];


            }
        }
        for (int i = 0; i < squares_3.Length; i++)
        {
            if (squares_3[i].GetComponent<Square>().isOn)
            {
                squares_3[i].GetComponentInChildren<Image>().sprite = sprites[squares_3[i].GetComponent<Square>().icon];
                squares_3[i].GetComponentInChildren<Image>().color = spriteColors[squares_3[i].GetComponent<Square>().squareColor];


            }
        }
        for (int i = 0; i < squares_4.Length; i++)
        {
            if (squares_4[i].GetComponent<Square>().isOn)
            {
                squares_4[i].GetComponentInChildren<Image>().sprite = sprites[squares_4[i].GetComponent<Square>().icon];
                squares_4[i].GetComponentInChildren<Image>().color = spriteColors[squares_4[i].GetComponent<Square>().squareColor];


            }
        }
    }


    public void TurnIconsOff()
    {
        for (int i = 0; i < squares_1.Length; i++)
        {
            if (squares_1[i].GetComponent<Square>().isOn)
            {
                squares_1[i].GetComponentInChildren<Image>().color = transparent;


            }
        }
        for (int i = 0; i < squares_2.Length; i++)
        {
            if (squares_2[i].GetComponent<Square>().isOn)
            {
                squares_2[i].GetComponentInChildren<Image>().color = transparent;


            }
        }
        for (int i = 0; i < squares_3.Length; i++)
        {
            if (squares_3[i].GetComponent<Square>().isOn)
            {
                squares_3[i].GetComponentInChildren<Image>().color = transparent;


            }
        }
        for (int i = 0; i < squares_4.Length; i++)
        {
            if (squares_4[i].GetComponent<Square>().isOn)
            {
                squares_4[i].GetComponentInChildren<Image>().color = transparent;


            }
        }
    }
}
