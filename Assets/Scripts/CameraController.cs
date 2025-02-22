using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _t;

    public float suavidade = 2f * (1/1000);

    void Start()
    {
       if(_t == null) _t = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,
                                          new Vector3(_t.position.x + (suavidade * Time.deltaTime),
                                                      _t.position.y+1,
                                                      transform.position.z),
                                          Time.deltaTime * suavidade);
    }
}
