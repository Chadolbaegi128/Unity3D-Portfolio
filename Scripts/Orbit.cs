using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    //���� ��ǥ, ���� �ӵ�, ��ǥ���� �Ÿ��� ������ ���� ����
    public Transform _target;
    public float _orbitSpeed;
    Vector3 _offset;

    void Start()
    {
        //����ź�� �÷��̾�� �Ÿ� ���̸� �̿���
        //����ź �˵��� �÷��̾ ���󰡵��� �Ѵ�.
        _offset = transform.position - _target.position;    
    }

    void Update()
    {
        //�÷��̾ �̵��ϸ鼭 ��ǥ�� �ڵ����� ���ŵ�
        transform.position = _target.position + _offset;

        //����ź �˵��� �÷��̾� ĳ���͸� ���󰡵��� ������
        transform.RotateAround(_target.position, Vector3.up, _orbitSpeed * Time.deltaTime);

        //RotateAround() ���� ��ġ�� ������ ��ǥ���� �Ÿ��� ����
        //�� _offset ������ ���� �ٽ� �÷��̾ ������ �� �ݿ��� ��
        _offset = transform.position - _target.position;
    }
}
