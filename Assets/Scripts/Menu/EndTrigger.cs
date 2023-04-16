// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class EndTrigger : MonoBehaviour
// {
//     /// <summary>
//     /// OnTriggerEnter is called when the Collider other enters the trigger.
//     /// </summary>
//     /// <param name="other">The other Collider involved in this collision.</param>

//     public Animator anim;
//     public Image image;
    
//     void OnTriggerEnter(Collider other)
//     {
        
//         if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
//             GameManager.Score = other.GetComponent<Score>().GetScore();
//             StartCoroutine(Fading());

              
//         }
//     }
    
//     IEnumerator Fading(){

//         anim.SetTrigger("Fade");
//         yield return new WaitUntil(()=>image.color.a == 1);
//         SceneManager.LoadScene("EndMenu"); 
//     }
     
// }
