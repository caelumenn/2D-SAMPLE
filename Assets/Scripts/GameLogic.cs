using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class GameLogic : MonoBehaviour
{
    public GameObject rubbish_prefab;
    public Text ui_time;
    public Text ui_score;
    public GameObject pauseMenu;
    public int stuck = 0;

    string[] rubbish_type = { "card paper", "metal plastic glass", "compost" };
    Color[] rubbish_color = { new Color(0.75f, 0.0f, 0.0f, 1.0f), new Color(1.0f,1.0f, 1.0f, 1.0f), new Color(0.4f, 0.0f, 0.0f, 1.0f) };

    int score = 0;
    float time = 60.0f;
    float re_time = 3.0f;
    bool isGameOver = false;
    
    public void addScore(int a) {
        score += a;
    }

    public void minScore() {
        score--;
        if (score < 0) {
            score = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        createRubbish();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            time -= Time.deltaTime;
            re_time -= Time.deltaTime;
            if (stuck > 2 || time <= 0)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                isGameOver = true;
            }
            else if (time > 0)
            {
                if (re_time < 0)
                {
                    createRubbish();
                    re_time = 3.0f;
                }
                ui_time.text = time.ToString();
                ui_score.text = score.ToString();

            }
        }
    }

    void createRubbish() {
        Vector3 position = new Vector3(-8.0f,Random.Range(-4.8f,-1.8f),1);
        GameObject rubbish = Instantiate(rubbish_prefab, position, Quaternion.identity);
        AIDestinationSetter ai =  rubbish.GetComponent<AIDestinationSetter>();
        ai.target = GameObject.Find("EndPoint").transform;

        int random_seed = Random.Range(0, 3);
        rubbish.GetComponent<Rubbish>().type = rubbish_type[random_seed];
        rubbish.GetComponent<SpriteRenderer>().color = rubbish_color[random_seed];
        
    }

    public void reStart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
