using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    // 0 - down, 1 - up, 2 - left, 3 - right
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    public Vector2 size;
    public int startPos = 0;
    public GameObject room;
    public GameObject end;
    public Vector2 offset;
    Vector3 endPos;

    List<Cell> board;
    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for(int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {
                    var newRoom = Instantiate(room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status);

                    newRoom.name += " " + i + " - " + j;
                    endPos = new Vector3(newRoom.transform.position.x, newRoom.transform.position.y, newRoom.transform.position.z);
                }
            }
        }
        var startPoint = Instantiate(end, new Vector3(0, 0, 0), Quaternion.identity);
        startPoint.name = "start";
        var endPoint = Instantiate(end, endPos, Quaternion.identity, transform);
        endPoint.name = "end";
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;
    
        while (k < 1000)
        {
            k++;

            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
                break;

            //Check the cell's neighbours
            List<int> neighbours = CheckNeighbour(currentCell);

            if (neighbours.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbours[Random.Range(0, neighbours.Count)];

                if(newCell > currentCell)
                {
                    //down or right
                    if(newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //down or right
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbour(int cell)
    {
        List<int> neighbours = new List<int>();

        //Check direction down neighbours
        if (cell - size.x >= 0 && board[Mathf.FloorToInt(cell-size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - size.x));
        }

        //Check direction up neighbours
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell+size.x)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + size.x));
        }

        //Check direction left neighbours
        if ((cell+1) % size.x != 0 && !board[Mathf.FloorToInt(cell+1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell + 1));
        }

        //Check direction left neighbours
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell-1)].visited)
        {
            neighbours.Add(Mathf.FloorToInt(cell - 1));
        }



        return neighbours;
    }
}
