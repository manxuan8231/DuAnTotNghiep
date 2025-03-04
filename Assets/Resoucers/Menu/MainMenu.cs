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
    private AudioSource audioSource;
    public AudioClip audioClipPlay;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        settingsPanel.SetActive(false);
        helpPanel.SetActive(false);
    }
    public void Play()
    {
        audioSource.PlayOneShot(audioClipPlay);
        SceneManager.LoadScene(scene);
    }
    public void Setting()
    {
        audioSource.PlayOneShot(audioClipPlay);
        settingsPanel.SetActive(true);
    }
    public void Help()
    {
        audioSource.PlayOneShot(audioClipPlay);
        helpPanel.SetActive(true);
    }
    public void Quit()
    {
        audioSource.PlayOneShot(audioClipPlay);
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
