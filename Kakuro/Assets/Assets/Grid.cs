using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

    // grid specs
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;

    //// cell specs
    //[SerializeField]
    //private Sprite cellSprite;

    [SerializeField]
    public GameObject cell;

    private Vector2 cellSize;
    private Vector2 cellScale;



    void Start () {
        InitCells();
	}
	
	void InitCells()
    {

        // need to instantiate input field cells instead of sprite game objects

        // creates empty obj and adds sprite renderer component -> set sprite to cellSprite
        //cell.AddComponent<SpriteRenderer>().sprite = cellSprite;

        // get size of sprite
        cellSize = cell.GetComponent<SpriteRenderer>().sprite.bounds.size;  //cellSprite.bounds.size;

        // get new cell size -> adjust size of cells to fit size of grid
        Vector2 newCellSize = new Vector2(gridSize.x / (float)cols, gridSize.y / (float)rows);

        // get scales of cells in order to change to fit in grid
        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize; // size is replaced by new computed size. we needed cellSize for finding scale

        cell.transform.localScale = new Vector2(cellScale.x, cellScale.y);


        // fix cells to grid by getting half of grid and cells
        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;


        // TESTING gameboard loading*****************
        System.Random r = new System.Random();
        List<List<int>> test_gameboard = new List<List<int>>();
        List<int> newRow;
        for (int col = 0; col < cols; col++)
        {
            newRow = new List<int>();
            for (int row = 0; row < rows; row++)
            {
                newRow.Add(r.Next(2));
                print(newRow[row] + " ");
            }
            test_gameboard.Add(newRow);
            print("\n");
        }
        // ******************************************


        // fill grid with cells by instantiation
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // add cell size so no two cells have same x and y pos
                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);

                if (test_gameboard[col][row] == 1)
                {
                    // instantiate game obj, at position pos, w/ rotation set to id
                    GameObject cO = Instantiate(cell, pos, Quaternion.identity) as GameObject;
                    // set parent of cell to grid to move cells together w/ grid
                    cO.transform.parent = transform;
                    continue;
                }
            }
        }

        // destroy obj used to instantiate cells
        //Destroy(cell);
    }

    // to see width and height of grid on editor
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }

}
