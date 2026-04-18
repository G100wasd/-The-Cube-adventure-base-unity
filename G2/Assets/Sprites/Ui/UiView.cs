using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiView : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button startButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private RectTransform bagPanel;
    [SerializeField] private RectTransform pausePanel;
    [SerializeField] private GameObject DeadPanel;

    void Start()
    {
        Debug.Log(UiManager.Instance);
    }

    // Update is called once per frame
    public void RegisterStartButton(System.Action onclick)
    {
        startButton.onClick.AddListener(() => onclick?.Invoke());
    }
    public void RegisterExitButton(System.Action onclick)
    {
        ExitButton.onClick.AddListener(() => onclick?.Invoke());
    }
    public void RegisterBackButton(System.Action onclick)
    {
        BackButton.onClick.AddListener(() => onclick?.Invoke());
    }
    public void ShowBagCanvas()
    {
        UiManager.Instance.isBag = !UiManager.Instance.isBag;
        bagPanel.DOAnchorPos(new Vector2((UiManager.Instance.isBag?1:-1)*120, 0), 0.3f);
    }
    public void DestroyStartButton()
    {
        GameObject.Destroy(startButton.gameObject);
    }
    public void ShowPauseCanvas()
    {
        pausePanel.DOAnchorPos(new Vector2(0, GameManager.Instance.isPause?0:360), 0.2f);
    }
    public void ShowDeadPanel()
    {
        pausePanel.DOAnchorPos(new Vector2(0, 0), 0.2f);
        StartCoroutine(wait());
        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.22f);
            GameManager.Instance.isPause = true;
            Time.timeScale = 0;
        }
    }
}
