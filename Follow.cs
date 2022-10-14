using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    Transform _target;

    [SerializeField]
    Vector3 _offset;
    void Update()
    {
        transform.position = _target.position + _offset;
    }
}
