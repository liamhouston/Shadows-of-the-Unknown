using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class PuzzleObject : MonoBehaviour
{
    public Canvas canvas;
    public GameObject puzzlePanel;
    public PuzzlePiece[] pieces;


    private bool playerIsNearby = false;
    [SerializeField]
    private bool puzzleComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(puzzlePanel != null);
        puzzlePanel.SetActive(false);
        string fromScene = SceneManager.GetActiveScene().name + "Puzzle";
        if (PlayerPrefs.GetInt(fromScene) == 1)
        {
            TryGetComponent(out Collider2D collider);
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // start puzzle if player clicks
        if (playerIsNearby && InputManager.Instance.ClickInput && !DialogueManager.Instance.DialogueIsActive() && !puzzlePanel.activeSelf){
            puzzlePanel.SetActive(true);
            InputManager.PlayerInput.actions.FindAction("Move").Disable();
            CursorManager.Instance.MouseColliderSwitch();
        }
        // close panel if user clicks
        else if (puzzleComplete && puzzlePanel.activeSelf && InputManager.Instance.ClickInput){
            puzzlePanel.SetActive(false);
            InputManager.PlayerInput.actions.FindAction("Move").Enable();
            CursorManager.Instance.MouseColliderSwitch();

        }
        // check for puzzle completion
        else if (puzzlePanel.activeSelf && !puzzleComplete)
        {
            foreach (PuzzlePiece piece in pieces)
            {
                if (!piece.inCorrectPosition) return;
            }
            // otherwise puzzle complete
            puzzleComplete = true;
            
            string fromScene = SceneManager.GetActiveScene().name + "Puzzle";
            PlayerPrefs.SetInt(fromScene, 1);
            
            string scene = SceneManager.GetActiveScene().name;
            if (scene != "Darkroom"){
                SoundManager.Instance.PlaySound2D("PuzzleComplete");
            }
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
}

