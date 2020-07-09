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
    GameObject trashbin;
    Animator animator;
    AIPath path;
    AIDestinationSetter ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<AIDestinationSetter>();
        path = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!holding_trashbin && count < 1)
        {
            if (collision.gameObject.tag.Equals("TrashBin"))
            {
                holding_trashbin = true;
                trashbin = collision.gameObject;
                trashbin.transform.parent = this.gameObject.transform;
                trashbin.transform.position = trashbin.transform.parent.position + new Vector3(1.1f, 0.5f, 0.0f);
                count++;
            }
        }
    }

    void FixedUpdate()
    {
        if (!pathfinding_mode)
        {
            Vector2 position = transform.position;
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

        }
    }

    [Task]
    void ActivePathFinding()
    {
        pathfinding_mode = true;
        path.canMove = true;
        path.canSearch = true;
        path.maxSpeed = speed;
        
        holding_trashbin = false;
        count = 0;
        Task.current.Succeed();
    }

    [Task]
    void DropBin()
    {
        holding_trashbin = false;
        trashbin.transform.parent = null;
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
        ai.target = GameObject.Find("TrashBin1" /*Random.Range(1, 4).ToString()*/).transform;
        Task.current.Succeed();
    }  
    [Task]
    void SetDestination() 
    {
        ai.target = GameObject.Find("Place").transform;
        Task.current.Succeed();
    }
    [Task]
    void Move() 
    {
        if (path.reachedDestination)
        {
            Task.current.Succeed();
        }
    }
}

