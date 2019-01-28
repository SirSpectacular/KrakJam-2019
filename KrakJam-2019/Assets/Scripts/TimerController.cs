using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerController : MonoBehaviour
{
    public Text TimerText;
    public PlayerControler player;
    public GameManager gameManager;
    public GameObject gameOver;

    private void Start() {
        gameOver.GetComponent<SpriteRenderer>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(!player.isDead && !gameManager.allLocatorsAreMad && !gameManager.allRoomsDestroyed) {
            float timeNow = Time.realtimeSinceStartup;
            int seconds;
            int minutes;
            seconds = (int)timeNow;

            minutes = seconds / 60;
            Debug.Log(minutes);
            seconds = seconds % 60;
            if(seconds < 10) TimerText.GetComponent<Text>().text = minutes + ":0" + seconds;
            else TimerText.GetComponent<Text>().text = minutes + ":" + seconds;
        }
        else {
            gameOver.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
