using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingLayerFix : MonoBehaviour {

    private Renderer r;
    // Use this for initialization
	void Start () {
        r = GetComponent<Renderer>();
        r.sortingLayerName = "Player";
        r.sortingOrder = -1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
