
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;


#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

public class BagManager : MonoBehaviour
{
    public int _maxAmount = 64;
    public int _maxId = 4;

    public List<Item> ItemList;
    public List<GameObject> CargoList;
    public int index = 0;
    public float viewStartIndex = 0;

    public static BagManager Instance { get; private set; }

    [SerializeField] public GameObject _CraftCargos;
    [SerializeField] public GameObject _Cargos;
    [SerializeField] public GameObject _CargosPerfab;
    [SerializeField] public GameObject _BagPanel;

    private void Awake()
    {
        Instance = this;
        ItemList = new List<Item>();
        CargoList = new List<GameObject>();
    }

    private void Start()
    {
        initCargo();
    }

    public void initCargo()
    {
        foreach (Transform child in _CraftCargos.transform)
        {
            CargoList.Add(child.gameObject);
        }
        for (int i = 0; i < 24; i++)
        {
            GameObject obj = ObjectPoolController.instance.GetPool(_Cargos);
            obj.SetActive(true);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            CargoList.Add(obj);
        }
        Debug.Log(BagView.Instance);
        for (int i = 0; i < CargoList.Count; i++) { ItemList.Add(null); }
        BagView.Instance.BagScrolleUpdateName(Convert.ToInt32(viewStartIndex));
        BagView.Instance.BagUpdate();
    }

    public class Item
    {
        public int id;
        public int amount;
        public Item(int id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }
    
    public int GetIndex(int id)
    {
        int index = -1;
        for(int i = 0;i < ItemList.Count; i++)
        {
            if (ItemList[i] == null) { continue; }
            else if (ItemList[i].id == id) {index = i;}
        }
        return index;
    }
    public int Add(int id, int amount)
    {
        if(!isFull())
        {
            int _addAmount = 0;
            for (int i = 3; i < ItemList.Count && amount > 0; i++)
            {
                if (ItemList[i] == null)
                {
                    _addAmount = Mathf.Min(amount, _maxAmount);
                    ItemList[i] = new Item(id, _addAmount);
                    amount -= _addAmount;
                }
                else if ( ItemList[i].id == id && ItemList[i].amount < 64)
                {
                    _addAmount = Mathf.Min(amount, (_maxAmount - ItemList[i].amount));
                    ItemList[i].amount += _addAmount;
                    amount -= _addAmount;
                }
            }
            return amount;
        }
        else
        {
            extendCargo();
            Add(id, amount);
        }
        return -1;
        
    }
    public void extendCargo()
    {
        for (int i = 0; i < 4; i++)
        {
            int index = ItemList.Count;
            ItemList.Add(null);
            GameObject obj = GameObject.Instantiate(_CargosPerfab);
            ObjectPoolController.instance.AddToPool(obj);
        }
    }
    public void Decrase(int index)
    {
        ItemList[index].amount-=1;

        if (ItemList[index].amount <= 0)
        {
            ItemList[index] = null;
        }
    }
    public void Clear()
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            ItemList[i] = null;
        }
    }
    public void Sort()
    {
        int index = 0;
        Dictionary<int, int> hashTable = new Dictionary<int, int>();
        foreach(Item item in ItemList)
        {
            if(item == null) {  continue; }
            else
            {
                if (hashTable.ContainsKey(item.id)) { hashTable[item.id] += item.amount; }
                else { hashTable.Add(item.id, item.amount); }
            }
        }
        Clear();
        foreach(var dict in hashTable)
        {
            int id = dict.Key;
            int amount = dict.Value;
            while (amount>0)
            {
                int _addAmount = Mathf.Min(amount, _maxAmount);
                ItemList[index] = new Item(id, _addAmount);
                amount -= _addAmount;
                index++;
            }
        }
    }
    public void Craft()
    {
        //len - 1 == 2
        //len - 2 == 1
        //len - 3 == 0
        Debug.Log("craft");
        int len = ItemList.Count;
        if (ItemList[1] != null && ItemList[0] != null )
        {
            Debug.Log(CharterModel.Instance.SkillList[ItemList[1].id + 1]);
            if (ItemList[1].id == ItemList[0].id)
            {
                int amount = Mathf.Min(ItemList[1].amount, ItemList[0].amount);
                if (ItemList[1].id < _maxId)
                {
                    if (!CharterModel.Instance.SkillList[ItemList[1].id+1]) { return; }
                    if (ItemList[2] == null) 
                    {
                        ItemList[2] = new Item(ItemList[1].id + 1, amount); 
                    }
                    else
                    {
                        if (ItemList[2].amount + amount > _maxAmount) 
                        {
                            amount = _maxAmount - ItemList[2].amount;
                            ItemList[2].amount = _maxAmount;
                        }
                        else 
                        {
                            ItemList[2].amount += amount;
                        }
                    }
                    ItemList[1].amount -= amount;
                    ItemList[0].amount -= amount;
                    if (ItemList[1].amount <= 0) { ItemList[1] = null; }
                    if (ItemList[0].amount <= 0) { ItemList[0] = null; }

                }



            }

        }
    }
    public bool isFull()
    {
        for(int i = 3; i < ItemList.Count; i++)
        {
            if (ItemList[i]==null) { return false; }
            else if (ItemList[i].amount < 64) { return false; }
        }

        return true;
    }
}
