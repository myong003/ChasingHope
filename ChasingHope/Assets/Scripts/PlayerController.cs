using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance {get; private set; }
    public float moveSpeed;     // how fast the player moves between squares
    public Transform movePoint;
    public GameObject interactTrigger;
    public string facingDirection;

    public LayerMask collision;

    [SerializeField]
    private Animator animator;
    private CameraManager cm;

    private bool isFrozen;
    public int horizontalInput = 0;
    private int verticalInput = 0;


    // Make singleton to allow any script to access using PlayerController.Instance
    // Note: might change later idk, only making this to let dialogue freeze movement
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        facingDirection = "down";
        cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (!isFrozen && !cm.isPanning) {
            CheckMovement();
        }

        animator.SetFloat("horizontalMovement", horizontalInput);
        animator.SetFloat("verticalMovement", verticalInput);
        if (horizontalInput < -0.1 || horizontalInput > 0.1 || verticalInput < -0.1 || verticalInput > 0.1) {
            animator.SetBool("isMoving", true);
        }
        else {
            animator.SetBool("isMoving", false);
        }
    }
    
    public void FreezePlayer() {
        isFrozen = true;
    }

    public void UnfreezePlayer() {
        isFrozen = false;
    }

    private void CheckMovement() {
        // If player already on movePoint
        if (Vector3.Distance(movePoint.position, this.transform.position) <= 0.05f)
        {
            horizontalInput = (int)Input.GetAxisRaw("Horizontal");
            verticalInput = (int)Input.GetAxisRaw("Vertical");
            
            // Move movePoint 1 tile horizontal or vertical
            Vector3 deltaX = new Vector3(horizontalInput, 0, 0);
            Vector3 deltaY = new Vector3(0, verticalInput, 0);

            // Check if no colliders in horizontal path
            if (!Physics2D.OverlapCircle(movePoint.position + deltaX, 0.2f, collision))
            {
                movePoint.position += deltaX;
            }
            
            // Check if no colliders in vertical path
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
