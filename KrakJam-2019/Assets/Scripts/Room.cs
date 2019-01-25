using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room :MonoBehaviour { 

    public bool isOnFire;
    public GameObject locatorPrefab;
    public List<GameObject> locators;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnLocator() {
        locators.Add(Instantiate(locatorPrefab, transform));
    }

    public void makeFire() {

    }

    public void makeFlood() {

    }
}
