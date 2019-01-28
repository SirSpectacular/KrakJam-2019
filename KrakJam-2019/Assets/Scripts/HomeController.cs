using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController :MonoBehaviour {

    public Room[] rooms { set; get; }
    private List<Enemy> enemies = new List<Enemy>();
    public Enemy enemyPrefab;
    public Locator locatorPrefab;
    private bool donePrepering = false;



    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        bool flag = false;
        if(donePrepering) {
            foreach(Room room in rooms) {
                if(room.isThereAnyLocator())
                    flag = true;
            }
            if(flag == false) GetComponentInParent<GameManager>().allLocatorsAreMad = true;
            foreach(Room room in rooms) {
                if(!room.isDestoyed)
                    return;
            }
            GetComponentInParent<GameManager>().allRoomsDestroyed = true;
        }
    }

    public void prepareRooms(int amountOfLocators) {
        rooms = GetComponentsInChildren<Room>();
        for(int i = 0; i < rooms.Length; i++) {
            rooms[i].id = i;
        }

        for(int i = 0; i < 4; i++) {
                rooms[i].adjacentRooms[0] = i > 0 ? rooms[i - 1] : null;
                rooms[i+4].adjacentRooms[0] = i > 0 ? rooms[i - 1 + 4] : null;
                rooms[i].adjacentRooms[1] = i < 4 ? rooms[i + 1] : null;
                rooms[i + 4].adjacentRooms[0] = i < 4 ? rooms[i - 1 + 4] : null;
        }
        rooms[8].adjacentRooms[0] = rooms[8].adjacentRooms[1] = null;

        for(int i = 0; i < amountOfLocators; i++) {
            int index = Random.Range( 0,7);
            rooms[index < 3 ? index : index + 1].spawnLocator(locatorPrefab);
        }
        donePrepering = true;
    }



    public void spawnEnemy(Room room, PlayerControler player) 
    {
        Vector2 maxVariance = room.GetComponent<Collider2D>().bounds.extents;
        Enemy enemy = Instantiate(enemyPrefab, room.transform.position + new Vector3(
                Random.Range(-maxVariance.x * 0.7f,maxVariance.x * 0.7f),
                0,0), room.transform.rotation, transform);
        enemy.player = player.gameObject;
        if(enemy != null) enemies.Add(enemy);
    }

}
