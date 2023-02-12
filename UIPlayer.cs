using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private Slider sliderFillPuck;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image imageFillPuck;

    public void SetStrongPuck(float strongPuck)
    {
        if (strongPuck > 1) strongPuck = 1f;
        if (strongPuck < 0) strongPuck = 0;
        if (strongPuck == 0)
        {
            sliderFillPuck.gameObject.SetActive(false);
            return;
        } else
        {
            sliderFillPuck.gameObject.SetActive(true);
        }
        sliderFillPuck.value = strongPuck;
        imageFillPuck.color = gradient.Evaluate(strongPuck);

    }
}
