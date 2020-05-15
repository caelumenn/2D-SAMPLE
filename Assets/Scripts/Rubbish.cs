using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour
{
    public float speed = 0.1f;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("collider");
        speed = 0.0f;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        position.x += speed * Time.deltaTime;
        transform.position = position;
    }

    
}
