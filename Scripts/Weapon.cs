using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{    
    //���� Ÿ��(������), ������/����/����/ȿ�� ���� ����
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
        // ���� ������ ������ ���� �����
        // �ֵθ��� ��ǰ� �������� ȿ���� �ش�.
        if (_type.Equals(Type.Melee))
        {
            // �ڷ�ƾ ���� �Լ�: ���� �����ϰ� �ִ� �ڷ�ƾ�� ���ߴ� �޼���
            // ���� ������ ������ �ʰ� �ڷ�ƾ�� �ٽ� ������ �� ���̴� �޼���
            StopCoroutine("Swing");
            // �ڷ�ƾ ���� �Լ�
            StartCoroutine("Swing");
        }
        else if (_type.Equals(Type.Range) && _currentAmmo > 0)
        {
            // ���� ������ ���� ������ ���Ÿ��̰� ź���� ���������� �߻� ����
            _currentAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {        
        // 1�� �ڵ� ����
        yield return new WaitForSeconds(0.1f); // 0.1�� ���
        _meleeArea.enabled = true;
        _trailEffect.enabled = true;
        // 2�� �ڵ� ����        
        yield return new WaitForSeconds(0.3f);
        _meleeArea.enabled = false;
        // 3�� �ڵ� ����
        yield return new WaitForSeconds(0.3f);
        _trailEffect.enabled = false;
    }

    // �Ϲ��Լ� ó������: Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ
    // �ڷ�ƾ �Լ� ó������:  Use() ���η�ƾ + Swing() �ڷ�ƾ ���� ���� (Co-op/����)
    // yield Ű���带 ���� �� ����Ͽ� �ð��� ���� �ۼ� ����
    // yield break; �ڷ�ƾ Ż��
    // yield return null; 1������ ���

    IEnumerator Shot()
    {
        // #1. �Ѿ� �߻�( �Ѿ� ������Ʈ ��ü, ��ġ, ���� �ν��Ͻ�ȭ)
        GameObject instantBullet = Instantiate(_bullet, _bulletPosition.position, _bulletPosition.rotation);

        // �ν��Ͻ�ȭ�� �Ѿ˿� �ӵ� ����
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = _bulletPosition.forward * 50;

        yield return null;

        // #2. ź��(ź�� ��� ����)
        GameObject instantCase = Instantiate(_bulletCase, _bulletCasePosition.position, _bulletCasePosition.rotation);                
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = _bulletCasePosition.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.down * 10, ForceMode.Impulse);
    }
}
