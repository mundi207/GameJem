using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private float Speed, JumpPow;

    private Slider HPbar;

    [SerializeField]
    private GameObject HPbarPrefab;
    [SerializeField]
    private GameObject CanvasObj;

    private Rigidbody2D rbody2D;
    private RectTransform rectTransform;

    private Vector2 playerPos;
    private bool isGround;

    private float HPvalue;

    private void Awake()
    {
        GameObject hpbar = Instantiate(HPbarPrefab, transform.position, Quaternion.identity);
        HPbar = hpbar.GetComponentInChildren<Slider>();

        HPbar.transform.SetParent(CanvasObj.transform);

        HPvalue = 1f;
        HPbar.value = HPvalue;

        rbody2D = GetComponent<Rigidbody2D>();
        rectTransform = HPbar.GetComponent<RectTransform>();

        playerPos = playerObj.transform.position;
    }
    private void Update()
    {
        playerPos = playerObj.transform.position;
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 3f, LayerMask.GetMask("Ground"));
    }
    private void FixedUpdate()
    {
        if(isGround)
        {
            rbody2D.AddForce(Vector2.up * JumpPow, ForceMode2D.Impulse);
        }
        transform.position = Vector2.Lerp(transform.position, playerPos, Time.deltaTime);
        rectTransform.position = new Vector3(transform.position.x, transform.position.y + 1f,0);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
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
