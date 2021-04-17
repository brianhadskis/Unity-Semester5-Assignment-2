using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeSceneManager : MonoBehaviour
{
    public AudioClip clickSound;
    public void GoToGame(bool isMountainStart)
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(clickSound);
        GameSceneManager.isMountainStart = isMountainStart;
        SceneManager.LoadScene("GameScene");
    }

    public void GoToSettings()
    {
        Camera.main.GetComponent<AudioSource>().PlayOneShot(clickSound);
        SceneManager.LoadScene("SettingsScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
