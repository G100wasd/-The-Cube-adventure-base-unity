using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BagContrtoller : MonoBehaviour
{
    BagManager bagManager;
    BagView bagView;
    GameObject getCargoObj;
    BagManager.Item getCargoItem;
    PackageTable packageTable;

    bool isGetCargo = false;
    Vector2 offset;
    Vector2 mousePos;
    Rect getCargoRect;

    [SerializeField] public Button craftBtn;
    [SerializeField] public GameObject getCargoPanel;
    [SerializeField] public Canvas canvas;

    public static BagContrtoller Instance;

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bagManager.Sort();
            bagView.BagUpdate();
        }

        GetCargoMove();
    }

    private void Init()
    {
        Instance = this;
        packageTable = Resources.Load<PackageTable>("TableData/PackageTable");

        bagManager = BagManager.Instance;
        bagView = BagView.Instance;

        craftBtn.onClick.AddListener(bagManager.Craft);
        craftBtn.onClick.AddListener(bagView.BagUpdate);

        getCargoRect = getCargoPanel.GetComponent<RectTransform>().rect;
        getCargoItem = new BagManager.Item(0, 0);

    }
    public void GetCargo(int index, int click)
    {
        if (bagManager.ItemList[index] != null)
        {
            if (!isGetCargo)
            {
                getCargoObj = GameObject.Instantiate(getCargoPanel, this.transform);
                if(click == 0)
                {
                    getCargoItem = bagManager.ItemList[index];
                    bagManager.ItemList[index] = null;
                }
                else if(click == 1)
                {
                    getCargoItem = new BagManager.Item(bagManager.ItemList[index].id, Convert.ToInt16(Mathf.Ceil(bagManager.ItemList[index].amount/2)));
                    bagManager.ItemList[index].amount -= getCargoItem.amount;
                    if (bagManager.ItemList[index].amount  <= 0) { bagManager.ItemList[index] = null ; }
                }
                isGetCargo = true;
            }
            else if (isGetCargo)
            {
                if(index == 0) { return; }
                if(click == 0)
                {
                    if(getCargoItem.id == bagManager.ItemList[index].id)
                    {
                        int gap =  64 - bagManager.ItemList[index].amount;
                        bagManager.ItemList[index].amount = Mathf.Min(64, bagManager.ItemList[index].amount + getCargoItem.amount);
                        getCargoItem.amount -= gap;
                        if(getCargoItem.amount <= 0) { isGetCargoItemlNull(); }
                    }
                    else
                    {
                        BagManager.Item temp;

                        temp = bagManager.ItemList[index];
                        bagManager.ItemList[index] = getCargoItem;
                        getCargoItem = temp;
                    }
                }
                else if(click == 1)
                {
                    if (bagManager.ItemList[index].id == getCargoItem.id)
                    {
                        if (bagManager.ItemList[index].amount < 64)
                        {
                            bagManager.ItemList[index].amount += 1;
                            getCargoItem.amount -= 1;
                            isGetCargoItemlNull() ;
                        }
                    }
                }
            }
        }
        else if (bagManager.ItemList[index] == null)
        {
            if (isGetCargo) 
            { 
                if(click == 0)
                {
                    bagManager.ItemList[index] = getCargoItem;
                    getCargoItem = null;
                    isGetCargo = false;
                    GameObject.Destroy(getCargoObj);
                }
                else if (click == 1)
                {
                    bagManager.ItemList[index] = new BagManager.Item(getCargoItem.id, 1);
                    getCargoItem.amount -= 1;
                    isGetCargoItemlNull();
                }
            }
        }
    }
    private void isGetCargoItemlNull()
    {
        if (getCargoItem != null && getCargoItem.amount <= 0)
        {
            isGetCargo = false;
            getCargoItem = null;
            GameObject.Destroy(getCargoObj);
        }
    }
    private void GetCargoMove()
    {
        if (!isGetCargo)
        {
            if (getCargoObj != null)
            {
                GameObject.Destroy(getCargoObj);
                getCargoItem = null;
            }
            return;
        }
        else if (!UiManager.Instance.isBag && isGetCargo)
        {
            bagManager.Add(getCargoItem.id, getCargoItem.amount);
            getCargoItem = null;
            isGetCargo = false;
            GameObject.Destroy(getCargoObj);
            bagView.BagUpdate();
            return;
        }
        offset = (new Vector2(25 + (getCargoRect.width/2), 25 + (getCargoRect.height)/2)) * canvas.scaleFactor;
        mousePos = Input.mousePosition;
        GetCargoUpdate();
    }
    private void GetCargoUpdate()
    {
        if(getCargoObj != null)
        {
            Image sprite = getCargoObj.GetComponent<Image>();
            TMP_Text text = getCargoObj.transform.GetChild(0).GetComponent<TMP_Text>();

            getCargoObj.transform.position = mousePos + offset;
            sprite.sprite = packageTable.DataList[getCargoItem.id].sprite;
            text.text = $"{getCargoItem.amount}";
        }
    }



}
