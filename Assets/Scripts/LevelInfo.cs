using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo {

    public int levelID { get; set; }
    public string levelName { get; set; }
    public int rows { get; set; }
    public int cols { get; set; }
    public int startCol { get; set; }
    public int startRow { get; set; }
    public List<List<int>> squareMatrix { get; set; } //in NewRoundManager: squaresData
    public int[][][] playerForm { get; set; }

    public LevelInfo()
    {
        rows = 1;
        cols = 1;
        squareMatrix = new List<List<int>>();
        squareMatrix.Add(new List<int>());
        squareMatrix[0].Add(999);

        // initialize standard player
        playerForm = new int[3][][];
        for(int i = 0; i < playerForm.Length; i++)
        {
            playerForm[i] = new int[6][];
            for(int e = 0; e < playerForm[i].Length; e++)
            {
                playerForm[i][e] = new int[6];
            }
        }
        playerForm[0][0][0] = 1;
        playerForm[0][1][1] = 1;
        playerForm[1][2][2] = 1;
        playerForm[1][3][3] = 1;
        playerForm[2][4][4] = 1;
        playerForm[2][5][5] = 1;

    }

    

}
