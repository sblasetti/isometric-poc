using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemContainer : MonoBehaviour
{
    public int height = 6;
    public int width = 6;
    public int cellSize = 10;
    public Vector3Int originPosition = default(Vector3Int);

    private GameGrid grid;

    void Start()
    {
        this.grid = new GameGrid(width, height, cellSize, originPosition);
    }

    void Update()
    {
        
    }

    public void DrawGrid()
    {
        this.grid.Draw();
    }

}
