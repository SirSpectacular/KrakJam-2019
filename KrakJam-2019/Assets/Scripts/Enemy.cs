using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hitsToKill;

    int hitPoints;
    

    void Start()
    {
        hitPoints = hitsToKill;
    }

    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 forceVector = (collision.gameObject.transform.position - transform.position)*(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(forceVector);
        }
    }
}
