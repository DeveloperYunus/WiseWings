using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AimJammer : MonoBehaviour
{
    public float jammerRange;
    [Tooltip("Value is Angle")]
    public float aimErrValue;

    float defaultAimErr;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = jammerRange;   
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            defaultAimErr = other.GetComponent<TBAttack>().jammer;
            other.GetComponent<TBAttack>().jammer = aimErrValue;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<TBAttack>().jammer = defaultAimErr;
        }
    }
}