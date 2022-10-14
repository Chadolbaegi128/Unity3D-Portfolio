using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    //공전 목표, 공전 속도, 목표와의 거리를 저장할 변수 선언
    public Transform _target;
    public float _orbitSpeed;
    Vector3 _offset;

    void Start()
    {
        //수류탄과 플레이어간의 거리 차이를 이용해
        //수류탄 궤도가 플레이어를 따라가도록 한다.
        _offset = transform.position - _target.position;    
    }

    void Update()
    {
        //플레이어가 이동하면서 좌표가 자동으로 갱신됨
        transform.position = _target.position + _offset;

        //수류탄 궤도가 플레이어 캐릭터를 따라가도록 설정함
        transform.RotateAround(_target.position, Vector3.up, _orbitSpeed * Time.deltaTime);

        //RotateAround() 후의 위치를 가지고 목표와의 거리를 유지
        //이 _offset 변수의 값이 다시 플레이어가 움직일 때 반영이 됨
        _offset = transform.position - _target.position;
    }
}
