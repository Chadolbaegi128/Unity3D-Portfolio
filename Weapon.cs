using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{    
    //무기 타입(열거형), 데미지/공속/범위/효과 변수 생성
    public enum Type
    {
        Melee, Range
    };
    public Type _type;
    public int _damage;
    public float _rate;
    public int _maxAmmo;
    public int _currentAmmo;

    public BoxCollider _meleeArea;
    public TrailRenderer _trailEffect;
    public Transform _bulletPosition;
    public GameObject _bullet;
    public Transform _bulletCasePosition;
    public GameObject _bulletCase;

    public void Use()
    {
        // 현재 무기의 종류가 근접 무기면
        // 휘두르는 모션과 근접무기 효과를 준다.
        if (_type.Equals(Type.Melee))
        {
            // 코루틴 정지 함수: 현재 동작하고 있는 코루틴을 멈추는 메서드
            // 같은 로직이 꼬이지 않게 코루틴을 다시 시작할 때 쓰이는 메서드
            StopCoroutine("Swing");
            // 코루틴 실행 함수
            StartCoroutine("Swing");
        }
        else if (_type.Equals(Type.Range) && _currentAmmo > 0)
        {
            // 현재 장착한 무기 종류가 원거리이고 탄약이 남아있으면 발사 가능
            _currentAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {        
        // 1번 코드 실행
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        _meleeArea.enabled = true;
        _trailEffect.enabled = true;
        // 2번 코드 실행        
        yield return new WaitForSeconds(0.3f);
        _meleeArea.enabled = false;
        // 3번 코드 실행
        yield return new WaitForSeconds(0.3f);
        _trailEffect.enabled = false;
    }

    // 일반함수 처리과정: Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴
    // 코루틴 함수 처리과정:  Use() 메인루틴 + Swing() 코루틴 동시 실행 (Co-op/협동)
    // yield 키워드를 여러 개 사용하여 시간차 로직 작성 가능
    // yield break; 코루틴 탈출
    // yield return null; 1프레임 대기

    IEnumerator Shot()
    {
        // #1. 총알 발사( 총알 오브젝트 자체, 위치, 각도 인스턴스화)
        GameObject instantBullet = Instantiate(_bullet, _bulletPosition.position, _bulletPosition.rotation);

        // 인스턴스화된 총알에 속도 적용
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = _bulletPosition.forward * 50;

        yield return null;

        // #2. 탄피(탄피 경로 설정)
        GameObject instantCase = Instantiate(_bulletCase, _bulletCasePosition.position, _bulletCasePosition.rotation);                
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = _bulletCasePosition.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.down * 10, ForceMode.Impulse);
    }
}
