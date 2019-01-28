using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler :MonoBehaviour {
    public float speed;
    public float jump;
    bool onFloor;
    Rigidbody2D rgbd;
    BoxCollider2D swing;
    public Animator animator;

    bool onLadder;

    public float attackCooldown;
    public float attackDuration;
    float timer;
    float lastHit;
    bool canAttack;

    public Image Stamina;
    public Image Health;

    public float maxHitPoints;
    float hitPoints;

    private string item = null;

    public bool isDead { get; set; }


    enum sideOfSwing {
        Left, Right
    }
    sideOfSwing side;

    public GameObject SwingObject;
    Vector2 lastPosition;

    public void receiveDamage(float damage) {
        hitPoints -= damage;
        Debug.Log("Player hit for " + damage);
        Health.fillAmount = (float)hitPoints / (float)maxHitPoints;


        if(hitPoints <= 0) {
            Debug.Log("PLayer should be dead");
            rgbd.constraints = RigidbodyConstraints2D.None; 


            float rotator = 100.0f;
            rgbd.AddTorque(rotator);
            isDead = true;
          

        }
    }

    public string getItem() {
        return item;
    }

    ParticleSystem particles;

    void Start() { 
        particles = GetComponentInChildren<ParticleSystem>();
        disableParticle();
        onLadder = false;
        hitPoints = maxHitPoints;
        canAttack = true;
        timer = 0.0f;
        side = sideOfSwing.Right;
        rgbd = GetComponent<Rigidbody2D>();
        swing = GetComponentInChildren<BoxCollider2D>();
        onFloor = false;
        lastPosition = transform.position;

        SwingObject.GetComponent<BoxCollider2D>().enabled = false;
        isDead = false;
    }

    public void startParticle() {
        particles.enableEmission = true;
    }

    public void disableParticle(){
        Debug.Log("Stop");
        particles.enableEmission = false;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;


    }

    void Update() {

        if(!isDead) { 
        animator.SetFloat("HSpeed",Mathf.Abs(Input.GetAxis("Horizontal")));
        animator.SetFloat("VSpeed",Mathf.Abs(Input.GetAxis("Vertical")));
    }

        getInputs();

        Stamina.fillAmount =(float)lastHit / (float)attackCooldown  ;
        if (timer - lastHit > attackCooldown)
        {
            canAttack = true;
            animator.ResetTrigger("Swing");
            lastHit = 0.0f;
        }
        if (timer - lastHit > attackDuration)
        {
            SwingObject.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    void getInputs()
    {
        if(isDead) { }
        else { 
            if(Input.GetKeyDown("space") && canAttack) {
                animator.SetTrigger("Swing");
                SwingObject.GetComponent<BoxCollider2D>().enabled = true;
                canAttack = false;
                lastHit = timer;
            }

            if(Input.GetKeyUp(KeyCode.Z)) {
                disableParticle();
            }

            if(Input.GetAxis("Horizontal") > 0) {
                if(transform.localScale.x < 0) transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
                rgbd.AddForce(new Vector2(1f,0f) * speed);


            }
            else if(Input.GetAxis("Horizontal") < 0) {
                if(transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
                rgbd.AddForce(new Vector2(-1f,0f) * speed);
 

            }

            if((Input.GetAxis("Vertical") > 0) && onLadder == true) {
                onFloor = false;
                rgbd.velocity = new Vector2(0f,1f) * jump;
            }

            else if((Input.GetAxis("Vertical") < 0)) {
                rgbd.AddForce(new Vector2(0f,-1f) * jump);
            }
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
   }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            Debug.Log("On ladder");
            onLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            Debug.Log("Off ladder");
            onLadder = false;
        }
    }

    public void setItem(string content) {
        item = content;
    }

}