using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CargoController : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerMoveHandler,IPointerEnterHandler
{
    [SerializeField] public GameObject description;
    GameObject BagPanel;
    GameObject des;
    Camera uiCamera;
    Canvas canvas;
    TMP_Text title;
    TMP_Text content;
    PackageTable packageTable;
    Image color;

    float canvasScale;
    bool isDescription = false;
    Vector2 desPos;
    private void Start()
    {
        uiCamera = Camera.main;
        BagPanel = this.transform.parent.parent.parent.gameObject;
        canvas = BagPanel.transform.parent.gameObject.GetComponent<Canvas>();
        color = this.gameObject.GetComponent<Image>();
        packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        int index = Convert.ToInt16(this.name);
        int click = 0;
        if (eventData.button == PointerEventData.InputButton.Left) { click = 0; }
        else if (eventData.button == PointerEventData.InputButton.Right) { click = 1; }
        BagView.Instance.GetCargo(index, click);
        color.DOColor(new Color(256, 256, 256, 160) / 256f, 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(des != null)
        {
            GameObject.Destroy(des);
            isDescription = false;
        }
        color.DOColor(new Color(256, 256, 256, 160)/256f, 0.2f);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (BagManager.Instance.ItemList[Convert.ToInt16(this.name)] != null)
        {
            if (!isDescription)
            {
                des = GameObject.Instantiate(description, BagPanel.transform);
                isDescription = true;

                BagManager.Item item = BagManager.Instance.ItemList[Convert.ToInt16(this.name)];
                title = des.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
                content = des.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();

                title.text = packageTable.DataList[item.id].name;
                content.text = packageTable.DataList[item.id].description;
            }
            RectTransform desTran = des.GetComponent<RectTransform>();
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(desTran, Input.mousePosition, uiCamera, out desPos);
            //desTran.anchoredPosition = desPos;

            Vector2 mousePos = Input.mousePosition;
            canvasScale = canvas.scaleFactor;
            desTran.position = new Vector2(5, -5) + new Vector2(desTran.rect.width / 2, desTran.rect.height / -2) * canvasScale + mousePos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (BagManager.Instance.ItemList[Convert.ToInt16(this.name)] != null)
        {
            color.DOColor(new Color(160, 160, 160, 160) / 256f, 0.2f);
        }
    }
}
