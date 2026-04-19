
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    
    public static BagManager Instance { get; private set; }

    [SerializeField] public GameObject Cargos;
    [SerializeField] public GameObject _CraftCargos;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ItemList = new List<Item>();
        CargoList = new List<GameObject>();
        foreach(Transform child in Cargos.transform){ CargoList.Add(child.gameObject);}
        foreach(Transform child in _CraftCargos.transform) {  CargoList.Add(child.gameObject);}
        for(int i = 0; i < CargoList.Count; i++) { ItemList.Add(null); }
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
            for (int i = 0; i < ItemList.Count - 3 && amount > 0; i++)
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
        return -1;
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
        Debug.Log("craft");
        int len = ItemList.Count;
        if (ItemList[len-2] != null && ItemList[len-3] != null )
        {
            Debug.Log(CharterModel.Instance.SkillList[ItemList[len - 2].id + 1]);
            if (ItemList[len-2].id == ItemList[len - 3].id)
            {
                int amount = Mathf.Min(ItemList[len - 2].amount, ItemList[len-3].amount);
                if (ItemList[len - 2].id < _maxId)
                {
                    if (!CharterModel.Instance.SkillList[ItemList[len - 2].id+1]) { return; }
                    if (ItemList[len - 1] == null) 
                    {
                        ItemList[len - 1] = new Item(ItemList[len - 2].id + 1, amount); 
                    }
                    else
                    {
                        if (ItemList[len - 1].amount + amount > _maxAmount) 
                        {
                            amount = _maxAmount - ItemList[len - 1].amount;
                            ItemList[len - 1].amount = _maxAmount;
                        }
                        else 
                        {
                            ItemList[len - 1].amount += amount;
                        }
                    }
                    ItemList[len - 2].amount -= amount;
                    ItemList[len - 3].amount -= amount;
                    if (ItemList[len - 2].amount <= 0) { ItemList[len - 2] = null; }
                    if (ItemList[len - 3].amount <= 0) { ItemList[len - 3] = null; }

                }



            }

        }
    }
    public bool isFull()
    {
        for(int i = 0; i < ItemList.Count - 3; i++)
        {
            if (ItemList[i]==null) { return false; }
        }

        return true;
    }
}
