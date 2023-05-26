using System.Collections;
using UnityEngine;

public class TarretAttack : MonoBehaviour
{
    readonly float pi_180 = (Mathf.PI / 180);

    [Header("Tarret")]
    public Transform target;
    public float range;
    public float attackSpeed;
    public float rotateSpeedMultiplier;
    public float checkTime;

    [Header("Ammo")]
    public GameObject ammoType;
    public float damage;
    public float speed;                                                 //bullet move speed
    public float lifeTimeBlt;


    PoolingClass enmyBltPool;
    Transform tarretObject, muzzleObject;
    Vector2 magnitudeHolder;                                             //hem uzaklýðýn karesini tutar hemde turret'in nereye döneceðini tutar
    Animator anim;
    bool isTrgtInRng, canAttack, oneCorontine;                           //coldown corountine'si tek sefer calýþsýn diye             
    float sqrRange, atkSpeed;
    int tarretType;


    private void Start()
    {
        isTrgtInRng = false;
        canAttack = true;
        oneCorontine = true;
        sqrRange = range * range;
        atkSpeed = attackSpeed;

        if (ammoType.name == "Bullet") tarretType = 1;
        else tarretType = 2;

        anim = GetComponent<Animator>();
        target = GameObject.Find("TarretTargetPivot").transform;    
        tarretObject = transform.GetChild(0);
        muzzleObject = tarretObject.GetChild(0);
        enmyBltPool = new PoolingClass(ammoType);
        enmyBltPool.FillThePool(Mathf.CeilToInt(lifeTimeBlt / attackSpeed + 2), lifeTimeBlt);

        InvokeRepeating(nameof(TargetControl), 0, checkTime);
    }
    private void Update()
    {
        if (isTrgtInRng)
        {
            tarretObject.up = Vector2.Lerp(tarretObject.up, magnitudeHolder, Time.deltaTime * rotateSpeedMultiplier);

            if (atkSpeed <= 0)
            {
                atkSpeed = attackSpeed;
                if (tarretType == 1)//bullet
                    AttackBullet();
                else if (tarretType == 2)//Rocket
                    AttackRocket();
            }
            else atkSpeed -= Time.deltaTime;
        }        
    }

    void TargetControl()
    {
        if (target)
        {
            magnitudeHolder = target.position - tarretObject.position;
            if (Vector2.SqrMagnitude(magnitudeHolder) <= sqrRange && target.position.y > transform.position.y)
                isTrgtInRng = true;
            else
                isTrgtInRng = false;
        }
        else isTrgtInRng = false;
    }
    void AttackBullet()
    {
        if (canAttack)
        {
            anim.Play("bulletTrtAttack");
            float rotZ = (tarretObject.eulerAngles.z + 90) * pi_180;
            GameObject blt = enmyBltPool.GetObjectFromPool();
            blt.transform.position = muzzleObject.position;
            blt.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, tarretObject.eulerAngles.z + 90);
            blt.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rotZ), Mathf.Sin(rotZ)) * speed;

            if (blt.GetComponent<Bullet>().damage == 0)
            {
                blt.GetComponent<Bullet>().lifeTime = lifeTimeBlt;
                blt.GetComponent<Bullet>().damage = damage;
                blt.GetComponent<Bullet>().pool = enmyBltPool;
                blt.GetComponent<Bullet>().whomAmmo = 2;                  // 2 enemyler (yani tarretlerden çýkan mermiler için)
            }                                                             //hem player hemde tarretler için ayný prefablarý saðlýklý bir þekilde kullanmak için
            if (oneCorontine)
                StartCoroutine(BulletColdown());
        }
    }
    void AttackRocket()
    {
        anim.Play("rocketTrtAttack");
        float rotZ = (tarretObject.eulerAngles.z + 90) * pi_180;
        GameObject rkt = enmyBltPool.GetObjectFromPool();
        rkt.transform.position = muzzleObject.position;
        rkt.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, tarretObject.eulerAngles.z + 90);
        rkt.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rotZ), Mathf.Sin(rotZ)) * speed;

        if (rkt.GetComponent<RoketAndBomb>().damage == 0)
        {
            rkt.GetComponent<RoketAndBomb>().lifeTime = lifeTimeBlt;
            rkt.GetComponent<RoketAndBomb>().damage = damage;
            rkt.GetComponent<RoketAndBomb>().pool = enmyBltPool;
            rkt.GetComponent<RoketAndBomb>().whomAmmo = 2;
            rkt.GetComponent<RoketAndBomb>().AreaSet();
        }
    }


    IEnumerator BulletColdown()
    {
        oneCorontine = false;
        yield return new WaitForSeconds(3.5f);
        canAttack = false;
        yield return new WaitForSeconds(2f);
        oneCorontine = true;
        canAttack = true;
    }
    private void OnDrawGizmosSelected()//turretin etrafýnda daire çizer (sadece görüþte kolaylýk olsun diye)
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
