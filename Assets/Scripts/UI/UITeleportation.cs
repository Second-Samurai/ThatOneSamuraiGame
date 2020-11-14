using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITeleportation : MonoBehaviour
{
    [Header("Flash")]
    public Image teleportationFlash;

    [Header("TextMesh")]
    public TextMeshProUGUI successfulText;
    public TextMeshProUGUI unsucessfulText;
    public TextMeshProUGUI helperText;

    private Color flashColor;

    // Start is called before the first frame update
    void Start()
    {
        flashColor = Color.white;
        flashColor.a = 0;
    }

    public bool RampFlashOn()
    {
        //Debug.Log("Screen is brightening up");
        if (flashColor.a < 1)
        {
            flashColor.a += 0.005f;
            flashColor.a = Mathf.Clamp(flashColor.a, 0, 1);
            teleportationFlash.color = flashColor;
            //Debug.Log(flashColor.a);
            return true;
        }

        teleportationFlash.color = flashColor;
        return false;
    }


    public bool RampFlashOff()
    {
        //Debug.Log("Screen is ramping down");
        if (flashColor.a > 0)
        {
            flashColor.a -= 0.005f;
            flashColor.a = Mathf.Clamp(flashColor.a, 0, 1);
            teleportationFlash.color = flashColor;
           // Debug.Log(flashColor.a);
            return true;
        }

        flashColor.a = 0;
        teleportationFlash.color = flashColor;
        return false;
    }

    public void ConsoleSuccessful()
    {
        Debug.Log("Console Successful");
        StartCoroutine(UITextScreenReveal(successfulText, 3.5f));
    }

    public void ConsoleUnsucessful()
    {
        Debug.Log("Console Unsucessful");
        StartCoroutine(UITextScreenReveal(unsucessfulText, 3.5f));
    }

    public void HelperText()
    {
        Debug.Log("Display Helper");
        StartCoroutine(UITextScreenReveal(helperText, 3.5f));
    }

    public IEnumerator UITextScreenReveal(TextMeshProUGUI textUI, float revealTime)
    {
        textUI.gameObject.SetActive(true);
        float timer = revealTime/2f;
        Color textColor = Color.white;
        textColor.a = 0;

        while (timer > 0)
        {
            textColor.a += 0.05f;
            textColor.a = Mathf.Clamp(textColor.a, 0, 1);
            textUI.color = textColor;
            timer -= 0.005f;
            yield return null;
        }

        timer = revealTime / 2f;

        while (timer > 0)
        {
            textColor.a -= 0.01f;
            textColor.a = Mathf.Clamp(textColor.a, 0, 1);
            textUI.color = textColor;
            timer -= 0.005f;
            yield return null;
        }
    }
}
