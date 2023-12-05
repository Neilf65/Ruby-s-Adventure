using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    int currentHealth;
    public int health { get { return currentHealth; }}

    public GameObject projectilePrefab;
    private int extraHits= 0;
    private SpriteRenderer spriteRenderer;
    bool isInvincible;
    float invincibleTimer;
    

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    bool canMove;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    AudioSource audioSource;

    public AudioClip CogToss;
    public AudioClip DamageTaken;
    public AudioClip SkeletonDeath;
    
    
    private AudioSource audiosource;
    public AudioClip BackgroundMusic;

    public ParticleSystem HitVFX;

    public MenuController menuController;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(BackgroundMusic);

       


    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);


        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("Skeleton"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    PlaySound(SkeletonDeath);
                    animator.SetTrigger("skdeath");
                }
            }
        }

        if (currentHealth <= 0)
        {
            menuController.LoseGame();
            

            speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            extraHits++;
            speed= 5;
            
            
        }
    }
    public void OnPlayerHit()
    {
        if (extraHits > 0)
        {
            // Decrease the extra hits
            extraHits--;
                
            spriteRenderer.color = Color.white;

                
        }
    }

    // Set physics to update at fixed interval
    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log ("Collided");

        HitVFX.Play();
    }

    // Change Health when player game object collides with enemy game object
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {

            if (isInvincible)
                return;
            Instantiate(HitVFX, transform.position, Quaternion.identity);
            if(extraHits>0)
                return;
            
            

            isInvincible = true;
            invincibleTimer = timeInvincible;

            
            animator.SetTrigger("Hit");


            PlaySound(DamageTaken);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0 , maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    //Call and launch projectile
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(CogToss);
    }

    //Initialize one shot for audio
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
