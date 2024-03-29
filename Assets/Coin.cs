using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public LevelManager levelManager;
    private void Start()
    {
        levelManager = LevelManager.FindAnyObjectByType<LevelManager>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelManager.CoinCounter();
            Destroy(gameObject);
        }
    }
}
