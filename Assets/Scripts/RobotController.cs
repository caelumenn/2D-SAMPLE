using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;
using UnityEngine.UI;

public class RobotController : MonoBehaviour
{
    public GameObject talking_image;

    int count = 0;

    float speed = 2.0f;

    bool holding_Trashbin = false, holding_Trash = false;
    string target = "";

    Vector2 last_frame_pos;

    GameObject trashbin, trash;

    Animator animator;
    AIPath path;
    AIDestinationSetter ai;

    GameLogic game;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        last_frame_pos = transform.position;

        ai = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        path.canMove = true;
        path.canSearch = true;
        path.maxSpeed = speed;

        animator = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        if (holding_Trash) 
        {
            Destroy(trash);
            holding_Trash = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!holding_Trashbin)
        {
            if (collision.gameObject.name.Equals(target))
            {
                holding_Trashbin = true;
                trashbin = collision.gameObject;
                trashbin.GetComponent<BoxCollider2D>().enabled = false;
                GameObject.Find(trashbin.name + "/Canvas/Name").GetComponent<Text>().enabled = false;
                trashbin.transform.parent = this.gameObject.transform;
                trashbin.transform.position = trashbin.transform.parent.position + new Vector3(1.1f, 0.0f, 0.0f);
                count++;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        Vector2 offset = position - last_frame_pos;
        last_frame_pos = transform.position;
        animator.SetFloat("Move X", offset.x * 10);
        animator.SetFloat("Move Y", offset.y * 10);
    }

    [Task]
    void DropBin()
    {
        target = "";
        ai.target = null;
        holding_Trashbin = false;
        trashbin.transform.parent = null;
        trashbin.GetComponent<BoxCollider2D>().enabled = true;
        GameObject.Find(trashbin.name + "/Canvas/Name").GetComponent<Text>().enabled = true;
        Task.current.Succeed();
    }

    [Task]
    void StartTalking()
    {
        talking_image.SetActive(true);
        speed = 0;
        Task.current.Succeed();
    }

    [Task]
    void StopTalking()
    {
        talking_image.SetActive(false);
        speed = 2.0f;
        Task.current.Succeed();
    }

    [Task]
    void IsStart() 
    {
        if (Time.timeScale == 0) 
        {
            Task.current.Fail();
        }
        else 
        { 
            Task.current.Succeed(); 
        }
    }

    [Task]
    void SetTarget() 
    {
        if (ai.target == null)
        {
            target = "TrashBin" + Random.Range(0,3).ToString();
            ai.target = GameObject.Find(target + "/Offset").transform;
            Task.current.Succeed();
        }
        else
        {
            Task.current.Succeed();
        }
    }
    
    [Task]
    void SetDestination() 
    {
        if (holding_Trashbin)
        {
            if (count < 4)
            {
                ai.target = GameObject.Find("BackGround/Place/" + count.ToString()).transform;
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
        else 
        {
            if (ai.target == null)
            {
                ai.target = GameObject.Find("BackGround/Place/" + Random.Range(0, 5).ToString()).transform;
                float distance = Mathf.Abs(ai.target.transform.position.x - this.transform.position.x);
                if (distance < 1.0f)
                {
                    ai.target = null;
                }
                else 
                {
                    Task.current.Succeed();
                }
            }
            else 
            { 
                Task.current.Succeed(); 
            }
        }
    }
    [Task]
    void Move() 
    {
        if (path.reachedDestination)
        {
            ai.target = null;
            Task.current.Succeed();
        }
    }

    [Task]
    void CreateTrash() 
    {
        if (!holding_Trash) 
        {
            trash = game.CreateRubbish();
            if (trash) 
            {
                trash.GetComponent<Rigidbody2D>().gravityScale = 0;
                trash.GetComponent<AIPath>().canMove = false;
                trash.GetComponent<AIPath>().canSearch = false;
                trash.transform.parent = this.gameObject.transform;
                trash.transform.position = trash.transform.parent.position + new Vector3(1.0f, 0.0f, 0.0f);
                holding_Trash = true;
            }
            Task.current.Succeed();
        }
        else 
        { 
            Task.current.Succeed(); 
        }
    }

    [Task]
    void DropTrash() 
    {
        if (holding_Trash)
        {
            trash.transform.parent = null;
            trash.GetComponent<Rigidbody2D>().gravityScale = 1;
            holding_Trash = false;
            ai.target = null;
            Task.current.Succeed();
        }
        else 
        {
            Task.current.Fail();
        }
    }
}