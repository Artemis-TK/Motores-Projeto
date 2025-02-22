using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private SpriteRenderer sp;

    [SerializeField]
    private float speed = 4f;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private const float startPositionX = -3.538148f;
    private const float startPositionY = -0.5400812f;

    private bool isDead;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isOnGround = true;

    private GameObject coin;

    [SerializeField]

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        coin = GameObject.Find("Coin");
    }

    private void Start()
    {
        isDead = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            HandleCoinInteraction();
            HandleFall();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (isOnGround)
        {
            isJumping = false;
        }else
        {
            isJumping = true;
        }

        if (horizontal != 0)
        {
            rb.velocity = new Vector2(horizontal * (speed * 3 / 2.5f), rb.velocity.y);
            sp.flipX = horizontal > 0;
        }

        // if (horizontal == 0 && isOnGround)
        // {
        //     rb.velocity = new Vector2(0,0);
        // }

        if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && isOnGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isOnGround = false;
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleCoinInteraction()
    {
        if (coin != null && Vector3.Distance(transform.position, coin.transform.position) < 0.80f)
        {
            Destroy(coin.gameObject);
            StartCoroutine(LoadMainSceneAfterDelay());
        }
    }

    private IEnumerator LoadMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(0.57f);
        SceneManager.LoadScene("main");
    }

    private void HandleFall()
    {
        if (transform.position.y <= -4.0f) 
        {
            rb.velocity = new Vector2(transform.position.x, -1);
            StartCoroutine(ResetPlayerPosition());
        }
    }

    private IEnumerator ResetPlayerPosition()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(startPositionX, startPositionY, 0);
        isDead = false;
        rb.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("ground"))
        {
            isJumping = false;
            isOnGround = true;
        }

        if (c.gameObject.CompareTag("water"))
        {
            isJumping = true;
            StartCoroutine(ResetPlayerPosition());
            isJumping = false;
        }
    }
}
