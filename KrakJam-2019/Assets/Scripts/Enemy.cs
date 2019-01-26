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

    bool canRotate;

    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        hitPoints = hitsToKill;
        timer = 0;
        offset = 0.5f;
        healthBar.enabled = false;
        healthBG.enabled = false;
        canRotate = false;

    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }


    void Update()
    {
        //Rotate(10.0f);
        Rotate(1.0f);
        if (canRotate)
        {
            Rotate(1.0f);
        }
        else
        {

            if (hitPoints <= 0)
            {
                Debug.Log("Enemy killed");
                canRotate = true;
                rgbd.isKinematic = true;
                GetComponent<BoxCollider2D>().enabled = false;
               // Destroy(this.gameObject);
            }
            if (timer > intervalBetweenActions)
            {
                MakeAction(player);
                timer = 0.0f;
            }
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
            healthBar.enabled = true;
            healthBar.fillAmount = (float)hitPoints /(float) hitsToKill;
            healthBG.enabled = true;
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

    void Die()
    {

    }

    IEnumerator TurnOver()
    {
        while (true)
        {
            transform.Rotate(Vector3.back,90f);
            yield return new WaitForSeconds(0.01f);
        }
    }


    IEnumerator Rotate(float duration)
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }
    }



    public void ReceiveDamage(float damage){

    }

}

