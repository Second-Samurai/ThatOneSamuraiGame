using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIGuardMeter : MonoBehaviour
{
    public Image guardImage;

    private Transform _entityTransform;
    private StatHandler _statHandler;

    // Start is called before the first frame update
    public void Init(Transform entityTransform, StatHandler statHandler)
    {
        this._entityTransform = entityTransform;
        this._statHandler = statHandler;
    }

    public void UpdateMeter()
    {
        //Check wither guard meter is full or not

        

        /*if (inputThrust == 0)
        {
            thrustImage.fillAmount = Mathf.Lerp(thrustImage.fillAmount, 0.5f, 0.05f);
        }
        else if (inputThrust > 0)
        {
            thrustImage.fillAmount = Mathf.Lerp(thrustImage.fillAmount, 1, 0.05f);
        }
        else
        {
            thrustImage.fillAmount = Mathf.Lerp(thrustImage.fillAmount, 0, 0.05f);
        }*/
        Debug.Log(">> GaurdMeter: Guard At = " + _statHandler.CurrentGuard);
    }

    public void SetMeterPosition()
    {

    }

}
