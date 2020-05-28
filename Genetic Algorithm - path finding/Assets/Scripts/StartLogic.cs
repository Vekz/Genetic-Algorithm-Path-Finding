using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// WARING!!! CHANGING UI STRUCTURE MAY RENDER THIS LOGIC UNUSABLE
/// </summary>


public class StartLogic : MonoBehaviour
{
    public PopulationController PopCon;
    public List<Slider> Sliders = new List<Slider>();

    public void StartSimulation()
    {

        PopCon.InitPopulation();

        foreach (Slider s in Sliders)
        {
            s.interactable = false;
        }
    }
}
