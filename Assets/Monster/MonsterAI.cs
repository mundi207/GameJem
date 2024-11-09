using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObj; // 플레이어 오브젝트 저장
    [SerializeField]
    private float Speed, JumpPow; // 몬스터 속도, 날아오르는 힘 정의

    private Slider HPbar; // HP바

    [SerializeField]
    private GameObject HPbarPrefab; // HP바에 쓸 슬라이더 프리팹
    [SerializeField]
    private GameObject CanvasObj; // 캔버스 오브젝트 저장

    private Rigidbody2D rbody2D;
    private RectTransform rectTransform; // UI이므로 RectTransform

    private Vector2 playerPos;
    private bool isGround; // 몬스터가 땅에 닿았는지 여부 확인

    private float HPvalue; // HP값 

    private void Awake()
    {
        GameObject hpbar = Instantiate(HPbarPrefab, transform.position, Quaternion.identity); // 프리팹 오브젝트로 생성
        HPbar = hpbar.GetComponentInChildren<Slider>(); // 생성한 오브젝트로 슬라이더 객체 생성

        HPbar.transform.SetParent(CanvasObj.transform); // HPbar를 캔버스 밑으로 생성함

        HPvalue = 1f; // 초기값 1f (범위 : 0 ~ 1)
        HPbar.value = HPvalue; // 초기값 슬라이더에 적용

        rbody2D = GetComponent<Rigidbody2D>(); 
        rectTransform = HPbar.GetComponent<RectTransform>(); // RectTransform 객체 생성

        playerPos = playerObj.transform.position; // 플레이어 위치 따옴
    }
    private void Update()
    {
        playerPos = playerObj.transform.position; // 플레이어 위치를 매프레임 업데이트
        isGround = Physics2D.Raycast(transform.position, Vector2.down, 3f, LayerMask.GetMask("Ground")); // 땅에 닿기 전에 위로 떠오름 (몬스터가 날라댕김)
    }
    private void FixedUpdate()
    {
        if(isGround)
        {
            rbody2D.AddForce(Vector2.up * JumpPow, ForceMode2D.Impulse); // 몬스터가 땅에 닿기 전에 위로 튀어오름
        }
        transform.position = Vector2.Lerp(transform.position, playerPos, Time.deltaTime); // 부드럽게 플레이어를 따라감 (한 템포 느리게)
        rectTransform.position = new Vector3(transform.position.x, transform.position.y + 1f,0); // 체력바가 몬스터 위를 위치하게
    }
    private void OnCollisionEnter2D(Collision2D collision) // 총알과 부딫혔는지 여부 확인
    {
        if(collision.gameObject.CompareTag("Bullet")) // 태그값 "Bullet"인 오브젝트에게 맞았다면
        {
            StartCoroutine(OnDamage()); // 데미지 입음 (코루틴 시작)
        }
    }
    IEnumerator OnDamage()
    {
        while(HPvalue > 0) // 체력이 0 이상일때만 코루틴 돌고
        {
            HPvalue -= 0.1f;
            HPbar.value = HPvalue;
            yield return new WaitForSeconds(1.5f); // 1.5초마다 데미지를 입음 (순식간에 체력이 없어지는 거 방지)
        }
        if(HPvalue == 0) // 0이면 코루틴 정지
            StopCoroutine(OnDamage());
    }
}
