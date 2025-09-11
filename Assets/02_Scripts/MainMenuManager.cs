using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject optionsPanel;
    public AudioClip normalButtonSXF;
    public AudioClip exitButtonSFX;
    public AudioSource sfxAS;

    public void StartGame()
    {
        sfxAS.PlayOneShot(normalButtonSXF);
        SceneManager.LoadScene("Lv1");
    }

    public void OpenOptionsPanel()
    {
        sfxAS.PlayOneShot(normalButtonSXF);
        //cargar informacion
        optionsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        sfxAS.PlayOneShot(exitButtonSFX);
        Application.Quit();
    }
}