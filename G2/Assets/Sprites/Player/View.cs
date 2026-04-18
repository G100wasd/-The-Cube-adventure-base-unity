using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class CharterView : MonoBehaviour
{
    Transform parentTran;
    SpriteRenderer color;

    [SerializeField] public CharterModel charterModel;
    [SerializeField] public Transform faceTran;


    private void Start()
    {
        parentTran = this.transform.parent;
        color = this.GetComponent<SpriteRenderer>();
    }
    public void FaceMove(float dir)
    {
        Vector2 Pos = this.transform.position;
     
        faceTran.position = new Vector2(Pos.x + (dir * 0.22f), Pos.y);
    }
    public void UpdateHpView(float hp)
    {
        color.DOColor(new Color(1, 1, 1, (hp / charterModel._maxHp)), 0.1f);

    }
}
