using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        hitPoints = hitsToKill;
        timer = 0;
        offset = 0.5f;
    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }


    void Update()
    {
        if (hitPoints <= 0)
        {
            Debug.Log("Enemy killed");
            Destroy(this.gameObject);
        }
        if (timer > intervalBetweenActions)
        {
            MakeAction(player);
            timer = 0.0f;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float playerX = collision.gameObject.transform.position.x;
            float enemyX = transform.position.x;
            float delta = playerX - enemyX;
            if (collision.collider.ToString() == "Player (UnityEngine.BoxCollider2D)")
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(delta * playerForceback, 5.0f));
                rgbd.AddForce(new Vector2(delta * playerForceback / 4.0f, 0f));
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
            rgbd.velocity = new Vector2(-1f * stepVelocity, 0f);

        }
        else if (deltaX < 0)
        {
            rgbd.velocity = new Vector2(1f * stepVelocity, 0f);
        }
    }

}

