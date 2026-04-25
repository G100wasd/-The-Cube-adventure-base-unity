using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState gameState;
    public bool isPause;
    void Start()
    {
        Instance = this;
        isPause = true;
        Time.timeScale = 0;
        gameState = GameState.Start;

        SceneManager.LoadScene("UiScene", LoadSceneMode.Additive);

        UiController.OnGameStart += GameStart;
    }

    public enum GameState
    {
        Start,
        Game
    }

    private void GameStart()
    {
        gameState = GameState.Game;
        isPause = false;
        Time.timeScale = 1;
    }
}
