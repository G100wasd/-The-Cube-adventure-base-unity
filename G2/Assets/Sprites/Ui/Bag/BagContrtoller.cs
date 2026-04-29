using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagContrtoller : MonoBehaviour, IScrollHandler
{
    BagManager bagManager;
    BagView bagView;
    GameObject getCargoObj;
    BagManager.Item getCargoItem;
    PackageTable packageTable;
    

    float scrollerSpeed;
    bool isGetCargo = false;
    Vector2 offset;
    Vector2 mousePos;
    Rect getCargoRect;


    [SerializeField] public Button craftBtn;
    [SerializeField] public GameObject getCargoPanel;
    [SerializeField] public Canvas canvas;
    [SerializeField] public RectTransform cargoListViewRect;
    [SerializeField] public GameObject cargos;

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

        scrollerSpeed = 4.0f;
        cargoListViewRect.anchoredPosition = new Vector2(0, 20);
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
    } // 背包中鼠标左键/右键的相关逻辑
    private void isGetCargoItemlNull()
    {
        if (getCargoItem != null && getCargoItem.amount <= 0)
        {
            isGetCargo = false;
            getCargoItem = null;
            GameObject.Destroy(getCargoObj);
        }
    } // 如果格子为空 移除
    #region 格子拿取时的相关操作
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
    #endregion


    public void OnScroll(PointerEventData eventData)
    {
        float delta = eventData.scrollDelta.y * scrollerSpeed;
        float top = 20; 
        float bottom = ((bagManager.ItemList.Count - 3) / 4) - 5; // 规定最大可视索引

        if (delta > 0 && cargoListViewRect.anchoredPosition.y < top) { return; }
        else if (delta < 0 && bagManager.viewStartIndex >= bottom && cargoListViewRect.anchoredPosition.y >= 36) { return; }
        // 若达到滚动最顶部，退出 || 若已达最大可视范围，退出

        cargoListViewRect.anchoredPosition = new Vector2(0, cargoListViewRect.anchoredPosition.y - delta);

        if (delta < 0)
        {
            if (cargoListViewRect.anchoredPosition.y >= 72)
            {
                for (int i = 0; i < 4; i++)
                {
                    ObjectPoolController.instance.AddToPool(bagManager.CargoList[Convert.ToInt16(i + 3)]);
                    bagManager.CargoList.RemoveAt(i + 3);
                    GameObject obj = ObjectPoolController.instance.GetPool(cargos);
                    obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    obj.transform.GetChild(0).GetComponent<TMP_Text>().gameObject.SetActive(false);
                    bagManager.CargoList.Add(obj);
                }
                bagManager.viewStartIndex += 1;
                cargoListViewRect.anchoredPosition = new Vector2(0, 16);
                bagView.BagScrolleUpdateName(Convert.ToInt32(bagManager.viewStartIndex));
                bagView.BagUpdate();
            }


        }
        else if (delta > 0 && bagManager.viewStartIndex > 0)
        {
            if (cargoListViewRect.anchoredPosition.y <= 24)
            {
                for (int i = 0; i < 4; i++)
                {
                    ObjectPoolController.instance.AddToPool(bagManager.CargoList[i + 23]);
                    bagManager.CargoList.RemoveAt(i + 23);
                    GameObject obj = ObjectPoolController.instance.GetPool(cargos);
                    obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    obj.transform.GetChild(0).GetComponent<TMP_Text>().gameObject.SetActive(false);
                    bagManager.CargoList.Add(obj);
                }
                bagManager.viewStartIndex -= 1;
                cargoListViewRect.anchoredPosition = new Vector2(0, 76);
                bagView.BagScrolleUpdateName(Convert.ToInt32(bagManager.viewStartIndex));
                bagView.BagUpdate();
            }

        }

    } // 鼠标滚动逻辑 无限滚动容器实现

}
