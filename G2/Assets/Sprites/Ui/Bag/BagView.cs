using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BagView : MonoBehaviour
{
    public static BagView Instance { get; private set; }

    [SerializeField] public BagManager bagManager;
    [SerializeField] public GameObject CargoMove;
    [SerializeField] public Canvas canvas;

    PackageTable packageTable;
    private void Awake()
    {
        Instance = this;
        packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
    }

    public void BagUpdate()
    {
        for (int i = 0; i < bagManager.CargoList.Count; i++)
        {
            int index = Convert.ToInt32(bagManager.CargoList[i].name);
            Image cargoSprite = bagManager.CargoList[i].GetComponent<Image>();
            TMP_Text amountText = bagManager.CargoList[i].transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            
            if (bagManager.ItemList[index] == null || bagManager.ItemList[index].amount == 0)
            {
                amountText.gameObject.SetActive(false);
                cargoSprite.sprite = null;
            }
            else
            {
                amountText.gameObject.SetActive(true);
                amountText.text = $"{bagManager.ItemList[index].amount} ";
                cargoSprite.sprite = packageTable.DataList[bagManager.ItemList[index].id].sprite;
            }
        }
    } // 通过cargolist[i]中的每个物体name获取itemlist[name]中的数据并更新
    public void BagScrolleUpdateName(int startIndex)
    {
        for (int i = 3; i < bagManager.CargoList.Count; i++)
        {
            bagManager.CargoList[i].name = $"{i + (startIndex * 4)}";

        }

    } // 根据可视索引更新cargolist中每个物体的名称
}
