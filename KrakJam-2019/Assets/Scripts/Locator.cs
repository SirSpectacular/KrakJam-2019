using UnityEngine;

public class Locator :MonoBehaviour {
    public float jump;
    private float maxStroll;
    public float speed;
    public int maxHitPoints;
    public Enemy madman;
    private bool onFloor;
    private Vector2 spawnPosition;

    private int hitPoints;
    private float dmgTimer;

    private void Awake() {
        spawnPosition = GetComponentInParent<Room>().transform.position;
    }

    void Start() {
        dmgTimer = 0;
        hitPoints = maxHitPoints;
        maxStroll = GetComponentInParent<BoxCollider2D>().size.x / 2;
        onFloor = false;
    }

    // Update is called once per frame
    void Update() {
        if(dmgTimer > 0) {
            dmgTimer -= Time.deltaTime;
            if(onFloor) {
                Vector2 currentPosition = transform.position;
                Vector2 deltaPosition = currentPosition - spawnPosition;
                if(deltaPosition.x > maxStroll) {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed,1.0f * jump));
                }
                else if(deltaPosition.x < (-1f) * maxStroll) {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(speed,1.0f * jump));
                }
                else {
                    if((Random.Range(0,2) == 0)) {
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed,1.0f * jump));
                    }
                    else {
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(speed,1.0f * jump));
                    }
                }
                onFloor = false;
            }
        }
        else {
            Vector2 currentPosition = transform.position;
            Vector2 deltaPosition = currentPosition - spawnPosition;
            if(deltaPosition.x > maxStroll) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed/5,0.0f));
            }
            else if(deltaPosition.x < (-1f) * maxStroll) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(speed/5,0.0f ));
            }
            else {
                if((Random.Range(0,2) == 0)) {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed/5,0.0f));
                }
                else {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(speed/5,0.0f));
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Floor") {
            Vector3 location = col.transform.position - transform.position;
            if(location.y < 0) {
                onFloor = true;
            }
        }
    }
    bool TMP = false;

    public void ReceiveDamage(int damage) {

        dmgTimer = 1.0f;
        Debug.Log("Dmg" + damage);
        hitPoints -= damage;
        GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red,Color.green,(float) hitPoints/maxHitPoints);
        if(hitPoints < 0 && !TMP) {
            TMP = true;
            GetComponentInParent<Room>().killMePlis(this, madman);
        }
    }



}
