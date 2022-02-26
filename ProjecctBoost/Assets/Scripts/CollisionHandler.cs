using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    
    AudioSource audioSource;
    
    bool isTransitioning= false;
    bool collisionDisable = false;
     void Update()
    {
        RespondToDebugKeys();
    }
   void  RespondToDebugKeys()// kullan�c� i�in l tu�una bas�nca next levela ge�mek i�in bir trick olusturduk.
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;// toggle kullan�p kullan�c� i�in collision compponentini c ye bas�nca etkisiz hale getirdik.
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisable ) { return; }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
               StartSuccessSequence();
                break;
            
          

            default:
                StartCrashSequence();
                break;


        }
    }
    void StartSuccessSequence()// leveli ge�tigimizde yeni level i�in gecikme ve roket i�in hareket k�s�tlamas� yapt�k.Sesi level atlad�ktan sonra kapatt�k
    {
         isTransitioning = true;
        audioSource.Stop();
        
        audioSource.PlayOneShot(success);

        successParticles.Play();
        
        GetComponent<Movement>().enabled = false;
       
        Invoke("LoadNextLevel", LevelLoadDelay);


    }
    void StartCrashSequence()// �arpt�g�m�zda roketin kontrol edilmemesi i�in.Sesi �arp��madan sonra kaptt�k.
    {
         isTransitioning = true;
        audioSource.Stop();
        
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;

        Invoke("ReloadLevel",LevelLoadDelay);// levelin geri y�klenmesi i�in gecikme

    }
    void LoadNextLevel()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
}

