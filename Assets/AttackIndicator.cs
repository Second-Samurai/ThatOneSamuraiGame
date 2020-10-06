using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackIndicator : MonoBehaviour
{
    Image indicator;
    bool bisShowing = false;


    // Start is called before the first frame update
    void Awake()
    {
        indicator = GetComponent<Image>();
        indicator.fillAmount = 0;
    }

    public void ShowIndicator()
    {
        if (GameManager.instance.bShowAttackPopups)
        {
            bisShowing = true;
            indicator.fillAmount = 0;
        }
    }

    public void HideIndicator()
    {
        bisShowing = false;
        indicator.fillAmount = 0;
    }
 
    private void Update()
    {
        if (bisShowing)
        {
            if (indicator.fillAmount != 1)
            {
                indicator.fillAmount += Time.deltaTime * 4;
            }
            else
            {
                HideIndicator();
            }
        }
    }
}
