using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class drag : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform panelRect;
    public RectTransform correctPosition;
    public int snapDistance;

    private bool inCorrectPosition = false;

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
            correctPosition.position,
            canvas.worldCamera,
            out correctPositionInRect);

        float dist = Vector2.Distance(position, correctPositionInRect);
        
        if (dist < snapDistance){
            // piece is in correct location
            inCorrectPosition = true;
            
            transform.position = canvas.transform.TransformPoint(correctPositionInRect); // change transform to correct pos
        }

    }

    
    // Clamps the position within the bounds of the panel
    private Vector2 ClampPosition(Vector2 position, Rect panelBounds)
    {
        Vector2 clampedPosition = position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, panelBounds.xMin, panelBounds.xMax);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, panelBounds.yMin, panelBounds.yMax);
        return clampedPosition;
    }
}
