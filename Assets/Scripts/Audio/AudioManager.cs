using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    internal PlayerScript player;

    public FMODUnity.EventReference DashSound;
    public FMODUnity.EventReference JumpSound;
    public FMODUnity.EventReference StepSound;
    public FMODUnity.EventReference DeathSound;
    public FMODUnity.EventReference LevelCompletedSound;
    public FMODUnity.EventReference AttackUp;
    public FMODUnity.EventReference AttackMid;
    public FMODUnity.EventReference AttackDown;

    public float stepSoundPeriod = 0.2f;
    public float stepSoundTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= stepSoundTimer
            && player.movementManager.GetComponent<Rigidbody>().velocity.magnitude > 0.2f
            && player.stateManager.GetIsDashing() == false && player.stateManager.GetIsGrounded() == true ){

            PlayStepSFX();
            stepSoundTimer = Time.time + stepSoundPeriod;
        }
    }

    public void PlayStepSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(StepSound, player.transform.position);
    }

    public void PlayDashSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(DashSound, player.transform.position);
    }

    public void PlayJumpSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(JumpSound, player.transform.position);
    }


    public void PlayDeathSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(DeathSound, player.transform.position);
    }

    public void PlayAttackUpSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(AttackUp, player.transform.position);
    }

    public void PlayAttackMidSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(AttackMid, player.transform.position);
    }

    public void PlayAttackDownSFX(){
        FMODUnity.RuntimeManager.PlayOneShot(AttackDown, player.transform.position);
    }

    public void PlayLevelCompletedSFX(Transform transform){
        FMODUnity.RuntimeManager.PlayOneShot(LevelCompletedSound, transform.position);
    }


}
