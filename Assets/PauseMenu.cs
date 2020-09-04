using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    float timeScale;
    public GameObject optionsMenu;

    private void OnEnable()
    {
        timeScale = Time.timeScale;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeButton()
    {
        Time.timeScale = timeScale;
        gameObject.SetActive(false);
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }
}
