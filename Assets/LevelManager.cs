using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    private int coinCounter =0;
    private int healthCounter;
    public TextMeshProUGUI CoinCounterText;
    public TextMeshProUGUI HealthText;
    public GameObject youWonPanel;
    public AudioSource goblindeathSound;
    public AudioSource goblinhitSound;
    public AudioSource swordSound;
    public AudioSource bGSound;
    public AudioSource winSound;
    public AudioSource coinSound;
    public GameObject player;

    private void Start()
    {
       
    }
    public void CoinCounter()
    {
        coinSound.pitch =Random.Range(1f, 1.5f);
        coinCounter++;
        coinSound.Play();
    }
    public void GoblinDeathSound()
    {
        goblindeathSound.pitch =Random.Range(0.7f, 1f);
        goblindeathSound.Play();
    }
    public void GoblinHitSound()
    {
        goblinhitSound.pitch =Random.Range(0.7f, 1f);
        goblinhitSound.Play();
    }
    public void SwordSound()
    {
        swordSound.pitch =Random.Range(0.9f, 1.2f);
        swordSound.Play();
    }
    public void  WinSound()
    {
        winSound.Play();
    }
    public void PlinPlingSound()
    {
        bGSound.Play() ;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            youWonPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }
    private void Update()
    {
         player = GameObject.FindGameObjectWithTag("Player");
        Player playerScriptComponent = player.GetComponent<Player>();
        healthCounter = playerScriptComponent.currentHealth;
        
        CoinCounterText.text = "Coins: " + coinCounter.ToString(); ;
        HealthText.text = "Health: " + healthCounter.ToString(); ;
    }
    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // kaleite sto deathscreenbutton
    }
}
