using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static Button playPauseButton;
    private static Button resetLevelButton;
    private static Sprite pauseImage;
    private static Sprite playImage;
    private static Transform timer;
    private static Transform stickers;
    private static SushiMovement[] sushis;
    private static RobotPickup[] robots;
    private static RobotLine[] robotLines;

    public static bool isGamePaused;
    private static int totalNumberOfSushi;
    private static int currentNumberOfSushi;

    private static Transform winScreen;

    private void Awake() {
        isGamePaused = true;
        totalNumberOfSushi = FindObjectsOfType<SushiMovement>().Length;
        currentNumberOfSushi = 0;

        playPauseButton = GameObject.Find("PlayPauseButton").GetComponent<Button>();
        resetLevelButton = GameObject.Find("ResetLevelButton").GetComponent<Button>();
        playImage = Resources.Load<Sprite>("Sprites/UI/Play");
        pauseImage = Resources.Load<Sprite>("Sprites/UI/Pause");
        timer = GameObject.Find("Timer").transform;
        stickers = GameObject.Find("Stickers").transform;
        sushis = FindObjectsOfType<SushiMovement>();
        robots = FindObjectsOfType<RobotPickup>();
        robotLines = FindObjectsOfType<RobotLine>();

        playPauseButton.onClick.AddListener(() => { 
            if (isGamePaused) {
                playPauseButton.GetComponent<Image>().sprite = pauseImage;
                Play();
            }
            else {
                playPauseButton.GetComponent<Image>().sprite = playImage;
                Pause();
            }
        });

        resetLevelButton.onClick.AddListener(() => {
            Reset();
        });

        if (SceneManager.GetActiveScene().buildIndex == 7) {
            winScreen = GameObject.Find("WinScreen").transform;
            winScreen.transform.gameObject.SetActive(false);
        }
    }

    public static void Play() {
        isGamePaused = false;
        timer.GetComponent<Animator>().Play("TimerIn");
        stickers.GetComponent<Animator>().Play("StickersOut");
    }

    public static void Pause() {
        isGamePaused = true;
        currentNumberOfSushi = 0;
        timer.GetComponent<Animator>().Play("TimerOut");
        stickers.GetComponent<Animator>().Play("StickersIn");
        timer.GetComponent<Timer>().ResetTimer();

        for (int i = 0; i < sushis.Length; i++) {
            sushis[i].ResetPosition();
        }

        for (int i = 0; i < robots.Length; i++) {
            robots[i].ResetPosition();
        }

        for (int i = 0; i < robotLines.Length; i++) {
            robotLines[i].ResetPosition();
        }
    }

    public static void IncreaseNumberOfSushi() {
        currentNumberOfSushi++;

        if (currentNumberOfSushi == totalNumberOfSushi) {
            isGamePaused = true;
            NextLevel();
        }
    }

    private static void NextLevel() {
        if (SceneManager.GetActiveScene().buildIndex < 7) {
            GameObject.Find("LevelComplete").GetComponent<Animator>().Play("LevelComplete");
        }

        if (SceneManager.GetActiveScene().buildIndex == 7) {
            winScreen.transform.gameObject.SetActive(true);
            winScreen.GetComponent<Animator>().Play("Win");
        }
    }

    public static void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
