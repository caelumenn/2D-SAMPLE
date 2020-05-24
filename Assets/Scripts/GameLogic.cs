using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GameLogic : MonoBehaviour
{
    public GameObject rubbishPrefab;
    // Start is called before the first frame update
    void Start()
    {
        createRubbish();
        createRubbish();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createRubbish() {
        Vector3 position = new Vector3(-8.0f,Random.Range(-4.8f,-1.8f),1);
        Instantiate(rubbishPrefab, position, Quaternion.identity);
        AIDestinationSetter ai =  rubbishPrefab.GetComponent<AIDestinationSetter>();
        ai.target = GameObject.Find("EndPoint").transform;
    }
}
