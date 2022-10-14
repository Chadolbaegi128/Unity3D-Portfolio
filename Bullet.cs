using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int _damage;

    void OnCollisionEnter(Collision collision)
    {
        // 탄피가 바닥에 떨어지고 3초가 지나면 없애는 로직
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject, 3);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            //탄피가 벽에 부딪히면 바로 사라지게 함
            Destroy(gameObject);
        }
        
    }

}
