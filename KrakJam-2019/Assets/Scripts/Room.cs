using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room :MonoBehaviour {

    public int id { set; get; }
    public bool isOnFire = false;
    public bool isFlooded = false;
    public bool isPlayerInside;

    public float fireSpreedRate;
    private  float currentTime = 0;
    private int step = 1;
    public int damage;
    public float dmgInterval;

    public Room[] adjacentRooms = new Room[2];

    private SpriteRenderer renderer;

    public List<Enemy> enemies = new List<Enemy>();
    private List<Locator> locators = new List<Locator>();
    private PlayerControler player;


    private void Awake() {
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFlooded) {

        }
        if(isOnFire) {
            currentTime += Time.deltaTime;
            if(currentTime >= fireSpreedRate) {
                int index = Random.Range(0,1);
                if(adjacentRooms[index] != null) adjacentRooms[index].makeFire();
                else if(adjacentRooms[(index + 1) % 2] != null) adjacentRooms[(index + 1) % 2].makeFire();
                currentTime  = 0;
                step = 1;
            }
            if(currentTime >= dmgInterval * step) {
                dealDmg();
                step++;
            }
        }
    }

    public void dealDmg() {
       // foreach(Locator locator in locators)
       //    locator.RecieveDamage(damage);
       // if(isPlayerInside) player.RecieveDamage(damage);
    }

    public void spawnLocator(Locator prefab) {
        locators.Add(Instantiate(prefab, transform));
    }


    public void makeFire() {
        if(isFlooded || isOnFire) return;
        isOnFire = true;
        renderFire();
    }

     public Fire firePrefab;

    private void renderFire() {
        int amountOfFlames = Random.Range(1,10);
        for(int i = 0; i < amountOfFlames; i++) {
            Vector2 maxVariance = GetComponent<Collider2D>().bounds.extents;
            Instantiate<Fire>(firePrefab,this.transform.position + new Vector3(
                Random.Range(-maxVariance.x * 0.7f,maxVariance.x * 0.7f),
                Random.Range(-maxVariance.y * 0.7f,maxVariance.y * 0.7f),
                0),this.transform.rotation, transform);
        }
    }
        
    public void makeFlood() {
        if(isFlooded || isOnFire) return;
        isFlooded = true;
        renderWater();
    }

    private void renderWater() {
        renderer.color = Color.blue;
        renderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("entered room " + id);
        if(other.gameObject.tag == "Player") {
            isPlayerInside = true;
            player = other.GetComponentInParent<PlayerControler>();
        }
        else if(other.gameObject.tag == "Enemy") {
            enemies.Add(other.GetComponentInParent<Enemy>());
        }
        else if(other.gameObject.tag == "Locator") {
            locators.Add(other.GetComponentInParent<Locator>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("left room " + other.gameObject);
        if(other.gameObject.tag == "Player") {
            isPlayerInside = false;
            player = null;
        }
        else if(other.gameObject.tag == "Enemy") {
            enemies.Remove(other.GetComponentInParent<Enemy>());
        }
        else if(other.gameObject.tag == "Locator") {
            locators.Remove(other.GetComponentInParent<Locator>());
        }
    }

}
