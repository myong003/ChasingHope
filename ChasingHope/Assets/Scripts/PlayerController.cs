using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;     // how fast the player moves between squares
    public Transform movePoint;
    public GameObject interactTrigger;
    public string facingDirection;

    public LayerMask collision;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        facingDirection = "up";
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        int horizontalInput = 0;
        int verticalInput = 0;

        if (Vector3.Distance(movePoint.position, this.transform.position) <= 0.05f)
        {
            horizontalInput = (int)Input.GetAxisRaw("Horizontal");
            verticalInput = (int)Input.GetAxisRaw("Vertical");
            
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

        Vector3 alicePos = gameObject.transform.position;
        if (verticalInput >= 0.2f)
        {
            facingDirection = "up";
            interactTrigger.transform.position = alicePos + new Vector3(0, 0.5f, 0);
        }

        else if (verticalInput <= -0.2f)
        {
            facingDirection = "down";
            interactTrigger.transform.position = alicePos + new Vector3(0, -0.5f, 0);
        }

        else if (horizontalInput <= -0.2f)
        {
            facingDirection = "left";
            interactTrigger.transform.position = alicePos + new Vector3(-0.5f, 0, 0);
        }

        else if (horizontalInput >= 0.2f)
        {
            facingDirection = "right";
            interactTrigger.transform.position = alicePos + new Vector3(0.5f, 0, 0);
        }
    }
 
}
