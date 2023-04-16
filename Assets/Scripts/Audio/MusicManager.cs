using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MusicManager: MonoBehaviour
{   
    public FMODUnity.EventReference music;
    FMOD.Studio.EventInstance musicState;

    public bool inFirstLayer    = false; //Comme je veux
    public bool inSecondLayer   = false; // DeadBall
    public bool inThirdLayer    = false; // Max speed
    
    public float incFactor = 1f;
    public float decFactor = 1f;
    
    private float firstLayer    = 0f;
    private float secondLayer   = 0f;
    private float thirdLayer    = 0f;

    public FMODUnity.EventReference CoutdownSound;
    public FMODUnity.EventReference CoutdownFinalSound;
    public FMODUnity.EventReference SlowdownSound;




    


    private void Start() {
        musicState = FMODUnity.RuntimeManager.CreateInstance(music);
        musicState.start();

        musicState.setParameterByName("music_Layer1", 0f);
        musicState.setParameterByName("music_Layer2", 0f);
        musicState.setParameterByName("music_Layer3", 0f);
    }

    private void Update() {
       if(inFirstLayer){
            firstLayer = Mathf.Lerp(firstLayer, 1f, incFactor * Time.deltaTime);
       }else{
            firstLayer = Mathf.Lerp(firstLayer, 0f, decFactor * Time.deltaTime);
       }

        if(inSecondLayer){
            secondLayer = Mathf.Lerp(secondLayer, 1f, incFactor * Time.deltaTime);
        }else{
            secondLayer = Mathf.Lerp(secondLayer, 0f, decFactor * Time.deltaTime);
        }

        if(inThirdLayer){
            thirdLayer = Mathf.Lerp(thirdLayer, 1f, incFactor * Time.deltaTime);
        }else{
            thirdLayer = Mathf.Lerp(thirdLayer, 0f, decFactor * Time.deltaTime);
        }



        Debug.Log("ThirdLayer =" + thirdLayer);
         musicState.setParameterByName("music_Layer1", firstLayer);
         musicState.setParameterByName("music_Layer2", secondLayer);
         musicState.setParameterByName("music_Layer3", thirdLayer);    
    } 

    private void OnDestroy() {
        musicState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        musicState.setParameterByName("music_Layer1", 0f);
        musicState.setParameterByName("music_Layer2", 0f);
        musicState.setParameterByName("music_Layer3", 0f);

    }

    public void PlayCountdownSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(CoutdownSound, transform.position);
    }

    public void PlayCountdownFinalSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(CoutdownFinalSound, transform.position);
    }
    
    public void PlaySlowdownSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(SlowdownSound, transform.position);
    }

    public void TriggerSpeedParameter(){
        StartCoroutine(SpeedParameterRamp());
    }

    public IEnumerator SpeedParameterRamp(){
        float elapsedTime = 0f;
        float waitTime = 1f; 
        float speedParam=0f;
        while (elapsedTime < waitTime)
        {
            speedParam = Mathf.Lerp(speedParam, 1f, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SpeedUp",speedParam);
            // Yield here
            yield return null;
        }  
        // Make sure we got there
        speedParam = 1f;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SpeedUp",speedParam);
        yield return new WaitForSeconds(8f);

        elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            speedParam = Mathf.Lerp(speedParam, 0f, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SpeedUp",speedParam);
            // Yield here
            yield return null;
        }  
        speedParam = 0f;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("SpeedUp",speedParam);
        yield return null;
    }
}
