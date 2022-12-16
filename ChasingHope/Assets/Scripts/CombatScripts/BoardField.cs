using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;  // StringBuilder

struct Node {
    Vector2Int coord;
    int gCost;      // How far the space is from the root (how much it costs to get to the space)
    int hValue;     // How far the space is from the destination (how good is the space)
}

public class BoardField : MonoBehaviour
{
    [SerializeField]
    private Piece[,] board;

    private int height = 8;
    private int width = 8;
    // Start is called before the first frame update
    void Start()
    {
        ResetBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetBoard() {
        board = new Piece[height, width];
        for (int i=0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                board[i, j] = null;
            }
        }
    }

    public void PrintBoard() {
        StringBuilder sb = new StringBuilder();  

        //Print 8x8 chess board with width and height arguments. 
        for ( int i = 0; i < width; i++ )
        {
            for ( int j = 0; j < height; j++ )
            {
                sb.Append(board[i, j]);
                sb.Append(" | ");
            }
            sb.AppendLine(); 
        }
        Debug.Log(sb);
    }

    public bool CheckSpaceOccupied(int x, int y) {
        return board[y, x] != null;
    }

    public void AddPiece(Piece newPiece, int x, int y) {
        board[y, x] = newPiece;
    }

    // public int GetDistance(int x1, int y1, int x2, int y2) {
    //     int xDiff = Mathf.Abs(x1 - x2);
    //     int yDiff = Mathf.Abs(y1 - y2);

    //     return xDiff + yDiff;
    // }

    // public List<Vector2Int> GetPath(int x1, int y1, int x2, int y2) {
    //     List<Vector2Int> path = new List<Vector2Int>();
    //     return path;
    // }
}
