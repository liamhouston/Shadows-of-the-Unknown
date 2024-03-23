using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform panelRect;
    public GameObject correctPiece;
    public int snapDistance;

    public bool inCorrectPosition = false;

    public void DragHandler(BaseEventData data){
        // if the piece is in the correct position, we can't move it
        if (inCorrectPosition) return;

        PointerEventData pointerData = (PointerEventData) data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        position = ClampPosition(position, panelRect.rect);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void DropHandler(BaseEventData data){
        // drop piece in correct position if close enough
        PointerEventData pointerData = (PointerEventData) data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        Vector2 correctPositionInRect;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            correctPiece.GetComponent<RectTransform>().position,
            canvas.worldCamera,
            out correctPositionInRect);

        float dist = Vector2.Distance(position, correctPositionInRect);
        
        if (dist < snapDistance){
            // piece is in correct location
            inCorrectPosition = true;
            // disable self and make correct position visible
            gameObject.SetActive(false);
            Image correctPieceImg = correctPiece.GetComponent<Image>();
            correctPieceImg.color = new Color(correctPieceImg.color.r, correctPieceImg.color.g, correctPieceImg.color.b, 1);
        }

    }

    
    // Clamps the position within the bounds of the panel
    private Vector2 ClampPosition(Vector2 position, Rect panelBounds) {
        Vector2 clampedPosition = position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, panelBounds.xMin, panelBounds.xMax);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, panelBounds.yMin, panelBounds.yMax);
        return clampedPosition;
    }
}
