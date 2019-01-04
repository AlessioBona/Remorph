using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo {

    public int levelNumber { get; set; }
    public int rows { get; set; }
    public int columns { get; set; }
    public int startCol { get; set; }
    public int startRow { get; set; }
    public List<List<int>> squareMatrix { get; set; } //in NewRoundManager: squaresData


}
