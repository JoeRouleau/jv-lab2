using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    float horizontal;
    float vertical;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float limitCoyoteTime = 0.2f;
    [SerializeField] private float nextLevelDelay = 2.0f;

    private bool isGrounded = true;
    private bool isStuckOnCollision = false;
    private bool isOnEnemy = false;

    private float screenLeft = -3.86f;
    private float screenRight = 3.86f;
    private float resetDelay = 1.0f;
    private float currentCoyoteTime;

    private bool isDead = false;
    private bool hasWon = false;
    private bool isOnExit = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStuckOnCollision)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            isStuckOnCollision = false;
        }

        if(hasWon)
        {
            GameManager.instance.StartNextlevel(nextLevelDelay);
            hasWon = false;
        }
    }

    private void OnDisable()
    {
        if(isDead)
        {
            GameManager.instance.PlayerDie();
            GameManager.instance.RestartLevel(resetDelay);
        }
    }

    private void OnEnable()
    {
        isDead = false;
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        if(!isOnEnemy && !isOnExit)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float moveBy = x * speed;
            rb.velocity = new Vector2(moveBy, rb.velocity.y);
         }

        if(transform.position.x <= screenLeft)
        {
            gameObject.transform.position = new Vector2(screenLeft, gameObject.transform.position.y);
        }

        if(transform.position.x >= screenRight)
        {
            gameObject.transform.position = new Vector2(screenRight, gameObject.transform.position.y);
        }
    }

    void Jump()
    {
        if(!isOnEnemy && !isOnExit)
        {
            if(Input.GetButtonDown("Fire1") && (isGrounded || currentCoyoteTime == limitCoyoteTime))
            {
                rb.velocity = new Vector2(rb.velocity.x, 7);
                isGrounded = false;
                currentCoyoteTime = 0f;
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "HardBlock" || other.gameObject.tag == "Platform")
        {
            isGrounded = true;
            StartCoroutine(IncrementCoyoteTime());
        }
        if(other.gameObject.tag == "Exit")
        {
            hasWon = true;
            isOnExit = true;
            // Jouer musique
        }
        if(other.gameObject.tag == "Monster")
        {
            isDead = true;
            isOnEnemy = true;
            StartCoroutine(ScaleDownAnimation(resetDelay));
            // Son de mort
        }
        if(isDead)
        {
            
        }
    }

    IEnumerator IncrementCoyoteTime()
    {
        currentCoyoteTime = Time.deltaTime;
        if(currentCoyoteTime >= limitCoyoteTime)
        {
            currentCoyoteTime = limitCoyoteTime;
        }
        yield return 0;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "HardBlock" || collision.gameObject.tag == "Platform")
        {
            isStuckOnCollision = true;
        }
    }


    // Code de https://stackoverflow.com/questions/53513786/how-can-i-scale-slowly-the-object-to-0-0-0/53514215
    IEnumerator ScaleDownAnimation(float time)
    {
        float i = 0;
        float rate = 1 / time;

        Vector3 fromScale = transform.localScale;
        Vector3 toScale = Vector3.zero;
        while (i < 1)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(fromScale, toScale, i);
            yield return 0;
        }
        gameObject.SetActive(false);
    }
}
