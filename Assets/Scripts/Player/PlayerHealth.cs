using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    internal PlayerScript player;

    private Material baseMat;
    private Material[] mats;

    [SerializeField]
    private Material blinkMat;

    [SerializeField]
    private float health = 100f;

    public float maxHealth = 100f;

    public Vector3 hitNormal;

    public Color lowHealth;

    public Image healthBar;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerScript>();
        mats = player.prenderer.materials;
        baseMat = mats[0];
    }

    private void Update() {
        healthBar.fillAmount = health / maxHealth;
    }

    public void InflictDamage(float amount){
        health -= amount;
        Mathf.Clamp(health,0,maxHealth);
        StartCoroutine(blink());
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
            //Debug.Log("LowHealth");
            healthBar.color = lowHealth;
        }

        if(health <= 0f ){
            //Debug.Log("Dead");
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

    public IEnumerator blink(){
        float elapsedTime = 0f;
        float waitTime = 0.02f; 
        bool blink = true;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.fixedDeltaTime;
            if(blink){
                mats[0] = blinkMat;
                player.prenderer.materials= mats;
            }
            else{
                mats[0] = baseMat;
                player.prenderer.materials = mats;
            }
            blink = !blink;
            // Yield here
            yield return new WaitForSeconds(0.07f);
        }  
        // Make sure we got there
       
        mats[0] = baseMat;
        player.prenderer.materials = mats;

        yield return new WaitForSeconds(8f);
    }

}
