using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevelEditor_SelectIcon : MonoBehaviour
{

    CustomLevelEditor_Frame editorFrame;
    [SerializeField]
    GameObject squarePrefab;
    [SerializeField]
    Transform squaresParent;

    [SerializeField]
    Color[] spriteColors = { Color.red, Color.blue, Color.yellow };

    [SerializeField]
    Sprite[] iconSprites;
    private void Awake()
    {
        editorFrame = GetComponentInParent<CustomLevelEditor_Frame>();
        CreateButtons();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    int actualColor = 0;

    List<Square2D> buttonList;

    private void CreateButtons()
    {
        float scaleUnit = 61.5f + 10f;

        Vector3 newPosition = new Vector3(0f, 0f, 0f);

        // --- NO SQUARE ---
        newPosition = new Vector3(0f, 0f, 0f);
        //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
        //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
        newPosition.x -= scaleUnit * 3;
        newPosition.y += scaleUnit * 5;
        GameObject noSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
        noSquare.transform.localPosition = newPosition;
        noSquare.transform.localScale = new Vector3(3f, 3f, 0f);
        Button noSquareButton = noSquare.gameObject.AddComponent<Button>();
        noSquareButton.onClick.AddListener(
            () => SelectionDone(666)
            );

        // --- EMPTY ---
        newPosition = new Vector3(0f, 0f, 0f);
        //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
        //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
        newPosition.x -= scaleUnit * 1;
        newPosition.y += scaleUnit * 5;
        GameObject emptySquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
        emptySquare.transform.localPosition = newPosition;
        emptySquare.transform.localScale = new Vector3(3f, 3f, 0f);
        Button emptySquareButton = emptySquare.gameObject.AddComponent<Button>();
        emptySquareButton.onClick.AddListener(
            () => SelectionDone(999)
            );


        // --- PLAYER ---
        newPosition = new Vector3(0f, 0f, 0f);
        //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
        //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
        newPosition.x += scaleUnit * 1;
        newPosition.y += scaleUnit * 5;
        GameObject playerSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
        playerSquare.transform.localPosition = newPosition;
        playerSquare.transform.localScale = new Vector3(3f, 3f, 0f);
        Button playerSquareButton = playerSquare.gameObject.AddComponent<Button>();
        playerSquareButton.onClick.AddListener(
            () => SelectionDone(100)
            );

        // --- END ---
        newPosition = new Vector3(0f, 0f, 0f);
        //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
        //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
        newPosition.x += scaleUnit * 3;
        newPosition.y += scaleUnit * 5;
        GameObject endSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
        endSquare.transform.localPosition = newPosition;
        endSquare.transform.localScale = new Vector3(3f, 3f, 0f);
        Button endSquareButton = endSquare.gameObject.AddComponent<Button>();
        endSquareButton.onClick.AddListener(
            () => SelectionDone(101)
            );

        for (int c = 0; c < 3; c++)
        {
            newPosition = new Vector3(0f, 0f, 0f);
            //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
            //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
            newPosition.x += -2*scaleUnit + c*2*scaleUnit;
            newPosition.y += scaleUnit * 2;
            GameObject newSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
            newSquare.transform.localPosition = newPosition;
            newSquare.transform.localScale = new Vector3(3f, 3f, 0f);

            Button thisButton = newSquare.gameObject.AddComponent<Button>();

            int ca = c;

            thisButton.onClick.AddListener(() => SetColor(ca));
        }

        buttonList = new List<Square2D>();

        int yCount = 1;
        int xCount = 0;

        for(int s = 0; s < 16; s++)
        {
            newPosition = new Vector3(0f, 0f, 0f);
            //newPosition.x += (cols - 1) * -scaleUnit + c * scaleUnit*2;
            //newPosition.y += (rows - 1) * scaleUnit - r * scaleUnit*2;
            if(s == 4 || s == 8 || s == 12)
            {
                yCount++;
                xCount = 0;
            }

            newPosition.x += -3 * scaleUnit + xCount * 2 * scaleUnit;
            newPosition.y += 2*scaleUnit - yCount*2*scaleUnit;

            xCount++;

            GameObject newSquare = Instantiate(squarePrefab, newPosition, squaresParent.transform.rotation, squaresParent.transform);
            newSquare.transform.localPosition = newPosition;
            newSquare.transform.localScale = new Vector3(3f, 3f, 0f);

            Button thisButton = newSquare.gameObject.AddComponent<Button>();

            buttonList.Add(newSquare.GetComponent<Square2D>());
            newSquare.GetComponent<Square2D>().iconSprite.GetComponent<SpriteRenderer>().sprite = iconSprites[s];

            int sa = s;
            thisButton.onClick.AddListener(() => SelectionDone(sa));
        }

    }

    public void SetColor(int color)
    {
        actualColor = color;
        Debug.Log("color: " + color);
        foreach ( Square2D button in buttonList)
        {
            button.iconSprite.GetComponent<SpriteRenderer>().color = spriteColors[actualColor];
        }
    }

    public void SelectionDone(int selected)
    {
        
        editorFrame.squaresParent.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        editorFrame.UpdateSquare(selected, actualColor);
    }
}
