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
            // 아이템과 충돌하는 오브젝트의 태그가 'Floor'라면
            // 'Floor'를 가진 오브젝에 물리효과가 적용되지 않게 변경(isKinematic)
            _rigid.isKinematic = true;
            _sphereCollider.enabled = false;
        }
    }
}
