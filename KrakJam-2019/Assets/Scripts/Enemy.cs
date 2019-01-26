using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hitsToKill;
    public float playerForceback;

    int hitPoints;
    Rigidbody2D rgbd;
    

    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        hitPoints = hitsToKill;
    }

    
    void Update()
    {
        if (hitPoints <= 0)
        {
           
            Debug.Log("Enemy killed");
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            float playerX = collision.gameObject.transform.position.x;
            float enemyX = transform.position.x;
            float delta = playerX - enemyX;
            if (collision.collider.ToString()== "Player (UnityEngine.BoxCollider2D)")
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

    public void makeStepTowardsPlayer(GameObject player) 
    {

    }

}

