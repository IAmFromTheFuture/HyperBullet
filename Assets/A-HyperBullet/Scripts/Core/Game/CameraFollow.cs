using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    private Transform _target;

    private void Update()
    {
        if (_target is not null)
        {
            transform.localPosition = _target.localPosition + _offset;
            transform.LookAt(_target);
            //transform.localEulerAngles = _target.localEulerAngles;
        }
    }

    public void UpdateTarget(Transform newTarget, Vector3 offset = default)
    {
        _target = newTarget;
        _offset = offset;
    }

}
