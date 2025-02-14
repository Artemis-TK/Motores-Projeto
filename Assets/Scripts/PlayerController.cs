using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Transform _t;
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private bool isJumping = false;

    [SerializeField]
    private SpriteRenderer sp;

    [SerializeField]
    private float Speed = 4f;

    [SerializeField]
    private float Jump = 10f;

    private const float startPositionX = -3.522f;
    private const float startPositionY = -0.5400248f;

    [SerializeField]
    private bool isDead;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        isDead = false;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-Speed * 10, transform.localPosition.y));
            sp.flipX = false;
            if (Input.GetKeyDown(KeyCode.W) && !isJumping)
            {
                rb.AddForce(new Vector2(transform.localPosition.x, Jump * 130), ForceMode2D.Force);
                isJumping = true;
                rb.AddForce(new Vector2(transform.position.x, Jump));
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(Speed * 10, transform.localPosition.y));
            sp.flipX = true;
            if (Input.GetKeyDown(KeyCode.W) && !isJumping)
            {
                rb.AddForce(new Vector2(transform.localPosition.x, Jump * 130), ForceMode2D.Force);
                isJumping = true;
                rb.AddForce(new Vector2(transform.position.x, Jump));
            }
        }
        else if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            rb.AddForce(new Vector2(transform.position.x, Jump * 130), ForceMode2D.Force);
            isJumping = true;
        }

        if (VerifyPosition(transform, -4.0f))
        {
            ResetPlayerPosition();
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        Debug.Log("Colidiu com: " + c.gameObject.name);

        if (c.gameObject.CompareTag("ground"))
        {
            isJumping = false;
        }

        if (c.gameObject.CompareTag("agua") || VerifyPosition(transform, -4.0f))
        {
            Debug.Log("Colidiu com: " + c.gameObject.name);
            ResetPlayerPosition();
        }

    }
    private void ResetPlayerPosition()
    {
        if (!this.isDead)
        {
            this.isDead = true;
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(startPositionX, startPositionY, 0);
            this.isDead = false;
        }
        else return;
    }

    private bool VerifyPosition(Transform position, float arg)
    {
        return transform.position.y <= arg;
    }
}
