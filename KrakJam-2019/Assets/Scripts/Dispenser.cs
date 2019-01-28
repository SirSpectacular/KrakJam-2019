using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{

    public string content;
    private bool playerNerby;

    private PlayerControler player;

    private void Start() {
        playerNerby = false;
        player = GetComponentInParent<GameManager>().GetComponentInChildren<PlayerControler>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            playerNerby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            playerNerby = false;
        }
    }


    private void Update() {
        if(playerNerby && Input.GetKeyDown(KeyCode.X)) {
            player.setItem(content);
        }
    }


}
