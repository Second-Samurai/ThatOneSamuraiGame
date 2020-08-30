using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimeThreasholdReferance 
{ 

    public bool UseConstant = true;
    public float ConstantValue;
    public TimeThreasholdVariable Variable;

    public float TimeThreasholdValue
    {
        get 
        {
            return UseConstant ? ConstantValue : Variable.TimeThreashold;
        }
    }

}
