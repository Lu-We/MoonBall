// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class EnablePlayerInput : MonoBehaviour
// {
//     //private PlateformMove plateform;

//     private void OnTriggerEnter(Collider other) {
//         if(other.CompareTag("Player")){          
//             Player player = other.GetComponent<Player>();
//             player.inputManager.enabled = true;
//             player.spawnHandler.spawnpoint = (Transform) Instantiate(new GameObject("SPAWN"), player.transform.position, Quaternion.identity).transform;
            
//         }else if(other.gameObject.TryGetComponent(out PlateformMove plateform)){
//             plateform.pauseDuration =  999f;
//             plateform.emitter.Stop();
//             plateform.transform.GetChild(1).SetParent(null);
//             plateform.transform.parent.gameObject.SetActive(false);       
//         }
//     }
// }
