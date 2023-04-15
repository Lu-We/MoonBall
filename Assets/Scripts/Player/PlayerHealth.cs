using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    internal PlayerScript player;
    [SerializeField]
    private float health = 100f;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    public void InflictDamage(float amount){
        health -= amount;
        CheckDeath();
    }

    public void SetHealth(float amount){
        health = amount;
        CheckDeath();
    }
    public float GetHealth(){
       return health;        
    }

    private void CheckDeath(){
        if(health <= 0f ){
            Debug.Log("Dead");
            //LaunchDeadSequence
        }
    }

}
