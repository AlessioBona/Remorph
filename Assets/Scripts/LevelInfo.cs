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
    //public List<List<int>> squareMatrix { get; set; } //in NewRoundManager: squaresData
    //public List<List<int>> squareColorMatrix { get; set; } //in NewRoundManager: squaresData
    public int[][] squareMatrix { get; set; } //in NewRoundManager: squaresData
    public int[][] squareColorMatrix { get; set; } //in NewRoundManager: squaresData

    //public int[][][] playerForm { get; set; }
    public int[][][] playerForm { get; set; }

    public LevelInfo()
    {
        rows = 1;
        cols = 1;
        levelName = "un nome";

        squareMatrix = new int[4][];
        for (int i = 0; i < 4; i++)
        {
            squareMatrix[i] = new int[8];
            for (int e = 0; e < 8; e++)
            {
                squareMatrix[i][e] = 999;
            }
        }

        squareColorMatrix = new int[4][];
        for (int i = 0; i < 4; i++)
        {
            squareColorMatrix[i] = new int[8];
            for (int e = 0; e < 8; e++)
            {
                squareColorMatrix[i][e] = 0;
            }
        }


        //standard PLAYER
        playerForm = new int[3][][];
        for (int i = 0; i < playerForm.Length; i++)
        {
            playerForm[i] = new int[2][];
            for(int e = 0; e < playerForm[i].Length; e++)
            {
                playerForm[i][e] = new int[2];
                playerForm[i][e][0] = i*2 + e;
                playerForm[i][e][1] = i*2 + e;
            }

        }

        //squareMatrix = new List<List<int>>();
        //for(int i = 0; i < 4; i++)
        //{
        //    squareMatrix.Add(new List<int>());

        //    for(int e = 0; e < 8; e++)
        //    {
        //        squareMatrix[i].Add(999);
        //    }
        //}

        //squareColorMatrix = new List<List<int>>();
        //for (int i = 0; i < 4; i++)
        //{
        //    squareColorMatrix.Add(new List<int>());

        //    for (int e = 0; e < 8; e++)
        //    {
        //        squareColorMatrix[i].Add(0);
        //    }
        //}

        // initialize standard player
        //playerForm = new int[3][][];
        //for(int i = 0; i < playerForm.Length; i++)
        //{
        //    playerForm[i] = new int[6][];
        //    for(int e = 0; e < playerForm[i].Length; e++)
        //    {
        //        playerForm[i][e] = new int[6];
        //    }
        //}
        //playerForm[0][0][0] = 1;
        //playerForm[0][1][1] = 1;
        //playerForm[1][2][2] = 1;
        //playerForm[1][3][3] = 1;
        //playerForm[2][4][4] = 1;
        //playerForm[2][5][5] = 1;

    }



}
