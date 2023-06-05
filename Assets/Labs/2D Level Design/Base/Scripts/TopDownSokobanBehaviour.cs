using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSokobanBehaviour : MonoBehaviour
{
    // Internal Parameters
    private int _crates;
    private int _goals;

    // Check if Sokoban puzzle complete
    public bool puzzleComplete;

    // Start is called before the first frame update
    void Start()
    {
        puzzleComplete = false;

        VerifyCrates();
    }

    void VerifyCrates()
    {
        _goals = GameObject.FindGameObjectsWithTag("Goal").Length;
        _crates = GameObject.FindGameObjectsWithTag("Crate").Length;

        if (_crates != _goals)
        {
            Debug.LogError(("Number of crates and goals for the puzzle are not equal. \nNumber of Crates: " + _crates + " Number of Goals: " + _goals));
        }
    }

    public void DecrementGoals()
    {
        _goals--;

        if (_goals <= 0)
        {
            Debug.Log("Puzzle complete!");
            puzzleComplete = true;
        }
    }

    public void DecrementCrates()
    {
        _crates--;
    }
}
