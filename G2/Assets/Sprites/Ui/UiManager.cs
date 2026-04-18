using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    public bool isBag = false;
    private void Awake()
    {
        Instance = this;
    }

}
