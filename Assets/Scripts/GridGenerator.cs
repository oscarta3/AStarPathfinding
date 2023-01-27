using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public GameObject nodeCell;
    public int gridX;
    public int gridY;
    public bool isWalkable;

    public Node parent;

    public int gCost; //distance from the current node to the start node
    public int hCost; //distance from the current node to the target node
    public int fCost { get { return gCost + hCost; } } // G + H cost


    public Node(GameObject _nodeCell, int _gridX, int _gridY, bool _isWalkable)
    {
        nodeCell = _nodeCell;
        gridX = _gridX;
        gridY = _gridY;
        isWalkable = _isWalkable;
    }

}


public class GridGenerator : MonoBehaviour
{
    public int gridHeight;
    public int gridWidth;
    public float cellSize;
    public float cellSeparation;

    public GameObject cell;
    public Transform cameraGO;
    public Transform planeGO;
    public Transform lightGO;
    public Pathfinding _pathfinding;

    Node[,] gridArray;

    public List<Node> path;

    Node start;
    Node target;

    void Awake()
    {
        _pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {

        cell.transform.localScale = new Vector3(cellSize, cell.transform.localScale.y, cellSize);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearGrid();
            gridArray = new Node[gridHeight, gridWidth];
            GenerateGrid();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    gridArray[i, j].nodeCell.GetComponent<CellManager>().colorUpdate = true;
                    gridArray[i, j].nodeCell.GetComponent<CellManager>().size = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (path != null) { path.Clear(); }

            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    if (gridArray[i, j].nodeCell.GetComponent<Renderer>().material.color == Color.green) { gridArray[i, j].nodeCell.GetComponent<Renderer>().material.color = Color.white; }
                }
            }

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    if (gridArray[i, j].nodeCell.GetComponent<Renderer>().material.color == Color.red)
                    {
                        start = gridArray[i, j];
                    }

                    if (gridArray[i, j].nodeCell.GetComponent<Renderer>().material.color == Color.blue)
                    {
                        target = gridArray[i, j];
                    }
                }
            }
            _pathfinding.FindPath(start, target);
            start.nodeCell.GetComponent<Renderer>().material.color = Color.red;
            target.nodeCell.GetComponent<Renderer>().material.color = Color.blue;


            ColorPath();
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject gridCell = Instantiate(cell, new Vector3(i * (cellSize + cellSeparation), 0, j * (cellSize + cellSeparation)), Quaternion.identity, transform);
                gridArray[i, j] = new Node(gridCell, i, j, true);
            }
        }

        cameraGO.transform.position = new Vector3((gridHeight * (cellSize + cellSeparation)) / 2, cameraGO.transform.position.y, (gridWidth * (cellSize + cellSeparation)) / 2);
        planeGO.transform.position = new Vector3((gridHeight * (cellSize + cellSeparation)) / 2, planeGO.transform.position.y, (gridWidth * (cellSize + cellSeparation)) / 2);
        lightGO.transform.position = new Vector3((gridHeight * (cellSize + cellSeparation)) / 2, lightGO.transform.position.y, (gridWidth * (cellSize + cellSeparation)) / 2);
    }

    void ClearGrid()
    {
        if (gridArray != null)
        {
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    Destroy(gridArray[i, j].nodeCell);
                }
            }
        }

        if (path != null) { path.Clear(); }
    }

    public void UpdateGrid()
    {
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                if (gridArray[i, j].nodeCell.GetComponent<Renderer>().material.color == Color.black)
                {
                    gridArray[i, j].isWalkable = false;
                }
                else
                {
                    gridArray[i, j].isWalkable = true;
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridHeight && checkY >= 0 && checkY < gridWidth)
                {
                    neighbours.Add(gridArray[checkX, checkY]);
                    if (gridArray[checkX, checkY].isWalkable)
                    {
                        gridArray[checkX, checkY].nodeCell.GetComponent<CellManager>().colorUpdate = false;
                        gridArray[checkX, checkY].nodeCell.GetComponent<Renderer>().material.color = Color.magenta;
                    }

                }
            }
        }

        return neighbours;
    }

    void ColorPath()
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            gridArray[path[i].gridX, path[i].gridY].nodeCell.GetComponent<CellManager>().colorUpdate = false;
            gridArray[path[i].gridX, path[i].gridY].nodeCell.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}


