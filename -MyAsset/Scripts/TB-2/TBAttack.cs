using UnityEngine;

public class TBAttack : MonoBehaviour
{
    [Header("Bullet Mechanics")]
    public GameObject bullet;
    public Transform muzzleBlt;
    public float attckDamageBlt, attackSpeedBlt, lifeTimeBlt, speedBlt;
    PoolingClass bltPool;

    [Header("Roket Mechanics")]
    public GameObject rocket;
    public Transform muzzleRkt;
    public float attckDamageRkt, attackSpeedRkt, lifeTimeRkt, speedRkt;    
    PoolingClass rktPool;

    [Header("Bomb Mechanics")]
    public GameObject bomb;
    public Transform muzzleBmb;
    public float attckDamageBmb, attackSpeedBmb, lifeTimeBmb, speedBmb;
    PoolingClass bmbPool;

    Rigidbody2D rb;
    float atkSpeedBlt, atkSpeedRkt, atkSpeedBmb;
    [HideInInspector]public float jammer;                                         //jammer'ýn içine girince burdaki miktar kadar hatalý mermi atacaz.

    void Start()
    {
        atkSpeedBlt = attackSpeedBlt;
        atkSpeedRkt = attackSpeedRkt;
        atkSpeedBmb = attackSpeedBmb;
        jammer = 0.2f;                                                            //mermi istikametlerindeki normal hata payý
        rb = GetComponent<Rigidbody2D>();

        bltPool = new PoolingClass(bullet);
        rktPool = new PoolingClass(rocket);
        bmbPool = new PoolingClass(bomb);

        bltPool.FillThePool(Mathf.CeilToInt(lifeTimeBlt / attackSpeedBlt), lifeTimeBlt);           //mermi için
        rktPool.FillThePool(Mathf.CeilToInt(lifeTimeRkt / attackSpeedRkt), lifeTimeRkt);           //roket için
        bmbPool.FillThePool(Mathf.CeilToInt(lifeTimeBmb / attackSpeedBmb), lifeTimeBmb);           //bomba için
    }
    void Update()
    {
        if (atkSpeedBlt <= 0)//mermi ateþler
        {
            if (Input.GetMouseButton(0))
            {
                atkSpeedBlt = attackSpeedBlt;
                FireBullet();
            }
        }
        else atkSpeedBlt -= Time.deltaTime;


        if (atkSpeedRkt <= 0)//Roket ateþler
        {
            if (Input.GetMouseButton(1))
            {
                atkSpeedRkt = attackSpeedRkt;
                FireRoket();
            }
        }
        else atkSpeedRkt -= Time.deltaTime;

        if (atkSpeedBmb <= 0)//Bomba ateþler
        {
            if (Input.GetMouseButton(2))
            {
                atkSpeedBmb = attackSpeedBmb;
                FireBomp();
            }
        }
        else atkSpeedBmb -= Time.deltaTime;
    }


    void FireBullet()
    {
        GameObject blt = bltPool.GetObjectFromPool();

        blt.transform.position = muzzleBlt.position;
        blt.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, muzzleBlt.eulerAngles.z + AppJammerErr());
        blt.GetComponent<Rigidbody2D>().velocity = rb.velocity * speedBlt + new Vector2(0, AppJammerErr());

        if (blt.GetComponent<Bullet>().damage == 0)
        {
            blt.GetComponent<Bullet>().lifeTime = lifeTimeBlt;
            blt.GetComponent<Bullet>().damage = attckDamageBlt;
            blt.GetComponent<Bullet>().pool = bltPool;
            blt.GetComponent<Bullet>().whomAmmo = 1;
        }
    }
    void FireRoket()
    {
        GameObject rocket = rktPool.GetObjectFromPool();
        
        rocket.transform.position = muzzleRkt.position;
        rocket.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, muzzleRkt.eulerAngles.z + AppJammerErr());
        rocket.GetComponent<Rigidbody2D>().velocity = rb.velocity * speedRkt + new Vector2(0, AppJammerErr());

        if (rocket.GetComponent<RoketAndBomb>().damage == 0)
        {
            rocket.GetComponent<RoketAndBomb>().lifeTime = lifeTimeRkt;
            rocket.GetComponent<RoketAndBomb>().damage = attckDamageRkt;
            rocket.GetComponent<RoketAndBomb>().pool = rktPool;
            rocket.GetComponent<RoketAndBomb>().whomAmmo = 1;
            rocket.GetComponent<RoketAndBomb>().AreaSet();
        }
    }
    void FireBomp()
    {
        GameObject bmb = bmbPool.GetObjectFromPool();

        bmb.transform.position = muzzleBmb.position;
        bmb.GetComponent<Transform>().rotation = muzzleBmb.rotation;
        bmb.GetComponent<Rigidbody2D>().velocity = rb.velocity * speedBmb;

        if (bmb.GetComponent<RoketAndBomb>().damage == 0)
        {
            bmb.GetComponent<RoketAndBomb>().lifeTime = lifeTimeBmb;
            bmb.GetComponent<RoketAndBomb>().damage = attckDamageBmb;
            bmb.GetComponent<RoketAndBomb>().pool = bmbPool;
            bmb.GetComponent<RoketAndBomb>().whomAmmo = 1;
            bmb.GetComponent<RoketAndBomb>().AreaSet();
        }
    }

    float AppJammerErr()
    {
        return Random.Range(-jammer, jammer);
    }
}

/*
    Eskiden mermi ve roket fonksiyonlarýnda bu satýralr vardý þimdi yok:

float rotZ = (transform.eulerAngles.z + aimErrAmount) * pi_180;
blt.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rotZ), Mathf.Sin(rotZ)) * speedBlt;

    Artýk Aim sistemi ucaðýn hýzýna göre çalýþýyor. !! hýz "0" (sýfýr) ise mermilerde hiç biryere gitmez
 
 
 */