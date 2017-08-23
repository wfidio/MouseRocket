using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public float jetpackForce = 75.0f;
    public Rigidbody2D rb;

    [SerializeField]
    private float forwardMovementSpeed = 3.0f;

    public Transform groundCheckTransform;

    private bool grounded;

    public LayerMask groundCheckLayerMask;

    Animator animator;

    public ParticleSystem jetpack;

    private bool dead = false;

    private uint coins = 0;

    public Texture2D coinIconTexture;

    public AudioClip coinCollectSound;

    public AudioSource jetpackSource;

    public AudioSource footstepSource;

    public ParallaxScript parallax;



	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        
        
        if (jetpackActive)
        {
            rb.AddForce(new Vector2(0.0f, jetpackForce));
        }

        if (!dead)
        {
            Vector2 newVelocity = rb.velocity;
            newVelocity.x = forwardMovementSpeed;
            rb.velocity = newVelocity;
        }

        UpdateGroundedStatus();
        Adjustjetpack(jetpackActive);
        AdjustFootStepsAndSound(jetpackActive);
    }

    void UpdateGroundedStatus()
    {
        grounded = Physics2D.OverlapCircle(groundCheckTransform.transform.position, 0.1f, groundCheckLayerMask);

        animator.SetBool("grounded", grounded);
    }

    void Adjustjetpack(bool jetpackActive)
    {
        jetpack.enableEmission = !grounded;
        jetpack.emissionRate = jetpackActive ? 300.0f : 75.0f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }else
        {
           HitByLaser(collider);
        }
    }

    void HitByLaser(Collider2D laserCollider)
    {
        AudioSource audio = laserCollider.gameObject.GetComponent<AudioSource>();
        if (!dead)
        {
            audio.Play();
        }
        this.dead = true;
        animator.SetBool("dead", true);
    }

    void CollectCoin(Collider2D collider)
    {
        coins++;
        Destroy(collider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
    }

    void DisplayCoinCount()
    {
        Rect coinIconRect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(coinIconRect, coinIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
        GUI.Label(labelRect, coins.ToString(), style);
    }

    void OnGUI()
    {
        DisplayCoinCount();
        DisplayRestartButton();
    }

    void DisplayRestartButton()
    {
        if (dead && grounded)
        {
            Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
            if(GUI.Button(buttonRect,"Tap to restart"))
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }

    void AdjustFootStepsAndSound(bool jetpackActive)
    {
        footstepSource.enabled = grounded && !dead;

        jetpackSource.enabled = !grounded && !dead;
        jetpackSource.volume = jetpackActive ? 1.0f : 0.5f;
    }
}
