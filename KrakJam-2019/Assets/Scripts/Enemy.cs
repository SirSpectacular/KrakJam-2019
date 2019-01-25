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
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           // Debug.Log("Player and enemy hit");
            float playerX = collision.gameObject.transform.position.x;
            float enemyX = transform.position.x;
            float delta = playerX - enemyX;
            Debug.Log("Player and enemy hit"+delta);
            //     Vector3 forceVector = (collision.gameObject.transform.position - transform.position)*(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(delta*playerForceback,5.0f));
            rgbd.AddForce(new Vector2(delta * playerForceback/4.0f, 0f));
        }
    }
}
