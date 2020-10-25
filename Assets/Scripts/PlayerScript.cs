using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text scoreText;
    public Text winText;
    public Text livesText;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool isOnGround = false;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public AudioSource musicSource;
    public AudioClip musicGame;
    public AudioClip musicWin;
    Animator anim;
    private bool facingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        scoreText.text = "Score: " + scoreValue.ToString();
        winText.text = "";
        livesText.text = "Lives: " + livesValue.ToString();
        musicSource.clip = musicGame;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey("left") || Input.GetKey("right"))
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
        }
        if(isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if(facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if(facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            scoreText.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(scoreValue == 4)
            {
                transform.position = new Vector3(43.0f,6.5f,0.0f);
                livesValue = 3;
                SetLivesText();
            }
            if(scoreValue == 8)
            {
                winText.text = "You Win!\nGame created by: Eric Osorio";
                musicSource.clip = musicWin;
                musicSource.Play();
            }
        }
        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);
            if(livesValue == 0)
            {
                winText.text = "You Lose...";
                Destroy(this);
            }
        }
        if(collision.collider.tag == "BadFloor")
        {
            transform.position = new Vector3(43.0f,6.5f,0.0f);
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if((Input.GetKey(KeyCode.W)) || (Input.GetKey("up")))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    private void SetLivesText()
    {
        livesText.text = "Lives: " + livesValue.ToString();
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
