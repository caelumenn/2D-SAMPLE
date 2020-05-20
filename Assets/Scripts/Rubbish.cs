using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private bool isBeingHeld = false;
    public Vector2 speed = new Vector2(0, 0);
    public Vector2 acceleration = new Vector2(0, 0);
    

    void Start()
    {
        speed.Set(0, 0);
        
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("collider");
        
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
 
    void Update()
    {
        speed += acceleration * Time.deltaTime;

        if (isBeingHeld) 
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);

        }
        else
        {
            Vector2 position = transform.position;
            position += speed * Time.deltaTime;
            transform.position = position;
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;

            isBeingHeld = true;
        }
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;
    }

}
