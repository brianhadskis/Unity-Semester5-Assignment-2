using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsSceneManager : MonoBehaviour
{
    public TMP_InputField inputName;
    public TMP_Dropdown musicSelector;

    public AudioClip clickSound;

    public void GoHome()
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(clickSound);
        PlayerPrefs.SetString("PlayerName", inputName.text);
        SceneManager.LoadScene("HomeScene");
    }

    public void MusicSelect(int value)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(clickSound);
        PlayerPrefs.SetInt("GameMusic", value);
    }

    public void ResetHighScore()
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(clickSound);
        PlayerPrefs.SetInt("HighScore", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            inputName.text = PlayerPrefs.GetString("PlayerName");
        }

        if (PlayerPrefs.HasKey("GameMusic"))
        {
            musicSelector.value = PlayerPrefs.GetInt("GameMusic");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
