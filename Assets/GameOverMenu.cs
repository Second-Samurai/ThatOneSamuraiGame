using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image blackImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TextFadeIn()
    {
        text.DOFade(1, 5.0f);
        blackImage.DOFade(1, 5.0f);

    }

    public void TextFadeOut()
    {
        text.DOFade(0, 5.0f);


    }

    // Update is called once per frame
    void Update()
    {
        //if (Keyboard.current.lKey.wasPressedThisFrame)
        //{
        //    TextFadeIn();
        //    Invoke("ReturnToMenu", 10f);
        //    Invoke("TextFadeOut", 5f);
        //}
    }

    public void ReturnToMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        blackImage.DOFade(0, 5.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
