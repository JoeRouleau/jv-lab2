using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoring : MonoBehaviour
{
    [SerializeField] private int score = 600;
    private bool finishedLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DecreaseScore", 0f, 1f);   
    }

    // Update is called once per frame
    void Update()
    {
        if(finishedLevel)
        {
            GameManager.instance.AddScore(score);
            finishedLevel = false;
        }
    }

    void DecreaseScore()
    {
        score = score - 5;
        Debug.Log("Score: " + score);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            finishedLevel = true;
        }
    }
}
