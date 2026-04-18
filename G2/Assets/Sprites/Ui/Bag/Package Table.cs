using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="PackageTable", fileName = "PackageTable")]

public class PackageTable : ScriptableObject
{
    public List<CargoItem> DataList = new List<CargoItem>();
}

[System.Serializable]
public class CargoItem
{
    public int id;
    public string name;
    public string description;
    public Sprite sprite;
}
