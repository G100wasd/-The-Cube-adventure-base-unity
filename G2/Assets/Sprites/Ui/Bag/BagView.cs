using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BagView: MonoBehaviour
{
    public static BagView Instance { get; private set; }

    [SerializeField] public BagManager bagManager;
    [SerializeField] public GameObject CargoMove;
    [SerializeField] public Canvas canvas;

    PackageTable packageTable;
    private void Start()
    { 
        Instance = this;
        packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        BagUpdate();
    }



    public void BagUpdate()
    {
        for (int i = 0; i < bagManager.CargoList.Count; i++)
        {
            Image cargoSprite = bagManager.CargoList[i].GetComponent<Image>();
            TMP_Text amountText = bagManager.CargoList[i].transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            if (bagManager.ItemList[i] == null || bagManager.ItemList[i].amount == 0) 
            { 
                amountText.gameObject.SetActive(false);
                cargoSprite.sprite = null; 
            }
            else
            {
                amountText.gameObject.SetActive(true);
                amountText.text = $"{bagManager.ItemList[i].amount} ";
                cargoSprite.sprite = packageTable.DataList[bagManager.ItemList[i].id].sprite;
            }
        }
    }
    //public void GetCargo(int index, int click) // 需要迁移到controller中
    //{
    //    if (bagManager.ItemList[index] != null)
    //    {

    //        if (!isGetCargo)
    //        {
    //            CargoObj = GameObject.Instantiate(CargoMove, this.transform);
    //            if (cargoItem == null) { cargoItem = new BagManager.Item(0, 1); }
    //            if (click == 0)
    //            {
    //                cargoItem = bagManager.ItemList[index];
    //                bagManager.ItemList[index] = null;
    //            }
    //            else {
    //                cargoItem = new BagManager.Item(bagManager.ItemList[index].id, Convert.ToInt16(Mathf.Ceil(bagManager.ItemList[index].amount / 2)));
    //                bagManager.ItemList[index].amount -= cargoItem.amount;
    //                if (cargoItem.amount == 0) { 
    //                    cargoItem = null;
    //                    GameObject.Destroy(CargoObj);
    //                } // 为什么要写这一个if?
    //                if (bagManager.ItemList[index].amount == 0) { bagManager.ItemList[index] = null; }
    //            }
    //            isGetCargo = true;
    //        }
    //        else
    //        {
    //            if (index == 18) { return; }
    //            if (bagManager.ItemList[index].id == cargoItem.id)
    //            {
    //                if(click == 0)
    //                {
    //                    if (bagManager.ItemList[index].amount + cargoItem.amount > bagManager._maxAmount)
    //                    {
    //                        cargoItem.amount -= (bagManager._maxAmount - bagManager.ItemList[index].amount);
    //                        bagManager.ItemList[index].amount = bagManager._maxAmount;
    //                    }
    //                    else
    //                    {
    //                        bagManager.ItemList[index].amount += cargoItem.amount;
    //                        isGetCargo = false;
    //                        cargoItem = null;
    //                        GameObject.Destroy(CargoObj);
    //                    }
    //                }
    //                else
    //                {
    //                    if (bagManager.ItemList[index].amount + 1 > 64) { return;}
    //                    else
    //                    {
    //                        bagManager.ItemList[index].amount += 1;
    //                        cargoItem.amount -= 1;
    //                        if(cargoItem.amount == 0)
    //                        {
    //                            cargoItem = null;
    //                            GameObject.Destroy(CargoObj);
    //                            isGetCargo = false;
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if (click == 0)
    //                {
    //                    BagManager.Item temp = cargoItem;
    //                    cargoItem = bagManager.ItemList[index];
    //                    bagManager.ItemList[index] = temp;
    //                }

    //            }
    //        }
    //    }
    //    else if (bagManager.ItemList[index] == null && isGetCargo)
    //    {
    //        if (click == 0)
    //        {
    //            if (index == 18) { return; }
    //            bagManager.ItemList[index] = cargoItem;
    //            cargoItem = null;
    //            GameObject.Destroy(CargoObj);
    //            isGetCargo = false;
    //        }
    //        else if (click == 1)
    //        {
    //            bagManager.ItemList[index] = new BagManager.Item(cargoItem.id, 1);
    //            cargoItem.amount -= 1;
    //            if (cargoItem.amount == 0)
    //            {
    //                cargoItem = null;
    //                GameObject.Destroy (CargoObj);
    //                isGetCargo = false;
    //            }
    //        }

    //    }
    //    if (cargoItem != null) { UpdateCargoObj(); }
    //    BagUpdate();
    //}
    //public void CargoFollowMouse()
    //{
    //    if (CargoObj != null && cargoItem != null)
    //    {
    //        if (!UiManager.Instance.isBag && isGetCargo)
    //        {
    //            bagManager.Add(cargoItem.id, cargoItem.amount);
    //            cargoItem = null;
    //            isGetCargo = false;
    //            GameObject.Destroy(CargoObj);
    //            BagUpdate();
    //        }
    //        else if (isGetCargo)
    //        {
    //            mousePos = Input.mousePosition;
    //            CargoObj.transform.position = offsent + mousePos;
    //        }
    //    }

    //}
    //public void UpdateCargoObj()
    //{
    //    offsent = (new Vector2(50, 50) + new Vector2(CargoObjRect.rect.x / 2, CargoObjRect.rect.y / 2)) * canvas.scaleFactor;

    //    Image sprite = CargoObj.GetComponent<Image>();
    //    sprite.sprite = packageTable.DataList[cargoItem.id].sprite;

    //    TMP_Text text = CargoObj.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
    //    text.text = $"{cargoItem.amount}";
    //}
}
