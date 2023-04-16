// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class UpdateTexteScore_Menu : MonoBehaviour
// {
//     // Start is called before the first frame update
//     public Text scoreText;
//     private float scoreLerp=0f;
//     private int toint;

//     // Update is called once per frame
//     void Update()
//     {
//         scoreLerp = Mathf.Lerp(scoreLerp,GameManager.Score, Time.deltaTime);
//         if(scoreLerp >= (GameManager.Score - 1) )
//             scoreLerp = GameManager.Score;

//         toint = (int)scoreLerp;
//         scoreText.text = toint.ToString("0");
//     }
// }
