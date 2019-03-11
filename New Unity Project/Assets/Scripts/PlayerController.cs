using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    bool grounded;
    bool facingRight;
    bool canInput;
    bool win;
    Animator anim;
    float speed;
    float nextStage;
    AudioSource playerSound;
    public AudioClip clip2;

    
    public float moveSpeed;
    public float jump;
    public float delay;
    public int lives;
    public int score;
    public Text scoreText;
    public Text winText;
    public Text lifeText;
    public Transform lvl2;
    public int goal;
    public int goal2;



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingRight = true;
        lives = 3;        
        winText.text = "Collect The Burritos!";
        canInput = true;
        playerSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetBool("OnGround", true);

        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        if (canInput && grounded)
        {
            rb2d.AddForce(movement * moveSpeed);
        }

        if (moveHorizontal != 0 && canInput)
        {
            winText.text = "";
        }

        speed = rb2d.velocity.magnitude;


        if (!grounded)
        { 
            anim.SetBool("OnGround", false);
        }
        if (speed > 0)
        {
            anim.SetInteger("State", 1);
            anim.speed = speed/3;
        }    
        if (speed == 0)
        {
            anim.SetInteger("State", 0);
            anim.speed = 1;
        }

        Flip(moveHorizontal);

    }
    private void Update()
    {
        if (lives < 1)
        {
            this.gameObject.SetActive(false);
            winText.text = ";_; You Lose ;_;";
        }
        AddScore();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            grounded = true;
            if (Input.GetKeyDown(KeyCode.UpArrow) && canInput)
            {
                rb2d.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);                
            }            

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            grounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            grounded = true;
            anim.SetTrigger("Land");
        }
        if(collision.collider.tag == "Bad")
        {
            lives --;
            collision.gameObject.SetActive(false);
            AddScore();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Good"))
        {
            score ++;
            collision.gameObject.SetActive(false);
            AddScore();
        }
    }

    private void Flip(float moveHorizontal)
    {
        if (moveHorizontal > 0 && !facingRight || moveHorizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }
    private void AddScore()
    {
        scoreText.text = "x " + score.ToString();
        lifeText.text = "x " + lives.ToString();

        if (score >= goal && !win)
        {
            winText.text = "Stage Cleared!";
            canInput = false;
            Win();
            playerSound.clip = clip2;
            playerSound.Play();

        }
        else if (score >= goal && win)
        {
            winText.text = "You Win!";
            canInput = false;            
        }
        
    }

    private void Win()
    {
        Debug.Log("win");
        nextStage += Time.deltaTime;
        Debug.Log(nextStage);
        if (nextStage >= delay)
        {
            Debug.Log("Next");
            this.transform.position = lvl2.position;
            goal = goal + goal2;
            win = true;
            winText.text = "";
            canInput = true;
            lives = 3;
        }
    }
}
