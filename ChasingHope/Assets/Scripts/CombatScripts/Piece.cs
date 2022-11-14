using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int coord;

    protected virtual void Start() {
        UpdateCoords();
    }

    public void UpdateCoords() {
        int newX = Mathf.RoundToInt(this.gameObject.transform.position.x);
        int newY = Mathf.RoundToInt(this.gameObject.transform.position.y);
        coord = new Vector2Int(newX, newY);
    }
}
