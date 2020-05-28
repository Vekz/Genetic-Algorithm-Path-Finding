using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidersControllers : MonoBehaviour
{
    PopulationController popCon;
    public Slider populationSizeSlider;
    public Slider genomLengthSlider;
    public Slider CutoffSlider;
    public Slider MutationRateSlider;
    public Slider surviovrsKeepSlider;

    void Start()
    {
        popCon = GetComponent<PopulationController>();

        popCon.populationSize = Mathf.FloorToInt(populationSizeSlider.value);
        popCon.genomLength = Mathf.FloorToInt(genomLengthSlider.value);
        popCon.cutoff = CutoffSlider.value;
        popCon.mutationRate = MutationRateSlider.value;
        popCon.survivorKeep = Mathf.FloorToInt(surviovrsKeepSlider.value);

    }

    public void setPopulationSize()
    {
        popCon.populationSize = Mathf.FloorToInt(populationSizeSlider.value);
    }

    public void setGenomLength()
    {
        popCon.genomLength = Mathf.FloorToInt(genomLengthSlider.value);
    }

    public void setCutoff()
    {
        popCon.cutoff = CutoffSlider.value;
    }
    public void setMutationRate()
    {
        popCon.mutationRate = MutationRateSlider.value;
    }

    public void setSurvivorsKeep()
    {
        popCon.survivorKeep = Mathf.FloorToInt(surviovrsKeepSlider.value);
    }
}
