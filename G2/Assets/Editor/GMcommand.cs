using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GMcmd
{
    [MenuItem("GMcmd/读取表格")]
    public static void ReadTable()
    {
        PackageTable packageTable = Resources.Load<PackageTable>("TableData/PackageTable");
        foreach (var item in packageTable.DataList)
        {
            Debug.Log(item.id);
            Debug.Log(item.name);
        }
    }

    [MenuItem("GMcmd/读取背包")]
    public static void ReadBag()
    {
        foreach(var item in BagManager.Instance.ItemList)
        {
            if(item == null) { continue; }
            else { Debug.Log($"id:{item.id}, amount:{item.amount}"); }
        }
    }

    [MenuItem("GMcmd/添加背包")]
    public static void AddBag()
    {
        int num = 30;
        BagManager.Instance.Add(0, num);
        BagView.Instance.BagUpdate();
    }
    [MenuItem("GMcmd/添加me控制器")]
    public static void AddME()
    {
        BagManager.Instance.Add(6, 1);
        BagView.Instance.BagUpdate();
    }
}
