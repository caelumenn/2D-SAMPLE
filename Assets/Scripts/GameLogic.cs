using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class GameLogic : MonoBehaviour
{

    public Text ui_time;
    public Text ui_score;
    public GameObject pauseMenu;
    public GameObject startMenu;
    public GameObject result;
    public GameObject next_page_button;
    public GameObject pre_page_button;
    public GameObject restart_button;
    public GameObject result_button;
    public GameObject back_button;
    public GameObject quit_button;
    public GameObject rubbish_prefab;

    public int stuck = 0;
    public static bool isRestart = false;

    int i = 0;
    string[] rubbish_type = { "card paper", "metal plastic glass", "compost" };
    Color[] rubbish_color = { new Color(0.75f, 0.0f, 0.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(0.4f, 0.0f, 0.0f, 1.0f) };

    int score = 0;
    float time = 60.0f;
    float re_time = 3.0f;
    bool isGameOver = false;
    OrderedDictionary wrong_result = new OrderedDictionary();

    // Start is called before the first frame update
    void Start()
    {
        addWrongResult("1", "2");
        addWrongResult("2", "3");
        addWrongResult("4", "5");

        Application.targetFrameRate = 60;
        if (isRestart)
        {
            startMenu.SetActive(false);
            Time.timeScale = 1f;
            createRubbish(99);
        }
        else
        {
            startMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            time -= Time.deltaTime;
            re_time -= Time.deltaTime;
            if (stuck > 0 || time <= 0)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                isGameOver = true;
            }
            else if (time > 0)
            {
                if (re_time < 0)
                {
                    createRubbish(i);
                    re_time = 3.0f;
                    i++;
                }
                ui_time.text = time.ToString();
                ui_score.text = score.ToString();

            }
        }
    }

    void createRubbish(int a)
    {
        Vector3 position = new Vector3(-8.0f, Random.Range(-4.8f, -1.8f), 1);
        GameObject rubbish = Instantiate(rubbish_prefab, position, Quaternion.identity);
        AIDestinationSetter ai = rubbish.GetComponent<AIDestinationSetter>();
        ai.target = GameObject.Find("EndPoint").transform;

        int random_seed = Random.Range(0, 3);
        rubbish.name = "abc" + a.ToString();
        rubbish.GetComponent<Rubbish>().type = rubbish_type[random_seed];
        rubbish.GetComponent<SpriteRenderer>().color = rubbish_color[random_seed];

    }

    public void addWrongResult(string r_name, string r_type)
    {
        if (!wrong_result.Contains(r_name))
        {
            wrong_result.Add(r_name, r_type);
        }
    }

    public void addScore(int a)
    {
        score += a;
    }

    public void minScore()
    {
        score--;
        if (score < 0)
        {
            score = 0;
        }
    }

    public void reStart()
    {
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void startGame()
    {
        startMenu.SetActive(false);
        Time.timeScale = 1f;
        createRubbish(98);
    }

    public void showResult()
    {
        restart_button.SetActive(false);
        result_button.SetActive(false);
        quit_button.SetActive(false);
        result.SetActive(true);
        result.GetComponentInChildren<Text>().text = getResult();
    }

    string getResult() 
    {
        string s_result = "You have gain " + score.ToString() + " marks,\n" +
            "and here are the list of rubbishs you put in the wrong bin.\n";
        if (wrong_result.Count < 10) 
        {
            foreach (KeyValuePair<string, string> entry in wrong_result) 
            {
                s_result += entry.Key + " should be put in " + entry.Value + " bin.\n";
            }
        }
        
        
        return s_result;
    }
}