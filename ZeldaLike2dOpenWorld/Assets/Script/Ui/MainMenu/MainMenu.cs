using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject settingsWindow;
    public GameObject mainWindow;
    public void GameStart(){
        Debug.Log("GameStart called");
        SceneManager.LoadScene(levelToLoad);
        Debug.Log("Loading" + levelToLoad);
    }

    public void TestButton(){
    Debug.Log("Button Clicked!");
}


    public void SettingsButton(){
        settingsWindow.SetActive(true);
        mainWindow.SetActive(false);
    }

    public void BackSettingsButton(){
        settingsWindow.SetActive(false);
        mainWindow.SetActive(true);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
