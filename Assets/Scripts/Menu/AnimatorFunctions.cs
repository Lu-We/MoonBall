using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	public bool disableOnce;

	// void PlaySound(AudioClip whichSound){
	// 	if(!disableOnce){
	// 		menuButtonController.audioSource.PlayOneShot (whichSound,0.5f);
	// 	}else{
	// 		disableOnce = false;
	// 	}
	// }

	// void PlaySoundLow(AudioClip whichSound){
	// 	if(!disableOnce){
	// 		menuButtonController.audioSource.PlayOneShot (whichSound,0.2f);
	// 	}else{
	// 		disableOnce = false;
	// 	}
	// }
}	
