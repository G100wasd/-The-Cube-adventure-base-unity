using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.XR;
#endif

public class CharterModel : MonoBehaviour {
    public float _maxHp;
    public float hp;
    public float hit;
    public List<bool> SkillList;
    public event Action<float> onHpChanged;
    public static CharterModel Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        _maxHp = 30;
        hp = _maxHp;
        hit = 2;
        SkillList = new List<bool> { true, true, false, false, false };
    }

    public void ChangeHp(int change = 0, bool func = true)
    {
        hp += change * (func ? 1 : -1);
        if(hp > _maxHp)
        { 
            hp = _maxHp;
        }
        else if(hp == 0)
        {
            return;
        }
            onHpChanged?.Invoke(hp);
    }
}
