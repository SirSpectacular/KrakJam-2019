using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float speed;
    public float jump;
    bool onFloor;
    Rigidbody2D rgbd;

    public GameObject stairs;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        onFloor = false;
    }

    void Update()
    {
        getInputs();
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
            rgbd.velocity= new Vector2(0f, 1f) * jump;
        }


        else if ((Input.GetAxis("Vertical") < 0))
        {
            stairs.GetComponent<BoxCollider2D>().enabled=false;
            rgbd.AddForce(new Vector2(0f, -1f) * jump);
        }

    }

    void canJump()
    {

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
