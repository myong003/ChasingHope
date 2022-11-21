using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPiece : Piece
{
    [Header("Enemy Stats")]

    public float moveSpeed;
    public Transform movePoint;

    public bool isMoving;
    public bool isMovingBetweenPoints;

    protected override void Start() {
        isMoving = false;
        movePoint.parent = null;
        IEnumerator coroutine = MovePiece(7, -7);
        StartCoroutine(coroutine);
    }

    public IEnumerator MovePiece(int newX, int newY) {
        isMoving = true;
        IEnumerator coroutine;
        int xDir = 0, yDir = 0;
        
        while (coord.x != newX || coord.y != newY) {
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

            if (coord.x != newX) {
                coroutine = MoveOneSquare(xDir, 0);
                StartCoroutine(coroutine);
                yield return new WaitUntil(() => !isMovingBetweenPoints);
            }

            if (coord.y != newY) {
                coroutine = MoveOneSquare(0, yDir);
                StartCoroutine(coroutine);
                yield return new WaitUntil(() => !isMovingBetweenPoints);
            }
        }
        isMoving = false;
    }

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
