using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public List<Transform> Tails;
    public float BoneDist;
    public GameObject BonePref;
    public UnityEvent eating;
    public GameObject ScoreText;
    public string sceneName;

    private int Score = 0;
    private Transform _transform;
    private float Speed = 10f;
    private bool isPause = false;

    [SerializeField]
    private GameObject obj;
    float xRand;
    float yRand;
    Vector3 spawnPos;
    
    void Start()
    {
        _transform = GetComponent<Transform>();
        xRand = Random.Range(-17.98f, 19.52f);
        yRand = Random.Range(-135.65f, -97.96f);
        spawnPos = new Vector3(xRand, 26.1f, yRand);
        Instantiate(obj, spawnPos, Quaternion.identity);
    }

    void Update()
    {
        MoveSnake(_transform.position + _transform.forward * Speed * Time.deltaTime);
        float angel = Input.GetAxis("Horizontal");
        _transform.Rotate(0, angel * Time.deltaTime * 200, 0);

        if(Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            Time.timeScale = 0;
            isPause = true;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPause)
        {
            Time.timeScale = 1;
            isPause = false;
        }
    }

    private void MoveSnake(Vector3 newPositin)
    {
        float distance = BoneDist * BoneDist;
        Vector3 prevPosition = _transform.position;

        foreach(var bone in Tails)
        {
            if ((bone.position - prevPosition).sqrMagnitude > distance)
            {
                var temp = bone.position;
                bone.position = prevPosition;
                prevPosition = temp;
            }
            else
            {
                break;
            }
        }
        _transform.position = newPositin;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 prevPosition = _transform.position;
        Destroy(other.gameObject);
        var bone = Instantiate(BonePref);
        var temp = bone.transform.position;
        bone.transform.position = prevPosition;
        prevPosition = temp;
        Tails.Add(bone.transform);
        Count();
        xRand = Random.Range(-17.98f, 19.52f);
        yRand = Random.Range(-135.65f, -97.96f);
        spawnPos = new Vector3(xRand, 26.1f, yRand);
        Instantiate(obj, spawnPos, Quaternion.identity);
        eating.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(sceneName);
    }
    void Count()
    {
        Score++;
        ScoreText.GetComponent<Text>().text = "Score: " + Score.ToString();
    }
}
