using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step 
{
    public int Col { get; set; } = 0;
    public int Row { get; set; } = 0;

    public Step(int col, int row)
    {
        this.Col = col;
        this.Row = row;
    }
}
