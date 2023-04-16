using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	//[SerializeField] TimerGame timer;
	private AsyncOperation asyncLoad;
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		if(SceneManager.GetActiveScene().name == "StartMenu" && thisIndex == 0)
		{
			StartCoroutine(LoadYourAsyncScene());
		}
		
	}
    // Update is called once per frame
    void Update()
    {
		if(menuButtonController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if(Input.GetAxis ("Submit") == 1){
				animator.SetBool ("pressed", true);
			}else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
		}

		if(thisIndex == 0 && animator.GetBool("pressed")){
			//timer.StartTimer();
			StartCoroutine(LaunchLevel());
			
		}
		if(thisIndex == 1 && animator.GetBool("pressed")){
			Debug.Log("Quit");
			Application.Quit();
		}
    }

	IEnumerator LoadYourAsyncScene()
    {
		yield return new WaitForSeconds(0.2f);

        asyncLoad = SceneManager.LoadSceneAsync("LevelMoon");
		asyncLoad.allowSceneActivation = false;

       
        while (!asyncLoad.isDone)
        {
			Debug.Log("Pro :" + asyncLoad.progress);
            yield return null;
        }
    }

	IEnumerator LaunchLevel(){
		yield return new WaitForSeconds(.2f);
		asyncLoad.allowSceneActivation = true;
	}
}

