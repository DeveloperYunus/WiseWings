using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TBHealth : MonoBehaviour
{
    [Header("Health System")]
    public Slider healthSlider;
    public TextMeshProUGUI hpTxt;
    public float health;


    void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        hpTxt.text = health.ToString();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && other.GetComponent<Bullet>().whomAmmo != 1)    //mermi yada roket carparsa ve benim mermim deðillerse hasar al
        {
            GetDamage(other.GetComponent<Bullet>().damage);
            other.GetComponent<Bullet>().DestroyBullet();                 
        }

        if (other.CompareTag("RktBmb") && other.GetComponent<RoketAndBomb>().whomAmmo != 1)
        {
            other.GetComponent<RoketAndBomb>().DestroyRktAndBmb();
        }
        if (other.CompareTag("AreaEffect"))
        {
            GetDamage(other.gameObject.GetComponentInParent<RoketAndBomb>().damage);
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<Collider2D>().isTrigger)                 //herhangi bir "somut" alana çarparsam öl
        {
            GetDamage(health + 1);
        }
    }

    void GetDamage(float damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
            healthSlider.value = health;
            hpTxt.text = health.ToString();
        }
    }
    void Die()
    {
        Destroy(gameObject, 0.1f);
        //patlama effectini oynat
        //patlama effect'i süresi + 0.5 sn
        //restart yada ana menü paneli cýksýn
    }
}
