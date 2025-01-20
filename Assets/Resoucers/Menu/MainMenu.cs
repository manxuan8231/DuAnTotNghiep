using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject helpPanel;
    public Slider volumeSlider;
    public int scene = 0;

    void Start()
    {
        settingsPanel.SetActive(false);
        helpPanel.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene(scene);
    }
    public void Setting()
    {
        settingsPanel.SetActive(true);
    }
    public void Help()
    {
        helpPanel.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    public void HidePanel()
    {
        settingsPanel?.SetActive(false);
        helpPanel?.SetActive(false);
    }

}
