using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class drag : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform panelRect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragHandler(BaseEventData data){
        PointerEventData pointerData = (PointerEventData) data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);

        position = ClampPosition(position, panelRect.rect);
        //Debug.Log("Original Position " + position + " clamped to " + transform.position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    // Clamps the position within the bounds of the panel
    private Vector2 ClampPosition(Vector2 position, Rect panelBounds)
    {
        Vector2 clampedPosition = position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, panelBounds.xMin, panelBounds.xMax);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, panelBounds.yMin, panelBounds.yMax);
        Debug.Log("Positoin: " + position + "Rect XMin: " + panelBounds.xMin + ", XMax: " + panelBounds.xMax + ", YMin: " + panelBounds.yMin + ", YMax: " + panelBounds.yMax);
        return clampedPosition;
    }
}
