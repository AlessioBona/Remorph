using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class LevelEditor : MonoBehaviour {

    public GameObject firstSquarePosition;
    public GameObject squaresParent;
    public GameObject squarePrefab;
    //public List<GameObject> squaresLine;
    public List<List<GameObject>> linesRow = new List<List<GameObject>>();
    public int columns;
    public int rows;

    public bool resetSquares = false;
    public bool resetSymbols = false;

    public Sprite[] sprites;
    public Color[] spriteColors;
    public Color transparent;


	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (resetSquares)
        {
            GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("square");
            for(int d = 0; d < toDestroy.Length; d++)
            {
                GameObject.DestroyImmediate(toDestroy[d]);
            }

            //foreach (List<GameObject> row in linesRow)
            //{
            //    foreach (GameObject sq in row)
            //    {
            //        GameObject.DestroyImmediate(sq);
            //    }
            //}

            linesRow = new List<List<GameObject>>();

            for (int r = 0; r < rows; r++)
            {
                List<GameObject> newRow = new List<GameObject>();
                linesRow.Add(newRow);

                for (int c = 0; c < columns; c++)
                {
                    Vector3 newPosition = firstSquarePosition.transform.position;
                    newPosition.x += c * 5f;
                    newPosition.z += r * -5f;
                    GameObject newSquare = Instantiate(squarePrefab, newPosition, firstSquarePosition.transform.rotation, squaresParent.transform);
                    linesRow[r].Add(newSquare);
                }
            }
        }

        if (resetSymbols)
        {
            Debug.Log("I reset symbols");
            foreach (List<GameObject> row in linesRow)
            {
                foreach (GameObject sq in row)
                {
                    if (sq.GetComponent<Square>().isOn)
                    {
                        sq.GetComponentInChildren<Image>().sprite = sprites[(int)sq.GetComponent<Square>().icona];
                        sq.GetComponentInChildren<Image>().color = spriteColors[(int)sq.GetComponent<Square>().colore];
                    }
                    if (!sq.GetComponent<Square>().isOn)
                    {
                        sq.GetComponentInChildren<Image>().color = transparent;
                    }
                }
            }
        }
	}
}
