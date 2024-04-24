using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;
    public bool play;

    // Start is called before the first frame update
    void Start()
    {
        play = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(play) 
        {
            source.PlayOneShot(clip);
            Invoke("ChangeScene",1);
        }
        play = false;
        
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene("GameEnd");
    }
}
