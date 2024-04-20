using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class RespondToDarkRoomPuzzle : MonoBehaviour
{
    [Header ("Camera Shake Info")]
    [SerializeField] private float max_amplitude;
    [SerializeField] private float max_frequency;
    [SerializeField] private CinemachineVirtualCamera CinemachineVirtualCamera;

    [Header ("Puzzle zoom info")]
    public RectTransform panel;
    public RectTransform finishPanelScaleandLoc;
    public float move_seconds;
    public float wait_seconds;
    private bool zoomStarted = false;
    private bool zoomComplete = false;

    private float num_correct_pieces = 0;
    private bool startedCutscene = false;

    private Vector3 originalScale;
    private Vector3 originalPosition;

    private void Start()
    {
        originalScale = panel.localScale;
        originalPosition = panel.localPosition;
    }

    void Update(){
        // if player has solved a new piece. increase shake and play breathing
        if (num_correct_pieces != PuzzleObject.Instance.GetNumCorrectPieces()){
            num_correct_pieces = PuzzleObject.Instance.GetNumCorrectPieces();

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = (num_correct_pieces / PuzzleObject.Instance.GetNumPieces()) * max_amplitude;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = (num_correct_pieces / PuzzleObject.Instance.GetNumPieces()) * max_frequency;

            SoundManager.Instance.PlaySound2D("Heartbeating");
        }

        // start zoom if puzzle complete
        if (!zoomStarted && PuzzleObject.Instance.puzzleComplete) {
            zoomStarted = true;

            StartCoroutine(ScaleAndMoveCoroutine());
        }

        // if the zoom is complete and not loaded cutscene yet
        if (!startedCutscene && zoomComplete){
            startedCutscene = true;

            StartCoroutine(LoadCutsceneAfterSeconds(1));
        }
    }

    private IEnumerator ScaleAndMoveCoroutine()
    {
        float elapsedTime = 0f;
        SoundManager.Instance.PlaySound2D("Heartbeating");

        while (elapsedTime < move_seconds)
        {
            float t = elapsedTime / move_seconds;

            // Lerping scale
            panel.localScale = Vector3.Lerp(originalScale, finishPanelScaleandLoc.localScale, t);

            // Lerping position
            panel.localPosition = Vector3.Lerp(originalPosition, finishPanelScaleandLoc.localPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it ends up at the exact position and scale
        panel.localScale = finishPanelScaleandLoc.localScale;
        panel.localPosition = finishPanelScaleandLoc.localPosition;
        yield return new WaitForSeconds(wait_seconds);

        SoundManager.Instance.PlaySound2D("scrape");
        zoomComplete = true;
    }


    private IEnumerator LoadCutsceneAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        // load cutscene
        LevelManager.Instance.LoadScene("EndCutscene", "CrossFade");
    }
}
