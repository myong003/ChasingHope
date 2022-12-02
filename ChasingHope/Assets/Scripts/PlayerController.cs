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
    [SerializeField]
    private AudioSource audioSource;
    private CameraManager cm;

    private bool isFrozen;

    // Make singleton to allow any script to access using PlayerController.Instance
    // Note: might change later idk, only making this to let dialogue freeze movement
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        facingDirection = "up";
        cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (!isFrozen && !cm.isPanning) {
            CheckMovement();
        }
    }
    
    public void FreezePlayer() {
        isFrozen = true;
    }

    public void UnfreezePlayer() {
        isFrozen = false;
    }

    private void CheckMovement() {
        int horizontalInput = 0;
        int verticalInput = 0;

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

            animator.speed = 0;
        }
        else {
            animator.speed = 1;
        }

        Vector3 alicePos = gameObject.transform.position;
        if (verticalInput >= 0.2f)
        {
            facingDirection = "up";
            interactTrigger.transform.position = alicePos + new Vector3(0, 0.5f, 0);
            animator.Play("AliceWalkUp");
            // if (!audioSource.isPlaying) {
            //     audioSource.Play();
            // }        
        }

        else if (verticalInput <= -0.2f)
        {
            facingDirection = "down";
            interactTrigger.transform.position = alicePos + new Vector3(0, -0.5f, 0);
            animator.Play("AliceWalkDown");
            // if (!audioSource.isPlaying) {
            //     audioSource.Play();
            // }
        }

        else if (horizontalInput <= -0.2f)
        {
            facingDirection = "left";
            interactTrigger.transform.position = alicePos + new Vector3(-0.5f, 0, 0);
            animator.Play("AliceWalkLeft");
            // if (!audioSource.isPlaying) {
            //     audioSource.Play();
            // }        
        }

        else if (horizontalInput >= 0.2f)
        {
            facingDirection = "right";
            interactTrigger.transform.position = alicePos + new Vector3(0.5f, 0, 0);
            animator.Play("AliceWalkRight");
            // if (!audioSource.isPlaying) {
            //     audioSource.Play();
            // }        
        }
    }

}
