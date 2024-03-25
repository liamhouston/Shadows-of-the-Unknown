using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerHelper : MonoBehaviour
{
    private LevelManager _levelManagerHelper;
    private void Start()
    {
        _levelManagerHelper = LevelManager.Instance;
    }

    public void LoadBedroomScene()
    {
        _levelManagerHelper.LoadScene("Bedroom", "CrossFade");
        MusicManager.Instance.PlayMusic("Bedroom");
    }

    public void LoadBedroomCameScene()
    {
        _levelManagerHelper.LoadScene("BedroomCam", "CrossFade");
        MusicManager.Instance.PlayMusic("Bedroom");
    }

    public void LoadFishdockScene()
    {
        _levelManagerHelper.LoadScene("Fishdock", "CrossFade");
        MusicManager.Instance.PlayMusic("Fishdock");
    }
    public void LoadCampsiteScene()
    {
        _levelManagerHelper.LoadScene("Campsite", "CrossFade");
        MusicManager.Instance.PlayMusic("Campsite");
    }
    public void LoadStoreScene()
    {
        _levelManagerHelper.LoadScene("Store", "CrossFade");
        MusicManager.Instance.PlayMusic("Store");
    }
    public void LoadDarkroomScene()
    {
        _levelManagerHelper.LoadScene("Darkroom", "CrossFade");
        MusicManager.Instance.PlayMusic("Darkroom");
    }
}
