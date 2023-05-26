using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class TBController : MonoBehaviour
{
    [Header("Motor Power")]
    public float horizontalSpeed;                                                        //Yataydaki hýz
    public float rotateSpeed;                                                            //dönüþ hýzý
    public float rotateAcceleration;

    Rigidbody2D rb;                                                                      //bayraktarýn rigidbodysi           
    float rotateInput;
    int rotateUp;                                                                        //yukarýya rotate ise 1, aþaðýya rotate ise -1, ikiside deðilse 0

    [Header("Camera CM")]
    public float normalHeight;
    public float maxHeight;
    public float cameraScaleSpeed;
    
    CinemachineVirtualCamera cmCamera;
    float cameraSize, dffBtwnCamSize;



    readonly float pi_180 = (Mathf.PI / 180);                                                     //hesaplamalar sürekli yapýlmasý diye burada tutuluyor

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
        if (Input.GetKeyDown(KeyCode.W))//yukarýya dön        
            rotateUp = 1;        
        if (Input.GetKeyUp(KeyCode.W))        
            rotateUp = 0;        

        if (Input.GetKeyDown(KeyCode.S))//asagýya dön        
            rotateUp = -1;        
        if (Input.GetKeyUp(KeyCode.S))        
            rotateUp = 0;
    }
    private void FixedUpdate()
    {
        if (rotateUp == 1) rotateInput = Mathf.Lerp(rotateInput, 1, rotateAcceleration);
        else if (rotateUp == -1) rotateInput = Mathf.Lerp(rotateInput, -1, rotateAcceleration);
        else rotateInput = Mathf.Lerp(rotateInput, 0, rotateAcceleration * 0.5f);                           //Üsteki 2 satýrdan az olmasýnýn daha yavaþ dursun diye

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



//transform.rotate.z            0 ile 1 arasýndadýr (180 i geçtikden sonra -1 den 0 a gider)
//transform.eulerAngle.z ise    0 ile 360 arasýndadýr

//açýlar radyan olarak alýnýyor heralde bunu sin ve cos formüllerinde kullan - açý ile carp (Mathf.PI / 180)