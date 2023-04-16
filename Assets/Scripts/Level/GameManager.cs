using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public MusicManager musicManager;

    private PlayerScript player;
    private List<PlayerScript> players = new List<PlayerScript>();

    public GameObject winCamPrefab;
    private GameObject winCam;

    [SerializeField]
    private Image moon1;
    [SerializeField]
    private Image moon2;
    [SerializeField]
    private Image moon3;
    [SerializeField]
    private Image moon4;
    [SerializeField]
    private Image moon5;
    [SerializeField]
    private Image moon6;
    [SerializeField]
    private Image moon7;
    [SerializeField]
    private Image moonBlood;


    private bool gameStopped = false;

    private float defaultFixedDeltaTime;

    public Transform moon;
    public GameObject ballPrefab;
    public GameObject deadlyBallPrefab;
    public GameObject raquettePrefab;

    private float tenSec = 10f;
    private float nextEventTime = 0f;
    private float nextEventTime1 = -1f;
    private float nextEventTime2 = -2f;
    private float nextEventTime3 = -3f;

    void Awake()
    {   
        nextEventTime = Time.time + tenSec;
        nextEventTime1 += Time.time + tenSec;
        nextEventTime2 += Time.time + tenSec;
        nextEventTime3 += Time.time + tenSec;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
        Instance = this;

        
        DisableMoonsUI();

        var playersarr = FindObjectsOfType<PlayerScript>();
        foreach(var p in playersarr){
            players.Add(p);
        }
    }

    private void DisableMoonsUI(){
        moon1.enabled = false;
        moon2.enabled = false;
        moon3.enabled = false;
        moon4.enabled = false;
        moon5.enabled = false;
        moon6.enabled = false;
        moon7.enabled = false;
        moonBlood.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {

        if(!gameStopped){
            if(Time.time >= nextEventTime){
                DisableMoonsUI();
                musicManager.PlayCountdownFinalSFX();
                MoonEvent(4);//Random.Range(0,7));
                nextEventTime = Time.time + tenSec;
            }
            else if(Time.time >= nextEventTime1){
                musicManager.PlayCountdownSFX();
                nextEventTime1 = Time.time + tenSec;
            }
            else if(Time.time >= nextEventTime2){
                musicManager.PlayCountdownSFX();
                nextEventTime2 = Time.time + tenSec;
            }
            else if(Time.time >= nextEventTime3){
                musicManager.PlayCountdownSFX();
                nextEventTime3 = Time.time + tenSec;
            }

            //Debug.Log("Player Count :" + players.Count);        
            CheckWinner();
        }

    }



    public void RemovePlayer(PlayerScript player, float delay){   
            StartCoroutine(RemPlayer(player, delay));
    }

    IEnumerator RemPlayer(PlayerScript player,float delay){
        yield return new WaitForSecondsRealtime(delay); 
        players.Remove(player);
    }

    private void CheckCaDingDong(){
        if (musicManager.inThirdLayer) 
            return;
        foreach(PlayerScript p in players){
            if(p.playerHealth.GetHealth() <= 0.4f * p.playerHealth.maxHealth){
                musicManager.inThirdLayer = true;
            }
        }
    }
    private void CheckWinner(){
        if(players.Count == 1){
            StopGame();

            Debug.Log("Winner : " + players[0]);

            winCam = Instantiate(winCamPrefab, players[0].transform.position, players[0].transform.rotation, players[0].transform);
        }
    }

    private void StopGame(){
        gameStopped = true;
        StopAllCoroutines();
        var balls = FindObjectsOfType<Ball>();
        foreach(Ball b in balls){
            b.ballSpeed = 0f;
        }

        players[0].inputManager.enabled = false;
        players[0].movementManager.enabled = false;
    }

    private void MoonEvent(int eventId){
        var balls = FindObjectsOfType<Ball>();

        switch(eventId){
            case 0: // Ajout de ball
                moon1.enabled = true;
                Debug.Log("AddBall");
                Vector3 randomVector = new Vector3(Random.Range(0,1f),Random.Range(0,1f),0f).normalized * 50f; 
                var ball = Instantiate(ballPrefab, moon.position + randomVector, Quaternion.LookRotation(randomVector,Vector3.up) );
                ball.GetComponent<Ball>().SetMoon(moon);
                ball.GetComponent<Ball>().SetCurve(Random.Range(0,3));
                if(Random.Range(0f,1f) >= 0.5f) ball.GetComponent<Ball>().ChangeDirection();

                break;
            case 1: // speed up
                moon2.enabled = true;
                Debug.Log("SpeedUp");
                musicManager.TriggerSpeedParameter();
                StartCoroutine(SpeedEvent(1.5f));
                break;
            case 2: // slow down
                moon3.enabled = true;
                Debug.Log("SpeedDown");
                musicManager.PlaySlowdownSFX();
                StartCoroutine(SpeedEvent(0.5f));
                break;
            case 3:  // Deadly ball (can't retourner)
                moonBlood.enabled = true;
                Debug.Log("SpawnDeadlyBall");
                Vector3 randomVector1 = new Vector3(Random.Range(0,1f),Random.Range(0,1f),0f).normalized * 50f; 
                var ball1 = Instantiate(deadlyBallPrefab, moon.position + randomVector1, Quaternion.LookRotation(randomVector1,Vector3.up) );

                Debug.Log("SpawnDeadlyBall" + (moon.position + randomVector1));
                ball1.GetComponent<DeadlyBall>().SetMoon(moon);
                ball1.GetComponent<DeadlyBall>().SetCurve(Random.Range(0,3));
                if(Random.Range(0f,1f) >= 0.5f) ball1.GetComponent<DeadlyBall>().ChangeDirection();
                StartCoroutine(DeadlyBall());
                break;
            case 4:  // change ball direction + spawn random raquette
                moon5.enabled = true;
                Debug.Log("ChangeDir+Raquette");
                foreach(Ball b in balls){
                    b.ChangeDirection();
                }
                GameObject hitbox;
                int curve = Random.Range(0,3);
                Vector3 offset = new Vector3(curve * 4f, 0, 0);
                Vector3 position = (moon.transform.position + moon.transform.right) * 27f;
                Vector3 position2 = (moon.transform.position + moon.transform.right) * -27f;
        
                hitbox = Instantiate(raquettePrefab, position, Quaternion.LookRotation(position,Vector3.up));
                hitbox.GetComponent<raquette>().InitRaquette(offset, 5, 10f, curve);
                hitbox = Instantiate(raquettePrefab, position2, Quaternion.LookRotation(position2,-Vector3.up));
                hitbox.GetComponent<raquette>().InitRaquette(-offset, 5, 10f, curve);
                break;
            case 5: // Set all ball max speed
                moon6.enabled = true;
                Debug.Log("Max Speed");
                StartCoroutine(MaxSpeedEvent(balls));
                break;
            case 6: // Double damage time
                moon7.enabled = true;
                Debug.Log("Double Damage");
                StartCoroutine(DoubleDamageEvent(balls));
                break;

            default:
                break;        
        }
    }

    public IEnumerator SpeedEvent(float factor){
        Time.timeScale = factor;
        Time.fixedDeltaTime = Time.timeScale * defaultFixedDeltaTime;
        yield return new WaitForSecondsRealtime(9.5f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime; 
    }

    public IEnumerator HitStop(float factor, float duration){
        Time.timeScale = factor;
        Time.fixedDeltaTime = Time.timeScale * defaultFixedDeltaTime;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime; 
    }

    public IEnumerator DoubleDamageEvent(Ball[] balls){
        foreach(Ball b in balls){
            b.SetDamageMultiply(2f);
        }
                
        yield return new WaitForSecondsRealtime(9.5f);
        foreach(Ball b in balls){
            b.SetDamageMultiply(1f);
        }
    }

    public IEnumerator MaxSpeedEvent(Ball[] balls){
        float[] speeds = new float[balls.Length];
        int idx = 0;
        foreach(Ball b in balls){
            speeds[idx] = b.ballSpeed;
            b.SetMaxSpeed();
            idx++;
        }     
        //musicManager.inThirdLayer = true;           
        yield return new WaitForSecondsRealtime(9.5f);
       // musicManager.inThirdLayer = false;  
        idx = 0;
        foreach(Ball b in balls){
            b.SetSpeed(speeds[idx]);
            idx++;
        }
    }

    public IEnumerator DeadlyBall(){
        musicManager.inSecondLayer = true;
        yield return new WaitForSecondsRealtime(9.5f);
        musicManager.inSecondLayer = false;
    }

   
}
