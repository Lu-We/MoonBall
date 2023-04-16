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

    public Vector3 hitNormal;

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
            player.audioManager.PlayDeathSFX();
            
            
            player.inputManager.enabled = false;
            player.movementManager.enabled = false;

            player.playerRb.constraints = RigidbodyConstraints.None;
            player.playerRb.AddForce(hitNormal , ForceMode.Impulse);

            Vector3 torque;
            torque.x = Random.Range (-5, 5);
            torque.y = Random.Range (-5, 5);
            torque.z = Random.Range (-5, 5);
            player.GetComponent<ConstantForce>().torque = torque;
            GameManager.Instance.RemovePlayer(player,3f);
            Destroy(gameObject,5f);
        }
    }

}
