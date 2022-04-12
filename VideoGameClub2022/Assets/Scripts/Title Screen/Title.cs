using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Button Play, Settings, Back;
    private GameObject StartUI, SettingsUI;

    void Start()
    {
        Play.onClick.AddListener(playGame);
        Settings.onClick.AddListener(gameSettings);
        Back.onClick.AddListener(backButton);

        StartUI = GameObject.Find("StartUI");
        SettingsUI = GameObject.Find("SettingsUI");

        SettingsUI.SetActive(false);

    }
    void playGame()
    {
        SceneManager.LoadScene("Main");
        Debug.Log("Game Playing!");
    }
    void gameSettings()
    {
        StartUI.SetActive(false);
        SettingsUI.SetActive(true);
        Debug.Log("Game Settings!");
    }
    void backButton()
    {
        StartUI.SetActive(true);
        SettingsUI.SetActive(false);
        Debug.Log("Main Menu!");
    }
        //OnPointerEnter
}
