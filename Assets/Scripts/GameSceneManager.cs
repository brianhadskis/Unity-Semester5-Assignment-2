using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSceneManager : MonoBehaviour
{

    public TMP_Text tmPlayerName;
    public GameObject player;
    public GameObject MountainSpawn;
    public static bool isMountainStart = false;

    private Transform cameraMain;

    private AudioSource[] audio;

    public AudioClip[] music;
    private int selectedMusic;

    public GameObject explosionSource;
    public AudioClip explosionSound;
    public AudioClip scoreSound;
    public AudioClip clickSound;

    public TMP_Text tmScore;
    public TMP_Text tmHighScore;
    public TMP_Text tmTimeLeft;

    public TMP_Text checkpointUI;

    public GameObject gameoverDialog;
    public TMP_Text gameoverText;

    private int MAX_TIME = 30;
    private int SCORE_INCREMENT = 5;
    private int currentTime;
    private int currentScore;
    private int highScore;

    public GameObject EndTrigger;

    private bool timeRunning;

    public void GoHome()
    {
        audio[1].PlayOneShot(clickSound);
        SceneManager.LoadScene("HomeScene");
    }

    private void Awake()
    {
        audio = Camera.main.GetComponents<AudioSource>();
        currentScore = 0;
        currentTime = MAX_TIME;

        if (isMountainStart)
        {
            player.transform.position = MountainSpawn.transform.position;
            player.transform.rotation = MountainSpawn.transform.rotation;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        cameraMain = Camera.main.transform;
        if(PlayerPrefs.HasKey("PlayerName"))
        {
            tmPlayerName.text = PlayerPrefs.GetString("PlayerName");
        }

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            highScore = 0;
        }

        tmHighScore.text = "" + highScore;

        if (PlayerPrefs.HasKey("GameMusic"))
        {
            selectedMusic = PlayerPrefs.GetInt("GameMusic");
        }
        else
        {
            selectedMusic = 0;
        }

        audio[0].clip = music[selectedMusic];
        audio[0].loop = true;
        audio[0].Play();

        StartCoroutine("LoseTime");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreCard();
        if (EndTrigger.GetComponent<GameOver>().GetTriggered())
        {
            timeRunning = false;
            gameoverDialog.SetActive(true);
            gameoverText.text = "Game Over\nYou WIN!";
            player.GetComponent<PlayerController>().Winner();
            SetHighScore();
        }
    }

    void LateUpdate()
    {
        tmPlayerName.transform.LookAt(cameraMain);
    }

    public void DoCheckpoint()
    {
        currentTime += 30;
        currentScore += 30;

        tmScore.GetComponent<Animator>().SetTrigger("ScoreAddedTrigger");
        checkpointUI.GetComponent<Animator>().SetTrigger("CheckpointReached");
    }

    IEnumerator LoseTime()
    {
        timeRunning = true;

        while (timeRunning)
        {
            yield return new WaitForSeconds(1);

            currentTime--;

            if (currentTime % 5 == 0)
            {
                currentScore += SCORE_INCREMENT;
                tmScore.GetComponent<Animator>().SetTrigger("ScoreAddedTrigger");
            }
            
            if (currentTime <= 0)
            {
                break;
            }

            if (currentTime < 11)
            {
                tmTimeLeft.GetComponent<Animator>().SetBool("lowTime", true);
            }
            else
            {
                tmTimeLeft.GetComponent<Animator>().SetBool("lowTime", false);
            }

            /*if (currentTime % 6 == 0 && currentTime != MAX_TIME)
            {
                Vector2 randomLocation = Random.insideUnitCircle * 50;
                explosionSource.transform.position.Set(
                    player.transform.position.x + randomLocation.x,
                    player.transform.position.y,
                    player.transform.position.z + randomLocation.y);

                explosionSource.GetComponent<AudioSource>().PlayOneShot(explosionSound);
            }*/

            /*if (currentTime % 10 == 0 && currentTime != MAX_TIME)
            {
                audio[1].PlayOneShot(scoreSound);
            }*/
        }

        if (currentTime <= 0)
        {
            GameOver();
        }
        
    }

    private void GameOver()
    {
        player.GetComponent<PlayerController>().MakeDead();

        SetHighScore();

        gameoverDialog.SetActive(true);
        gameoverText.text = "Game Over\nYou're Dead!";

    }

    private void SetHighScore()
    {
        if (highScore < currentScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }

    private void UpdateScoreCard()
    {
        tmScore.text = "" + currentScore;
        tmTimeLeft.text = "" + currentTime;
    }
}
