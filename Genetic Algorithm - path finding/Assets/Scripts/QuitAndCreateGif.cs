﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Drawing;
public class QuitAndCreateGif : MonoBehaviour
{
    /// <summary>
    /// Pauses the time scale and exits
    /// /// </summary>
    public void quit()
    {

        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        Application.Quit();
    }

}