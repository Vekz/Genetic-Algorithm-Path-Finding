using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComp
{
    #region Fields

    /// <summary>
    ///  Virtually increasy quantity of object seen.
    /// </summary>
    public int Weight = 1;


    #endregion

    #region Methods
    
    private float percentCrashed(int howManyCrashed, int generationSize)
    {
        return (float)(Decimal.Round(howManyCrashed / generationSize));
    }
    
    public void ArbitraryIncreaseWeight(float percentCrashed, int blockCount)
    {
       
    }


    #endregion

}
