using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    internal PlayerScript player;
    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private float maxHealth = 100f;

    public Color lowHealth;

    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    private void Update() {
        healthBar.fillAmount = health / maxHealth;
    }

    public void InflictDamage(float amount){
        health -= amount;
        Mathf.Clamp(health,0,maxHealth);
        CheckStatus();
    }

    public void SetHealth(float amount){
        health = amount;
        Mathf.Clamp(health,0,maxHealth);
        CheckStatus();
    }
    public float GetHealth(){
       return health;        
    }

    private void CheckStatus(){
        if(health <= 0.3f * maxHealth){
            Debug.Log("LowHealth");
            healthBar.color = lowHealth;
        }

        if(health <= 0f ){
            Debug.Log("Dead");
            player.audioManager.PlayDeathSFX(player.transform);
            GameManager.Instance.RemovePlayer(player);
            Destroy(gameObject,.5f);
            //LaunchDeadSequence
        }
    }

}
