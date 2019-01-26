using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController :MonoBehaviour {

    public Room[] rooms { set; get; }
    private List<Enemy> enemies;
    private Enemy enemyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void prepareRooms(int amountOfLocators) {
        rooms = GetComponentsInChildren<Room>();
        for(int i = 0; i < amountOfLocators; i++) {
            rooms[i % rooms.Length].spawnLocator();
        }
    }



    public void spawnEnemy(Room room) 
    {
        enemies.Add(Instantiate(enemyPrefab, room.transform));
    }

}
