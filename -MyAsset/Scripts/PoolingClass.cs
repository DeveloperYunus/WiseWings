using System.Collections.Generic;
using UnityEngine;

public class PoolingClass
{
    GameObject prefab;
    Stack<GameObject> pool = new();

    public PoolingClass(GameObject prefab)//bir poolingClass caðýrdýðýmýzda direk prefabý atayacak
    {
        this.prefab = prefab;
    }

    public void FillThePool(int amount, float lifeTime)      //bullet = 1, roket = 2, bomba = 3
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obje = Object.Instantiate(prefab);
            if (obje.GetComponent<Bullet>())
                obje.GetComponent<Bullet>().lifeTime = lifeTime;
            else if (obje.GetComponent<RoketAndBomb>())
                obje.GetComponent<RoketAndBomb>().lifeTime = lifeTime;

            SendObjectToPool(obje);
        }
    }

    public GameObject GetObjectFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obje = pool.Pop();
            obje.SetActive(true);

            return obje;
        }
        return Object.Instantiate(prefab);
    }
    public void SendObjectToPool(GameObject obje)
    {
        obje.SetActive(false);
        pool.Push(obje);
    }
}
