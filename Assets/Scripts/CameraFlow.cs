﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour {

    [SerializeField]
    private GameObject targetObject;

    private float distanceToTarget;    
	// Use this for initialization
	void Start () {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }
	
	// Update is called once per frame
	void Update () {
        float targetObjectX = targetObject.transform.position.x;

        Vector3 newCameraPosition = transform.position;
        newCameraPosition.x = targetObjectX+distanceToTarget;
        transform.position = newCameraPosition;
    }
}
