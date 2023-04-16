using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlayerScript player;
    private List<PlayerScript> players = new List<PlayerScript>();
    void Awake()
    {
        Instance = this;

        var playersarr = FindObjectsOfType<PlayerScript>();
        foreach(var p in playersarr){
            players.Add(p);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Player Count :" + players.Count);        
        CheckWinner();
    }

    public void RemovePlayer(PlayerScript player){        
        players.Remove(player);
    }

    private void CheckWinner(){
        if(players.Count == 1){
            Debug.Log("Winner : " + players[0]);
        }
    }
}
