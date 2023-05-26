using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RoketAndBomb : MonoBehaviour
{
    public PoolingClass pool;
    public GameObject areaObject;
    public float lifeTime, damage;
    [SerializeField] float effectArea;

    [HideInInspector] public int whomAmmo;                                      //kiminMermisi mermi playerdan c�kt�ysa = 1, tarretlerden ��kt�rysa = 2
                                                                                //mermi c�kt��� objeye hasar vermeyecek

    void OnEnable()
    {
        StartCoroutine(SendBulletToPool(false));
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && !other.CompareTag("Player"))                     //kat� bir objeye(bina, araba, kutu) carp�nca mermi yok olsun
            StartCoroutine(SendBulletToPool(true));
    }

    public void DestroyRktAndBmb()                                                 //mermi bir �eye hasar verince yok olsun
    {
        StartCoroutine(SendBulletToPool(true));
    }
    void AreaEffect(bool patla)
    {
        if (patla)
        {
            areaObject.SetActive(true);
            areaObject.GetComponent<SpriteRenderer>().DOFade(0, 1);
        }
        else
        {
            areaObject.SetActive(false);
            areaObject.GetComponent<SpriteRenderer>().DOFade(0.5f, 1);
        }
    }
    public void AreaSet()
    {
        areaObject.transform.localScale *= effectArea;
    }

    IEnumerator SendBulletToPool(bool isTrigger)
    {
        if (!isTrigger)
        {
            yield return new WaitForSeconds(lifeTime);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            AreaEffect(true);
            //bullet yok olma effectini oynat                                   //buleet yok olma effect'i bullet prefab�n�n i�inde olacak

            yield return new WaitForSeconds(0);                                 //bullet yok olma effect s�resi
            AreaEffect(false);
            pool.SendObjectToPool(gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            AreaEffect(true);
            //bullet yok olma effectini oynat                                   //buleet yok olma effect'i bullet prefab�n�n i�inde olacak

            yield return new WaitForSeconds(0);                                 //bullet yok olma effect s�resi
            AreaEffect(false);
            pool.SendObjectToPool(gameObject);
        }
    }
}