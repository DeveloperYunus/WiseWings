using System.Collections;                                                       // EditorApplication.isPaused = true;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PoolingClass pool;
    public float lifeTime, damage;

    [HideInInspector] public int whomAmmo;                                      //kiminMermisi mermi playerdan cýktýysa = 1, tarretlerden çýktýrysa = 2
                                                                                //mermi cýktýðý objeye hasar vermeyecek
    void OnEnable()
    {
        StartCoroutine(SendBulletToPool(false));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.isTrigger && !other.CompareTag("Player"))                     //katý bir objeye(bina, araba, kutu) carpýnca mermi yok olsun
            StartCoroutine(SendBulletToPool(true));
    }

    public void DestroyBullet()                                                 //mermi bir þeye hasar verince yok olsun
    {
        StartCoroutine(SendBulletToPool(true));
    }
    IEnumerator SendBulletToPool(bool isTrigger)
    {
        if (!isTrigger)
        {
            yield return new WaitForSeconds(lifeTime);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //bullet yok olma effectini oynat                                   //bullet yok olma effect'i bullet prefabýnýn içinde olacak

            yield return new WaitForSeconds(0);                                 //bullet yok olma effect süresi
            pool.SendObjectToPool(gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //bullet yok olma effectini oynat                                   //buleet yok olma effect'i bullet prefabýnýn içinde olacak

            yield return new WaitForSeconds(0);                                 //bullet yok olma effect süresi
            pool.SendObjectToPool(gameObject);
        }
    }
}