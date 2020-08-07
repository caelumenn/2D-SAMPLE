using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

public class Rubbish : MonoBehaviour
{
    GameLogic game;
    public AudioClip correct;
    public AudioClip wrong;

    Text rubbish_name;

    float startPosX;
    float startPosY;
    bool isBeingHeld = false;
    bool isOnceFall = false;

    AIPath path;
    Rigidbody2D r2d;

    void Start()
    {
        game = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        rubbish_name = GameObject.Find(gameObject.name + "/Canvas/Name").GetComponent<Text>();
        rubbish_name.text = gameObject.name;

        path = GetComponent<AIPath>();
        r2d = GetComponent<Rigidbody2D>();
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag.Equals("TrashBin"))
        {
            string name = other.gameObject.name;
            int index = Int16.Parse(name.Substring(name.Length - 1)); 
            
            if (game.CheckType(this.gameObject.name, index))
            {
                game.AddScore(1);
                game.PlaySound(correct);
                Destroy(gameObject);
            }
            else
            {
                game.MinScore();
                game.PlaySound(wrong);
                game.AddWrongResult(this.name);
                Destroy(gameObject);
            }

        }
        else if (other.tag.Equals("River"))
        {
            path.canMove = true;
            path.canSearch = true;
            r2d.gravityScale = 0;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (other.tag.Equals("Street"))
        {
            if (!isOnceFall)
            {
                path.canMove = false;
                path.canSearch = false;
                GetComponent<BoxCollider2D>().isTrigger = false;
                isOnceFall = true;
            }
            else 
            {
                Destroy(gameObject);
                game.MinScore();
                game.PlaySound(wrong);
            }
        }
        else if (other.tag.Equals("End")) 
        {
            game.stuck++;
            Destroy(gameObject);
        }
        else
        {
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
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - this.transform.localPosition.x;
            startPosY = mousePos.y - this.transform.localPosition.y;
      
            isBeingHeld = true;
            GetComponent<BoxCollider2D>().isTrigger = false;
            rubbish_name.enabled = true;
        }
    }

    void OnMouseUp()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        path.canMove = false;
        path.canSearch = false;
        isBeingHeld = false;
        r2d.gravityScale = 1;
        rubbish_name.enabled = false;
    }
}
