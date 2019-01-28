using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite Bucket;
    public Sprite FireExtinguisher;
    private SpriteRenderer renderer;
    private PlayerControler player;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<PlayerControler>();
        renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(player.getItem() == "FireExtinguisher") {
            renderer.sprite = FireExtinguisher;
            renderer.enabled = true;
        }
       else if(player.getItem() == "Bucket") {
            renderer.sprite = Bucket;
            renderer.enabled = true;
        }
       else {
            renderer.enabled = false;
        }
    }
}
