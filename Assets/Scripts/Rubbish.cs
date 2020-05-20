using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Rubbish : MonoBehaviour
{
    float startPosX;
    float startPosY;
    bool isBeingHeld = false;
    AIPath path;
    Rigidbody2D r2d;

    void Start()
    {
        path = GetComponent<AIPath>();
        r2d = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("River"))
        {
            path.canMove = true;
            path.canSearch = true;
            r2d.gravityScale = 0;
        }
        else
        {
            path.canMove = false;
            path.canSearch = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
 
    void Update()
    {
        if (isBeingHeld) 
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);

        }
        else
        {
            //Vector2 position = transform.position;
            //position += speed * Time.deltaTime;
            //transform.position = position;
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) {
            path.canMove = false;
            path.canSearch = false;

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
        r2d.gravityScale = 1;
    }

}
