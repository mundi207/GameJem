using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody2D;

    [SerializeField]
    private GameObject HPbarPrefab;
    [SerializeField]
    private GameObject CanvasObj;
    private Slider HPbar;

    private float HPvalue;

    private RectTransform rectTransform;

    [SerializeField]
    private float Speed;
    private float axisH;

    private void Awake()
    {
        rbody2D = GetComponent<Rigidbody2D>();

        GameObject hpbar = Instantiate(HPbarPrefab,transform.position,Quaternion.identity);
        HPbar = hpbar.GetComponentInChildren<Slider>();
        HPbar.transform.SetParent(CanvasObj.transform);

        rectTransform = HPbar.GetComponent<RectTransform>();

        HPvalue = 1f;
        HPbar.value = HPvalue;
    }
    private void Update()
    {
        axisH = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        rbody2D.velocity = Vector2.right * axisH * Speed; // 좌우이동
        rectTransform.position = new Vector3(transform.position.x,transform.position.y + 1f,0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Bullet");
            StartCoroutine(OnDamage());
        }
    }
    IEnumerator OnDamage()
    {
        while(HPvalue > 0)
        {
            HPvalue -= 0.1f;
            HPbar.value = HPvalue;
            yield return new WaitForSeconds(1.5f);
        }
        if(HPvalue == 0)
            StopCoroutine(OnDamage());
    }
}
