using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using Pathfinding;

public class RobotController : MonoBehaviour
{
    public GameObject talking_image;

    int count = 0;
    int direction = 1;

    float speed = 2.0f;

    bool pathfinding_mode = false;
    bool vertical = false;
    bool holding_trashbin = false;
    string target = "";

    Vector2 last_frame_pos;
    GameObject trashbin;
    Animator animator;
    AIPath path;
    AIDestinationSetter ai;

    // Start is called before the first frame update
    void Start()
    {
        last_frame_pos = transform.position;
        ai = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!holding_trashbin)
        {
            if (collision.gameObject.name.Equals(target))
            {
                holding_trashbin = true;
                trashbin = collision.gameObject;
                trashbin.GetComponent<BoxCollider2D>().enabled = false;
                trashbin.transform.parent = this.gameObject.transform;
                trashbin.transform.position = trashbin.transform.parent.position + new Vector3(1.1f, 0.0f, 0.0f);
                count++;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;

        if (!pathfinding_mode)
        {
            if (speed > 0)
            {
                if (vertical)
                {
                    position.y = position.y + Time.deltaTime * speed * direction;
                    animator.SetFloat("Move X", 0);
                    animator.SetFloat("Move Y", direction);
                }
                else
                {
                    position.x = position.x + Time.deltaTime * speed * direction;
                    animator.SetFloat("Move X", direction);
                    animator.SetFloat("Move Y", 0);
                }
            }
            else
            {
                animator.SetFloat("Move X", 0);
                animator.SetFloat("Move Y", 0);
            }
            transform.position = position;
        }
        else 
        {
            Vector2 offset = position - last_frame_pos;
            last_frame_pos = transform.position;
            animator.SetFloat("Move X", offset.x * 10);
            animator.SetFloat("Move Y", 0);
        }
    }

    [Task]
    void ActivePathFinding()
    {
        pathfinding_mode = true;
        path.canMove = true;
        path.canSearch = true;
        path.maxSpeed = speed;
        
        Task.current.Succeed();
    }

    [Task]
    void DropBin()
    {
        target = "";
        holding_trashbin = false;
        trashbin.transform.parent = null;
        trashbin.GetComponent<BoxCollider2D>().enabled = true;
        pathfinding_mode = false;
        path.canMove = false;
        path.canSearch = false;
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
    void ChangeDirection() 
    {
        direction = -direction;
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
            target = "TrashBin" + Random.Range(1, 4).ToString();
            Debug.Log(target);
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
    [Task]
    void Move() 
    {
        if (path.reachedDestination)
        {
            ai.target = null;
            Task.current.Succeed();
        }
    }
}