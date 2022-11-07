using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {get; private set; }
    public enum State {
        Wait, HoldClick
    };

    public State currState;
    public GameObject pieceHeld;

    private Vector3 pieceOldPos;

    private void Awake(){
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        }
        else{
            Instance = this;
        }
        //maybe here, we can set the 5 building cards.
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
                    Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(r, out hit);

                    // If raycast hit an object with a collider
                    if (hit.collider != null) {
                        Piece pieceHit;
                        Debug.Log("Clicked on an object");

                        // If raycast hit a friendly inactive piece
                        if (hit.collider.gameObject.TryGetComponent<Piece>(out pieceHit)) {
                            Debug.Log("Clicked on a piece");
                        }
                    }
                }

                break;
            case State.HoldClick:
                pieceHeld.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 5);
                break;
            default:
                break;
        }
    }

    public void ClickPiece(Piece pieceClicked) {
        pieceHeld = pieceClicked.gameObject;
        pieceOldPos = pieceHeld.transform.position;

        currState = State.HoldClick;
    }

    public void ReleasePiece() {
        if (pieceHeld != null && currState == State.HoldClick) {
            Vector3 currPos = pieceHeld.transform.position;
            
            if (currPos.x > 0 && currPos.x < 7.5f && currPos.y <= 0 && currPos.y > -7.5f) {
                int newX = Mathf.RoundToInt(currPos.x);
                int newY = Mathf.RoundToInt(currPos.y);

                pieceHeld.transform.position = new Vector3(newX, newY, -5);
            }
            else {
                pieceHeld.transform.position = pieceOldPos;
            }

            pieceHeld = null;
        }

        currState = State.Wait;
    }
}
