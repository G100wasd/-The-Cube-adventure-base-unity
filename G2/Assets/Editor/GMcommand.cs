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

    [MenuItem("GMcmd/一次性添加8组")]
    public static void AddFour()
    {
        for (int i = 0; i < 8; i++)
        {
            BagManager.Instance.Add(Random.Range(0, 3), 64);
        }
        BagView.Instance.BagUpdate();
    }

    [MenuItem("GMcmd/一次性添加随机奇数组")]
    public static void AddRandomOdd()
    {
        int num = Random.Range(100, 150)*2+1;
        for (int i = 0; i < num; i++)
        {
            BagManager.Instance.Add(Random.Range(0, 3), 64);
        }
        BagView.Instance.BagUpdate();
        Debug.Log($"当前背包有{BagManager.Instance.ItemList.Count - 3}个元素");
    }
}
