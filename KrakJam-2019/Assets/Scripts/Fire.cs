using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire :MonoBehaviour {
    [Range(0f, 90f)]
    public float angle;
    private float timer;
    public float stepTime;
    public float maxSize;
    public float growRate;
    [Range(0, 1)]
    public float rotationOffsetVariance;
    private float rotated = 0;

    // Start is called before the first frame update
    void Start() {
        rotated += Random.Range(-angle * rotationOffsetVariance,angle * rotationOffsetVariance);
        GetComponent<SpriteRenderer>().sortingOrder = Random.Range(0,2);
        transform.Rotate(0,0, rotated, Space.Self);

        timer = 0f;
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update() {
        if(transform.localScale.x < maxSize) {
            timer += Time.deltaTime;
            if(timer > stepTime) {
                timer = 0f;
                transform.localScale *= growRate;
            }
        }
    }

    IEnumerator animate() {
        int tmp = 90;
        

        while(true) {
            transform.Rotate(0,0,angle * Time.deltaTime,Space.Self);
            rotated += angle * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            while(Mathf.Abs(rotated) < angle) {
                transform.Rotate(0,0,angle * Time.deltaTime, Space.Self);
                rotated += angle * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            transform.Rotate(0,0,-angle * Time.deltaTime,Space.Self);
            rotated -= angle * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            while(Mathf.Abs(rotated) < angle) {
                transform.Rotate(0,0,-angle * Time.deltaTime, Space.Self);
                rotated -= angle * Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

}


