using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour{

    AudioSource myaudio;

    void Start() {

        myaudio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){

        if (other.gameObject.tag == "playsound" ){
            myaudio.Play();
            Debug.Log("How do you enjoy the sound?");
        }
    }
}