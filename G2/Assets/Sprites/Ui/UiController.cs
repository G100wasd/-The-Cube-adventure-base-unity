using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UiController : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField] private UiManager uiManager ;
    [SerializeField] private UiView uiView;

    public static event Action OnGameStart;

    void Start()
    {
        uiView.RegisterStartButton(onStartClick);
        uiView.RegisterExitButton(onExitClick);
        uiView.RegisterBackButton(onBackClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !GameManager.Instance.isPause)
        {
            uiView.ShowBagCanvas();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.gameState != GameManager.GameState.Start)
        {
            ChangePause();
        }
    }
    public void onStartClick()
    {
        //GameObject.Destroy(uiView);
        uiView.DestroyStartButton();
        OnGameStart?.Invoke();
    }
    public void onExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        
    }
    public void onBackClick()
    {
        GameManager.Instance.isPause = false;
        uiView.ShowPauseCanvas();
    }
    public void ChangePause()
    {
        GameManager.Instance.isPause = !GameManager.Instance.isPause;
        if(!GameManager.Instance.isPause) { Time.timeScale = 1.0f; }
        uiView.ShowPauseCanvas();
        StartCoroutine(wait()); 
        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.22f); 
            if (GameManager.Instance.isPause) { Time.timeScale = 0f; }
        };
    }
}
