using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioHandler : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;
    public bool play;
    public CameraShake cameraShake;
    [SerializeField] float magnitude;
    [SerializeField] float volume;
    

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
            source.volume = volume;
            source.PlayOneShot(clip);
            StartCoroutine(cameraShake.Shake(1.4f,magnitude));
            Invoke("ChangeScene",1);
        }
        play = false;
        
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene("GameEnd");
    }

    

    
}
