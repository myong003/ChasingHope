using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;     // how fast the player moves between squares
    public Transform movePoint;

    public LayerMask collision;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(movePoint.position, this.transform.position) <= 0.05f)
        {
            int horizontalInput = (int)Input.GetAxisRaw("Horizontal");
            int verticalInput = (int)Input.GetAxisRaw("Vertical");
            
            Vector3 deltaX = new Vector3(horizontalInput, 0, 0);
            Vector3 deltaY = new Vector3(0, verticalInput, 0);

            if (!Physics2D.OverlapCircle(movePoint.position + deltaX, 0.2f, collision))
            {
                movePoint.position += deltaX;
            }

            if (!Physics2D.OverlapCircle(movePoint.position + deltaY, 0.2f, collision))
            {
                movePoint.position += deltaY;
            }
        }
    }
}
