using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set; }
    public enum State {
        Wait, ClickedPiece
    };
    public GameObject pieceHighlight;
    public BoardField gameBoard;

    public State currState;
    public GameObject pieceHeld;
    public Piece pieceHit;

    private Vector3 pieceOldPos;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        }
        else{
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currState = InputManager.State.Wait;    
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState) {
            case State.Wait:
                // If left clicked
                if (Input.GetMouseButtonDown(0)) {
                    // Cast a ray from the screen to worldPoint
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                    // If raycast hit an object with a collider
                    if (hit.collider != null) {
                        // Piece pieceHit;

                        // If raycast hit a friendly inactive piece
                        if (hit.collider.gameObject.TryGetComponent<Piece>(out pieceHit)) {
                            ClickPiece(pieceHit);
                        }
                    }
                }

                break;
            case State.ClickedPiece:
                if (Input.GetMouseButtonDown(0)) {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                    // If raycast hit an object with a collider
                    if (hit.collider != null) {
                        // Piece pieceHit;

                        // If raycast hit a friendly inactive piece
                        if (hit.collider.gameObject.TryGetComponent<Piece>(out pieceHit)) {
                            ClickPiece(pieceHit);
                        }
                    } 
                    else {
                        ReleasePiece(mousePos);
                        pieceHit.PlacePiece();
                    }
                }
                break;
            // case State.HoldClick:
            //     pieceHeld.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 5);
            //     break;
            default:
                break;
        }
    }

    public void ClickPiece(Piece pieceClicked) {
        if (!pieceClicked.CanPlace())
        {
            Debug.Log("You don't have enough HOPE!");
            return;
        }
        pieceHeld = pieceClicked.gameObject;
        pieceOldPos = pieceHeld.transform.position;

        pieceHighlight.SetActive(true);
        pieceHighlight.transform.position = pieceHeld.transform.position;

        currState = State.ClickedPiece;
    }

    public void ReleasePiece(Vector3 currPos) {
        if (pieceHeld != null && currState == State.ClickedPiece) {
            
            if (currPos.x > 0 && currPos.x < 7.5f && currPos.y <= 0 && currPos.y > -7.5f) {
                int newX = Mathf.RoundToInt(currPos.x);
                int newY = Mathf.RoundToInt(currPos.y);

                // pieceHeld.transform.position = new Vector3(newX, newY, -5);

                if (!gameBoard.CheckSpaceOccupied(newX, -newY)) {
                    MovePiece(pieceHeld.GetComponent<Piece>(), newX, newY);
                }
            }
            else {
                pieceHeld.transform.position = pieceOldPos;
            }

            pieceHighlight.SetActive(false);
            pieceHeld = null;
        }

        currState = State.Wait;
    }
    
    private void MovePiece(Piece currPiece, int newX, int newY) {
        currPiece.transform.position = new Vector3(newX, newY, -5);
        gameBoard.AddPiece(currPiece, newX, -newY);
    }
}
