using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room :MonoBehaviour {

    public int id { set; get; }
    public bool isOnFire = false;
    public bool isFlooded = false;
    public bool hasRat = false;
    public bool isPlayerInside;

    public float fireSpreedRate;
    private  float currentTime = 0;
    private int step = 1;
    public int burnDamage = 5;
    public int floodDamge = 3;
    public int ratDamage = 1;
    public float dmgInterval = 1f;

    public Room[] adjacentRooms = new Room[2];
    public Fire firePrefab;
    public Water waterPrefab;

    private SpriteRenderer renderer;

    public List<Enemy> enemies = new List<Enemy>();
    private List<Locator> locators = new List<Locator>();
    private PlayerControler player;
    private HomeController home;

    public bool isThereAnyLocator() {
        return locators.Count > 0 ? true : false;
    }

    private float actionTimer = 0;
    private float actionTime = 3;

    private float durability = 30;
    private float HPtimer = 0;
    private bool flag = true;
    public bool isDestoyed = false;
    private Fog fog;

    private void Awake() {
        fog = GetComponentInChildren<Fog>();
        fog.GetComponent<SpriteRenderer>().enabled = false;
        home = GetComponentInParent<HomeController>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if(HPtimer > durability && flag) {
            flag = false;
            destroyRoom();
        }

        if(isFlooded) {
            currentTime += Time.deltaTime;
            HPtimer += Time.deltaTime;
            if(currentTime >= dmgInterval) {
                dealDmg(floodDamge);
                currentTime = 0;
                if(enemies.Count != 0) {
                    dealDmg(ratDamage);
                }
            }

            if(isPlayerInside && player.getItem() == "Bucket" && Input.GetKey(KeyCode.Z)) {
                actionTimer += Time.deltaTime;
                if(actionTimer > actionTime) {
                    Water[] pools = GetComponentsInChildren<Water>();
                    foreach(Water pool in pools) {
                        pool.GetComponentInChildren<SpriteRenderer>().enabled = false;
                        Destroy(pool);
                    }
                    isFlooded = false;
                }
            }
            else {
                actionTimer = 0;
            }
        }
        else if(isOnFire) {
            currentTime += Time.deltaTime;
            HPtimer += Time.deltaTime;

            if(currentTime >= fireSpreedRate) {
                int index = Random.Range(0,1);
                if(adjacentRooms[index] != null) adjacentRooms[index].makeFire();
                else if(adjacentRooms[(index + 1) % 2] != null) adjacentRooms[(index + 1) % 2].makeFire();
                currentTime = 0;
                step = 1;
            }
            else if(currentTime >= dmgInterval * step) {
                dealDmg(burnDamage);
                if(enemies.Count != 0) {
                    dealDmg(ratDamage);
                }
                step++;
            }

            if(isPlayerInside && player.getItem() == "FireExtinguisher" && Input.GetKey(KeyCode.Z)) {
                Debug.Log("Extinguising");
                actionTimer += Time.deltaTime;
                player.startParticle();
                if(actionTimer > actionTime) {
                    Fire[] flames = GetComponentsInChildren<Fire>();
                    foreach(Fire flame in flames) {
                        flame.GetComponentInChildren<SpriteRenderer>().enabled = false;
                        Destroy(flame);
                    }
                    isOnFire = false;
                }
            }
            else {
                actionTimer = 0;
            }
        }
        else if(enemies.Count != 0) {
            if(currentTime >= dmgInterval) {
                currentTime = 0;
                dealDmg(ratDamage);

            }

        }
    }

    private void destroyRoom() {
        Vector2 size = GetComponent<Collider2D>().bounds.extents;
        fog.GetComponent<SpriteRenderer>().enabled = true;
        isDestoyed = true;
    }

    public void dealDmg(int damage) {
        foreach(Locator locator in locators.ToArray())
           locator.ReceiveDamage(damage);
       if(isPlayerInside) player.receiveDamage((float)damage);
    }

    public void spawnLocator(Locator prefab) {
        Vector2 maxVariance = GetComponent<Collider2D>().bounds.extents;
        locators.Add(Instantiate(prefab, this.transform.position + new Vector3(
                Random.Range(-maxVariance.x * 0.7f,maxVariance.x * 0.7f),
                0, 0),this.transform.rotation, transform)); 
    }


    public void makeFire() {
        if(isFlooded || isOnFire) return;
        isOnFire = true;
        StartCoroutine(renderFire());
    }



    private IEnumerator renderFire() {
        int amountOfFlames = Random.Range(4,10);
        for(int i = 0; i < amountOfFlames; i++) {
            Vector2 maxVariance = GetComponent<Collider2D>().bounds.extents;
            Instantiate<Fire>(firePrefab,this.transform.position + new Vector3(
                Random.Range(-maxVariance.x * 0.7f,maxVariance.x * 0.7f),
                Random.Range(-maxVariance.y * 0.7f,maxVariance.y * 0.7f),
                0),this.transform.rotation, transform);
            yield return new WaitForSeconds(Random.Range(0f,2f));
        }
    }


    public void makeFlood() {
        if(isFlooded || isOnFire) return;
        isFlooded = true;
        StartCoroutine(renderWater());
    }

    private IEnumerator renderWater() {
        int amountOfWater = Random.Range(2,3);
        for(int i = 0; i < amountOfWater; i++) {
            Vector2 maxVariance = GetComponent<Collider2D>().bounds.extents;
            Instantiate<Water>(waterPrefab,this.transform.position + new Vector3(
                Random.Range(-maxVariance.x * 0.7f,maxVariance.x * 0.7f),
                Random.Range(-maxVariance.y * 0.7f, 0),
                0),this.transform.rotation,transform);
            yield return new WaitForSeconds(Random.Range(0f,3f));
        }
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

     public void killMePlis(Locator loc,Enemy madman) 
     {
        locators.Remove(loc);
        Instantiate(madman, transform.position, transform.rotation, transform);
          Destroy(loc.gameObject);
    }
}
