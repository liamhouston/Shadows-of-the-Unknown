using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleObject : MonoBehaviour
{
    public Canvas canvas;
    public GameObject puzzlePanel;
    public PuzzlePiece[] pieces;


    private bool playerIsNearby = false;
    private bool puzzleComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(puzzlePanel != null);
        puzzlePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // start puzzle if player clicks
        if (playerIsNearby && InputManager.Instance.ClickInput && !puzzlePanel.activeSelf){
            puzzlePanel.SetActive(true);
        }
        // close panel if user clicks
        else if (puzzleComplete && puzzlePanel.activeSelf && InputManager.Instance.ClickInput){
            puzzlePanel.SetActive(false);
        }
        // check for puzzle completion
        else if (puzzlePanel.activeSelf && !puzzleComplete){
            foreach (PuzzlePiece piece in pieces){
                if (!piece.inCorrectPosition) return;
            }
            // otherwise puzzle complete
            puzzleComplete = true;
            SoundManager.Instance.PlaySound2D("PuzzleComplete");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mouse"))
        {
            playerIsNearby = false;
        }
    }

    public void TestFunction(){
        Debug.Log("event triggered function");
    }

    public void DragHandler(BaseEventData data){
        PointerEventData pointerData = (PointerEventData) data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        transform.position = canvas.transform.TransformPoint(position);
    }
}

