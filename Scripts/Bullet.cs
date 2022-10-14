using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int _damage;

    void OnCollisionEnter(Collision collision)
    {
        // ź�ǰ� �ٴڿ� �������� 3�ʰ� ������ ���ִ� ����
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject, 3);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            //ź�ǰ� ���� �ε����� �ٷ� ������� ��
            Destroy(gameObject);
        }
        
    }

}
