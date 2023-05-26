using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class TBController : MonoBehaviour
{
    [Header("Motor Power")]
    public float horizontalSpeed;                                                        //Yataydaki h�z
    public float rotateSpeed;                                                            //d�n�� h�z�
    public float rotateAcceleration;

    Rigidbody2D rb;                                                                      //bayraktar�n rigidbodysi           
    float rotateInput;
    int rotateUp;                                                                        //yukar�ya rotate ise 1, a�a��ya rotate ise -1, ikiside de�ilse 0

    [Header("Camera CM")]
    public float normalHeight;
    public float maxHeight;
    public float cameraScaleSpeed;
    
    CinemachineVirtualCamera cmCamera;
    float cameraSize, dffBtwnCamSize;



    readonly float pi_180 = (Mathf.PI / 180);                                                     //hesaplamalar s�rekli yap�lmas� diye burada tutuluyor

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cmCamera = GameObject.Find("Camera CM").GetComponent<CinemachineVirtualCamera>();

        cameraSize = cmCamera.m_Lens.OrthographicSize;
        dffBtwnCamSize = cameraSize - normalHeight;
        rotateUp = 0;


        GoStraight();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))//yukar�ya d�n        
            rotateUp = 1;        
        if (Input.GetKeyUp(KeyCode.W))        
            rotateUp = 0;        

        if (Input.GetKeyDown(KeyCode.S))//asag�ya d�n        
            rotateUp = -1;        
        if (Input.GetKeyUp(KeyCode.S))        
            rotateUp = 0;
    }
    private void FixedUpdate()
    {
        if (rotateUp == 1) rotateInput = Mathf.Lerp(rotateInput, 1, rotateAcceleration);
        else if (rotateUp == -1) rotateInput = Mathf.Lerp(rotateInput, -1, rotateAcceleration);
        else rotateInput = Mathf.Lerp(rotateInput, 0, rotateAcceleration * 0.5f);                           //�steki 2 sat�rdan az olmas�n�n daha yava� dursun diye

        if (transform.position.y > normalHeight && transform.position.y < maxHeight)
        {
            cameraSize = Mathf.Lerp(cameraSize, transform.position.y + dffBtwnCamSize, cameraScaleSpeed);
            cmCamera.m_Lens.OrthographicSize = cameraSize;
        }

        GoStraight();
        transform.Rotate(0, 0, rotateSpeed * rotateInput);
    }


    void GoStraight()
    {
        rb.velocity = new Vector2(horizontalSpeed * Mathf.Cos(transform.eulerAngles.z * pi_180), horizontalSpeed * Mathf.Sin(transform.eulerAngles.z * pi_180));
    }
}



//transform.rotate.z            0 ile 1 aras�ndad�r (180 i ge�tikden sonra -1 den 0 a gider)
//transform.eulerAngle.z ise    0 ile 360 aras�ndad�r

//a��lar radyan olarak al�n�yor heralde bunu sin ve cos form�llerinde kullan - a�� ile carp (Mathf.PI / 180)