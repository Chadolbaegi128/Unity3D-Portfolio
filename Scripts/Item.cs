using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{    
   public  enum Type
    {
        Ammo, Coin, Grenade, Heart, Weapon, Missile
    };
        
    public Type _type;        
    public int _value;

    Rigidbody _rigid;
    SphereCollider _sphereCollider;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            // �����۰� �浹�ϴ� ������Ʈ�� �±װ� 'Floor'���
            // 'Floor'�� ���� �������� ����ȿ���� ������� �ʰ� ����(isKinematic)
            _rigid.isKinematic = true;
            _sphereCollider.enabled = false;
        }
    }
}
