using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int hitsToKill;
    public float playerForceback;
    public float stepVelocity;
    int hitPoints;
    Rigidbody2D rgbd;
    float offset;
    public GameObject player;
    float timer;
    public float intervalBetweenActions;
    public Image healthBar;
    public Canvas healthBG;
    float jumpTimer;

    bool isDead;

    float chaoticJumpInterval;
    public float jump;

    float dieTime;

    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        hitPoints = hitsToKill;
        Debug.Log(hitPoints);
        timer = 0;
        offset = 0.5f;
        healthBar.enabled = false;
        healthBG.enabled = false;

        isDead = false;

        chaoticJumpInterval = 3.0f;
        jumpTimer = 0.0f;
    }
    
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        jumpTimer += Time.fixedDeltaTime;
    }


    void Update() {
        if(!isDead) {
            if(hitPoints <= 0) {
                Debug.Log("Dead");
                float rotator = 100.0f;
                rgbd.AddTorque(rotator);
                isDead = true;
                dieTime = 0f;
            }
            if(timer > intervalBetweenActions) {
                MakeAction(player);
                timer = 0.0f;
            }
        }
        else {
            dieTime += Time.deltaTime;
            if(dieTime > 1.5f) {
                Destroy(this.gameObject);
            }
        }
    }

    private void jumpChaoticly()
    {
        if (jumpTimer > chaoticJumpInterval)
        {
            rgbd.AddForce( new Vector2(0f, 1f) * jump);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float playerX = collision.gameObject.transform.position.x;
            float enemyX = transform.position.x;
            float delta = playerX - enemyX;
            if (collision.collider.ToString() == "Player (UnityEngine.BoxCollider2D)") //Just keep scrolin down
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(delta * playerForceback, 5.0f));
                rgbd.AddForce(new Vector2(delta * playerForceback , 0f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            float playerX = collision.gameObject.transform.position.x;
            float enemyX = transform.position.x;
            float delta = playerX - enemyX;
            Debug.Log("Player and enemy hit" + delta);
            rgbd.AddForce(new Vector2(-5.0f * delta * playerForceback, 0f));
            hitPoints--;
            healthBar.enabled = true;
            healthBar.fillAmount = (float)hitPoints /(float) hitsToKill;
            healthBG.enabled = true;
            GetComponent<AudioSource>().Play();
        }
    }

    public void MakeAction(GameObject player)
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 myPosition = transform.position;
        float deltaY = myPosition.y - playerPosition.y;
        float deltaX = myPosition.x - playerPosition.x;


        if (deltaY < offset && deltaY > -offset)
        {
            
            MakeStep(deltaX);
        }
    }

    void MakeStep(float deltaX)
    {
        if (deltaX > 0)
        {
            rgbd.AddForce(new Vector2(-1f * stepVelocity, 0f));

        }
        else if (deltaX < 0)
        {
            rgbd.AddForce(new Vector2(1f * stepVelocity, 0f));
        }
    }





    public void ReceiveDamage(float damage){

    }

}

