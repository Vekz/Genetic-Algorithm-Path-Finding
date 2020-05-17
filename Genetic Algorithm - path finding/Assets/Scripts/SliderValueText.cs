using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueText : MonoBehaviour
{
    Slider slider;
    Text txt;
    string DefMess;

    void Start()
    {
        slider = GetComponentInParent<Slider>();
        txt = GetComponent<Text>();
        DefMess = txt.text;
        txt.text += slider.value.ToString();
    }

    // Start is called before the first frame update
    public void SetSliderTextValue()
    {
        
        txt.text = DefMess + slider.value.ToString();
    }
}
