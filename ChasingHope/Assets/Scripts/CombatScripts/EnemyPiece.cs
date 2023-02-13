using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiece : Piece
{
    [Header("Enemy Stats")]

    public float moveSpeed;
    public Transform movePoint;
    public bool isMovingBetweenPoints;
    public bool isFrozen;
    private Piece kingPiece;
    private Vector2Int direction;

    protected override void Start() {
        movePoint.parent = null;
        isFrozen = false;
        kingPiece = GameObject.FindGameObjectWithTag("King").GetComponent<Piece>();

        // IEnumerator coroutine = MovePiece(kingPiece.coord.x, kingPiece.coord.y);
        // StartCoroutine(coroutine);
    }

    protected override void Update() {
        if (!isMovingBetweenPoints && !isFrozen) {
            UpdateDirection();
            StartCoroutine(MoveOneSquare(direction.x, direction.y));
        }
    }

    private void UpdateDirection() {
        int newX = kingPiece.coord.x;
        int newY = kingPiece.coord.y;
        int xDir = 0, yDir = 0;

        if (coord.x > newX) {
            xDir = -1;
        }
        else if (coord.x < newX) {
            xDir = 1;
        }

        if (coord.y > newY) {
            yDir = -1;
        }
        else if (coord.y < newY) {
            yDir = 1;
        }

        if (direction.x == 0 && xDir != 0) {
            direction.x = xDir;
            direction.y = 0;
        }
        else if (direction.y == 0 && yDir != 0) {
            direction.x = 0;
            direction.y = yDir;
        }

        // if (coord.x != newX) {
        //     coroutine = MoveOneSquare(xDir, 0);
        //     StartCoroutine(coroutine);
        //     yield return new WaitUntil(() => !isMovingBetweenPoints);
        // }

        // if (coord.y != newY) {
        //     coroutine = MoveOneSquare(0, yDir);
        //     StartCoroutine(coroutine);
        //     yield return new WaitUntil(() => !isMovingBetweenPoints);
        // }
    }

    // public IEnumerator MovePiece(int newX, int newY) {
    //     IEnumerator coroutine;
    //     int xDir = 0, yDir = 0;
        
    //     while (coord.x != newX || coord.y != newY) {
    //         if (coord.x > newX) {
    //             xDir = -1;
    //         }
    //         else if (coord.x < newX) {
    //             xDir = 1;
    //         }

    //         if (coord.y > newY) {
    //             yDir = -1;
    //         }
    //         else if (coord.y < newY) {
    //             yDir = 1;
    //         }

    //         if (coord.x != newX) {
    //             coroutine = MoveOneSquare(xDir, 0);
    //             StartCoroutine(coroutine);
    //             yield return new WaitUntil(() => !isMovingBetweenPoints);
    //         }

    //         if (coord.y != newY) {
    //             coroutine = MoveOneSquare(0, yDir);
    //             StartCoroutine(coroutine);
    //             yield return new WaitUntil(() => !isMovingBetweenPoints);
    //         }
    //     }
    // }

    private IEnumerator MoveOneSquare(int xDir, int yDir) {
        isMovingBetweenPoints = true;
        movePoint.transform.position += new Vector3(xDir, yDir, 0);

        // while piece not on movePoint;
        while (Vector3.Distance(movePoint.position, this.transform.position) > 0)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        UpdateCoords();
        isMovingBetweenPoints = false;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "KingPiece") {
            Debug.Log("Reached the king");
            Destroy(this.gameObject);
        }
    }

    protected void Attack()
    {
        /*
            while (IsInRange)
            Enemy attack prio: 
            1) Piece they are blocked by. 
            2) the enemy King. 
            3) the most recently Deployed Piece. 
        */
    }
}
