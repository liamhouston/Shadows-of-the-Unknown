using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [SerializeField] private Texture2D clickableCursorTexture;
    [SerializeField] private Texture2D defaultCursorTexture;

    private bool isDefault;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {   
        SetDefaultCursor();
    }

    public void SetDefaultCursor(){
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void SetClickableCursor(){
        Cursor.SetCursor(clickableCursorTexture, Vector2.zero, CursorMode.Auto);
    }
}