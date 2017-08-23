using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    [SerializeField]
    private Sprite laserOnSprite;

    [SerializeField]
    private Sprite laserOffSprite;

    public float interval = 0.5f;
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

    private Collider2D collider;
    private SpriteRenderer render;
	// Use this for initialization
	void Start () {
        timeUntilNextToggle = interval;
        collider = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;
        if (timeUntilNextToggle <= 0)
        {
            isLaserOn = !isLaserOn;
            collider.enabled = isLaserOn;

            if (isLaserOn)
            {
                render.sprite = laserOnSprite;
            }
            else
            {
                render.sprite = laserOffSprite;
            }

            timeUntilNextToggle = this.interval;
        }

        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
    }
}
