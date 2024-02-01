using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolveBar : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    public void SetMaxResolve(int resolve)
    {
        slider.maxValue = resolve;
        slider.value = resolve;
    }

    // Update is called once per frame
    public void SetResolve(int resolve)
    {
        slider.value = resolve;
    }

    public void ChangeResolve(int change)
    {
        slider.value += change;
    }
}
