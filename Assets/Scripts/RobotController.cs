using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotController : MonoBehaviour
{
    public float speed = 2.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public GameObject talking_image;
    float timer;
    int direction = 1;
    bool talking = false;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = changeTime;
        animator = GetComponent<Animator>();
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("TrashBin")) 
        {
            collision.gameObject.transform.parent = this.gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 && talking)
        {
            direction = -direction;
            timer = changeTime;
            talking_image.SetActive(false);
            talking = false;
            speed = 2.0f;
        }
        else if (timer < 0) 
        {
            talking_image.SetActive(true);
            timer = changeTime;
            talking = true;
            speed = 0;
        }
    }
    void FixedUpdate()
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

    
}

