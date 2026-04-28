using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    public static ObjectPoolController instance;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] public GameObject _CargoPerfabs;

    private void Awake() {  instance = this; }

    public void AddToPool(GameObject cargo)
    {
        cargo.transform.SetParent(transform);
        cargo.SetActive(false);
        pool.Enqueue(cargo);
    }

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
    }
}
