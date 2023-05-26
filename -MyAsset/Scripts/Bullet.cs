using System.Collections;                                                       // EditorApplication.isPaused = true;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PoolingClass pool;
    public float lifeTime, damage;

    [HideInInspector] public int whomAmmo;                                      //kiminMermisi mermi playerdan c�kt�ysa = 1, tarretlerden ��kt�rysa = 2
                                                                                //mermi c�kt��� objeye hasar vermeyecek
    void OnEnable()
    {
        StartCoroutine(SendBulletToPool(false));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.isTrigger && !other.CompareTag("Player"))                     //kat� bir objeye(bina, araba, kutu) carp�nca mermi yok olsun
            StartCoroutine(SendBulletToPool(true));
    }

    public void DestroyBullet()                                                 //mermi bir �eye hasar verince yok olsun
    {
        StartCoroutine(SendBulletToPool(true));
    }
    IEnumerator SendBulletToPool(bool isTrigger)
    {
        if (!isTrigger)
        {
            yield return new WaitForSeconds(lifeTime);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //bullet yok olma effectini oynat                                   //bullet yok olma effect'i bullet prefab�n�n i�inde olacak

            yield return new WaitForSeconds(0);                                 //bullet yok olma effect s�resi
            pool.SendObjectToPool(gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //bullet yok olma effectini oynat                                   //buleet yok olma effect'i bullet prefab�n�n i�inde olacak

            yield return new WaitForSeconds(0);                                 //bullet yok olma effect s�resi
            pool.SendObjectToPool(gameObject);
        }
    }
}