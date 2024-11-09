using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPar : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Monsters;
    [SerializeField]
    private Slider[] HPbars;
    [SerializeField]
    private Vector2[] MonsterPoss;

    private void Awake()
    {
        Monsters = GameObject.FindGameObjectsWithTag("Monster"); // 몬스터 태그를 가진 몬스터들 모두 찾아서 배열로 저장
        HPbars = new Slider[Monsters.Length]; // 몬스터 수 만큼 체력바를 생성함

        for(int i = 0; i < HPbars.Length; i++)
        {
            MonsterPoss[i] = Monsters[i].transform.position; // 몬스터 각 위치 저장
            HPbars[i] = GetComponent<Slider>(); // 객체생성
        }
    }
    private void Update()
    {
        
    }
}
