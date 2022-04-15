using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseButton : MonoBehaviour
{
    public Button resume, settings, exit;
    private GameObject pauseUI;

    void Start()
    {
        resume.onClick.AddListener(resumeGame);
        settings.onClick.AddListener(gameSettings);
        exit.onClick.AddListener(quitGame);

        pauseUI = GameObject.Find("Pause");
    }
    void resumeGame()
    {
        

        pauseUI.SetActive(false);
    }
    void gameSettings()
    {
        
    }
    void quitGame()
    {
        //Save game, etc
        SceneManager.LoadScene("Title");
    }
    //OnPointerEnter
}
