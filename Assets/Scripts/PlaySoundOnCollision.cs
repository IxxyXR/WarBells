using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour{

    AudioSource myaudio;
    public RipplePulse ripple;

    void Start() {
        myaudio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){

        if (other.gameObject.tag == "playsound" )
        {
            myaudio.volume = 0.304f;
            myaudio.Play();
            ripple.playRipples();
        }
    }
}