using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectHealth : MonoBehaviour
{
    [Header("Object Health")]
    public Slider healthSlider;
    public TextMeshProUGUI hpTxt;
    public float health;


    void Start()
    {
        if (healthSlider)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
            hpTxt.text = health.ToString();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && other.GetComponent<Bullet>().whomAmmo != 2)
        {
            GetDamage(other.GetComponent<Bullet>().damage);
            other.GetComponent<Bullet>().DestroyBullet();
        }

        if (other.CompareTag("RktBmb") && other.GetComponent<RoketAndBomb>().whomAmmo != 2)
        {
            other.GetComponent<RoketAndBomb>().DestroyRktAndBmb();
        }
        if (other.CompareTag("AreaEffect"))
        {
            GetDamage(other.gameObject.GetComponentInParent<RoketAndBomb>().damage);
        }
    }

    void GetDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;

            if (healthSlider)
            {
                healthSlider.value = health;
                hpTxt.text = health.ToString();
            }

            if (health <= 0)
            {
                Die();
            }
        }
    }
    void Die()
    {
        Destroy(gameObject, 0f);

        //üstteki Destroy satýrýný sil                              
        //patlama effectini oynat                                                   patlama effet'i tarret'in içinde olacak 
        //effect oynarken tarret setactive = false olsun    
        //Destroy(object, patlama effect'i süresi)
    }
}
