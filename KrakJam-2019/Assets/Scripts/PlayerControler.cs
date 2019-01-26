using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float speed;
    public float jump;
    bool onFloor;
    Rigidbody2D rgbd;
    BoxCollider2D swing;

    public float attackCooldown;
    public float attackDuration;
    float timer;
    float lastHit;
    bool canAttack;

     enum sideOfSwing
    {
        Left,Right
    }
     sideOfSwing side;

    public GameObject SwingObject;
    Vector2 lastPosition;
    
    void Start()
    {
        canAttack = true;
        timer = 0.0f;
        side = sideOfSwing.Right;
        rgbd = GetComponent<Rigidbody2D>();
        swing = GetComponentInChildren<BoxCollider2D>();
        onFloor = false;
        lastPosition = transform.position;

        SwingObject.GetComponent<BoxCollider2D>().enabled = false;

    }
    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer - lastHit > attackCooldown)
        {
            canAttack = true;
            lastHit = 0.0f;
        }
        if (timer - lastHit> attackDuration){
            SwingObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void Update()
    {
        getInputs();
        chooseSideForSwing();
    }

    void getInputs()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            rgbd.AddForce(new Vector2(1f, 0f) * speed);

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rgbd.AddForce(new Vector2(-1f, 0f) * speed);
        }

        if ((Input.GetAxis("Vertical") > 0) && onFloor == true)
        {
            onFloor = false;
            rgbd.velocity = new Vector2(0f, 1f) * jump;
        }

        else if ((Input.GetAxis("Vertical") < 0))
        {
            rgbd.AddForce(new Vector2(0f, -1f) * jump);
        }

        if (Input.GetKeyDown("space")&&canAttack)
        {
            
            SwingObject.GetComponent<BoxCollider2D>().enabled = true;
            canAttack = false;
            lastHit = timer;
        }
    }
    void chooseSideForSwing()
    {
        Vector2 currentPosition = transform.position;
        float deltaX = currentPosition.x - lastPosition.x;
        lastPosition = currentPosition;
        if (deltaX > 0 && side==sideOfSwing.Left)
        {
            Vector3 revert = new Vector3(6.0f, 0.0f);
            transform.localScale += revert;

            side = sideOfSwing.Right;
        }
        else if (deltaX < 0 && side == sideOfSwing.Right)
        {
            Vector3 revert = new Vector3(6.0f, 0.0f);
            transform.localScale -=revert;
            side = sideOfSwing.Left;
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            Vector3 location = col.transform.position-transform.position;
            if (location.y < 0)
            {
                onFloor = true;
            }
        }
        else if (col.gameObject.tag == "Stairs")
        {

        }
   }

}
