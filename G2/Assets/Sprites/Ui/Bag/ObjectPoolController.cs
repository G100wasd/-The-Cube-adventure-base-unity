using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPoolController : MonoBehaviour
{
    public static ObjectPoolController instance;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] public GameObject _CargoPerfabs;

    private void Awake() 
    {
        instance = this;

        #region 对象池预热24个格子
        for (int i = 0; i < 24; i++)
        {
            GameObject obj = GameObject.Instantiate(_CargoPerfabs, this.transform);
            obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            obj.GetComponent<Image>().sprite = null;
            obj.transform.GetChild(0).GetComponent<TMP_Text>().text = null;
            obj.SetActive(false);
            AddToPool(obj);
        }
        #endregion

    }

    public void AddToPool(GameObject cargo)
    {
        cargo.transform.SetParent(transform);
        cargo.SetActive(false);
        pool.Enqueue(cargo);
    } // 添加obj到对象池中

    public GameObject GetPool(GameObject cargolist)
    {
        GameObject obj;
        if(pool.Count > 0)
        {
            obj =  pool.Dequeue();
            obj.transform.SetParent(cargolist.transform);
            obj.SetActive(true);

        }
        else
        {
            obj = GameObject.Instantiate(_CargoPerfabs, cargolist.transform);
            obj.SetActive(true);
        }

        return obj;
    } // 从对象池中取出obj，若不存在则创建新obj
}
