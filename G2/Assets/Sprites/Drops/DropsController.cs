using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.UIElements;
#endif


public class DropsController : MonoBehaviour
{
    private Transform tran;
    private PackageTable packageTable;
    private SpriteRenderer image;
    private Sequence sq;
    private BagManager.Item item;

    [SerializeField] public TMP_Text text;
    public int id;
    public int amount;
    void Start()
    {
        packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        tran = this.gameObject.transform;
        image = this.gameObject.GetComponent<SpriteRenderer>();
        item = new BagManager.Item(id, amount);
        Updateview();

        sq = DOTween.Sequence();
        sq.Append(tran.DOMoveY(transform.position.y + 0.2f, 1.5f));
        sq.Append(tran.DOMoveY(transform.position.y - 0.2f, 1.5f));
        sq.SetLoops(-1, LoopType.Restart);


    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.deltaTime;
        RotateY(t);
    }
    private void RotateY(float t)
    {
        tran.Rotate(new Vector3(0, 45 * t, 0));
    }

    private void Updateview()
    {
        if (item != null)
        {
            image.sprite = packageTable.DataList[item.id].sprite;
            text.text = $"{item.amount}";
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Player")
            {
                
                int isAdd = BagManager.Instance.Add(item.id, item.amount);
                if(isAdd <=0 ) 
                {
                    sq.Kill();
                    GameObject.Destroy(this.gameObject); 
                }
                else
                {
                    item.amount = isAdd;
                    Updateview();
                }
                BagView.Instance.BagUpdate();
            }
        }
    }

}
