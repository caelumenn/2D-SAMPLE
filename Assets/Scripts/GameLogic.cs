using System;
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
    public Text page;
    public GameObject pauseMenu;
    public GameObject startMenu;
    public GameObject result;
    public GameObject rubbish_prefab;
    
    //For rubbish creat
    Sprite[] sprites;
    string[] rubbish_type = { "card paper", "metal plastic glass", "compost" };
    string[] rubbish_name = {"water bottle","milk jug", "laundry soap bottle",
                            "cardboard box", "newspaper", "brown bag",
                            "green soda bottle", "mason jar","beer bottle",
                            "small tuna can", "larger steel can", "red soda can",
                            "ceramic coffee mug", "greasy pizza box", "polystyrene foam cup", "pressurized spray can"};

    //For result page
    int current_page = 1;
    OrderedDictionary wrong_result = new OrderedDictionary();
    
    //For Game main logic
    public int stuck = 0;
    int score = 0;

    int speed = 2;
    int next_speed_level = 2;

    float time = 60.0f;
    float refresh_time = 1.5f;
    float next_refresh = 3f;

    bool isGameOver = false;
    static bool isRestart = false;

    int rubbish_Sum = 0;

    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("recycle_items");
        Application.targetFrameRate = 60;
        if (isRestart)
        {
            startMenu.SetActive(false);
            Time.timeScale = 1f;
            createRubbish();
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
        result.GetComponent<Text>().text = getResult();
        page.text = current_page.ToString() +"/"+getAllPage().ToString();

        if (!isGameOver)
        {
            time -= Time.deltaTime;
            refresh_time -= Time.deltaTime;
            if (stuck > 10 || time <= 0)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                isGameOver = true;
            }
            else if (time > 0)
            {
                if (next_speed_level == 0)
                {
                    speed++;
                    next_speed_level = 2;
                    next_refresh -= 0.5f;
                    if (next_refresh < 1.0f)
                    {
                        next_refresh = 1.0f;
                    }
                }
                if (refresh_time < 0)
                {
                    if (createRubbish())
                    {
                        refresh_time += next_refresh;
                    }
                }
                ui_time.text = time.ToString();
                ui_score.text = score.ToString();

            }
        }
    }

    public GameObject createRubbish()
    {
        int random_seed = UnityEngine.Random.Range(0, 16);
        if (!GameObject.Find(rubbish_name[random_seed]))
        {
            Vector3 position = new Vector3(-8.0f, UnityEngine.Random.Range(-4.8f, -1.8f), 1);
            GameObject rubbish = Instantiate(rubbish_prefab, position, Quaternion.identity);

            AIDestinationSetter ai = rubbish.GetComponent<AIDestinationSetter>();
            ai.target = GameObject.Find("EndPoint").transform;

            AIPath path = rubbish.GetComponent<AIPath>();
            path.maxSpeed = speed;
            rubbish.name = rubbish_name[random_seed];
            rubbish.GetComponent<SpriteRenderer>().sprite = sprites[random_seed];
            return rubbish;
        }
        return null;
    }

    int GetRubbishType(int index) 
    {
        if (index >= 0 && index < 3)
        {
            return 1;
        }
        else if (index >= 3 && index < 6)
        {
            return 0;
        }
        else if (index >= 6 && index < 13) 
        {
            return 1;
        }
        else 
        {
            return 2;
        }
    }

    public bool CheckType(string name, int index) 
    {
        int rubbish_index = Array.IndexOf(rubbish_name, name);

        return GetRubbishType(rubbish_index) == index;
    }

    public void addWrongResult(string r_name)
    {
        if (!wrong_result.Contains(r_name))
        {
            wrong_result.Add(r_name, rubbish_type[GetRubbishType(Array.IndexOf(rubbish_name, r_name))]);
        }
    }

    public void addScore(int a)
    {
        score += a;
        next_speed_level--;
    }

    public void minScore()
    {
        score--;
        if (score < 0)
        {
            score = 0;
        }
    }

    public void returnToPause() 
    {
        result.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void reStart()
    {
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void nextPage() 
    {
        current_page++;
        if (current_page > getAllPage())
        {
            current_page = getAllPage();
        }
    }

    public void prePage() 
    {
        current_page--;
        if (current_page < 1)
        {
            current_page = 1;
        }
    }

    public void startGame()
    {
        startMenu.SetActive(false);
        Time.timeScale = 1f;
        createRubbish();
    }

    public void showResult()
    {
        pauseMenu.SetActive(false);
        result.SetActive(true);
    }

    public void shutDown()
    {
        Application.Quit();
    }

    string getResult()
    {
        string s_result = "You have gain " + score.ToString() + " marks,\n" +
            "and here is the list of rubbishes you put in the wrong bin.\n";
        string[] keys = new string[wrong_result.Count];
        string[] values = new string[wrong_result.Count];
        wrong_result.Keys.CopyTo(keys, 0);
        wrong_result.Values.CopyTo(values, 0);


        if (current_page < getAllPage())
        {
            for (int i = (current_page - 1) * 10; i <= current_page * 10 - 1; i++)
            {
                s_result += keys[i] + " should be put in " + values[i] + " bin.\n";
            }
        }
        else //remember to keep current_page not larger than allpage
        {
            for (int i = (current_page - 1) * 10; i <= wrong_result.Count - 1; i++)
            {
                s_result += keys[i] + " should be put in " + values[i] + " bin.\n";
            }
        }


        return s_result;
    }

    int getAllPage() 
    {
        return (wrong_result.Count / 10) + 1;
    }
}