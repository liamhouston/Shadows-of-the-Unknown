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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(puzzlePanel != null);
        puzzlePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNearby && InputManager.Instance.ClickInput && !puzzlePanel.activeSelf){
            // start puzzle if player clicks
            puzzlePanel.SetActive(true);
        }

        if (puzzlePanel.activeSelf){
            foreach (PuzzlePiece piece in pieces){
                if (!piece.inCorrectPosition) return;
            }
            // otherwise puzzle complete

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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

