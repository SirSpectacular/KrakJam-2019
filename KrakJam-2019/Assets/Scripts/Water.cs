using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water :MonoBehaviour {
    public float maxSize;
    private float scaled = 0;

    // Start is called before the first frame update
    void Start() {
        scaled += Random.Range(1,maxSize);
        transform.localScale = Vector3.Scale(transform.localScale,Vector3.one * scaled);

        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator animate() {
        float growFactor = 5;
        while(Mathf.Abs(scaled) < maxSize) {
            transform.localScale = Vector3.Scale(transform.localScale,Vector3.one * (1 + Time.deltaTime));
            scaled *= 1 + Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime * growFactor);
        }
    }
}
