using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room :MonoBehaviour {

    public int id { set; get; }
    public bool isOnFire;
    public bool isFlooded;
    public bool isPlayerInside;

    private SpriteRenderer renderer;
    
    
    public List<Enemy> enemies = new List<Enemy>();
    private List<Locator> locators = new List<Locator>();


    private void Awake() {
        isOnFire = false;
        isFlooded = false;
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

        }
    }
    public void spawnLocator(Locator prefab) {
        locators.Add(Instantiate(prefab, transform));
    }

    public void makeFire() {
        isOnFire = true;
        renderer.color = Color.red;
        renderer.enabled = true;
    }
        
    public void makeFlood() { 
        isFlooded = true;
        renderer.color = Color.blue;
        renderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("entered room " + id);
        if(other.gameObject.tag == "Player")
            isPlayerInside = true;
        else if(other.gameObject.tag == "Enemy") {
            enemies.Add(other.GetComponentInParent<Enemy>());
        }
        else if(other.gameObject.tag == "Locator") {
            locators.Add(other.GetComponentInParent<Locator>());
        }
    }

}
