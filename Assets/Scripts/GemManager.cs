using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        audioSource = gameObject.GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameManager.instance.AddScore(50);
            gameObject.SetActive(false);
            audioSource.PlayOneShot(SoundManager.Instance.GemCollected);
        }
    }
}
