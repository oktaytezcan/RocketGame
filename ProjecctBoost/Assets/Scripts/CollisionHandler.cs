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
   void  RespondToDebugKeys()// kullanýcý için l tuþuna basýnca next levela geçmek için bir trick olusturduk.
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;// toggle kullanýp kullanýcý için collision compponentini c ye basýnca etkisiz hale getirdik.
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
    void StartSuccessSequence()// leveli geçtigimizde yeni level için gecikme ve roket için hareket kýsýtlamasý yaptýk.Sesi level atladýktan sonra kapattýk
    {
         isTransitioning = true;
        audioSource.Stop();
        
        audioSource.PlayOneShot(success);

        successParticles.Play();
        
        GetComponent<Movement>().enabled = false;
       
        Invoke("LoadNextLevel", LevelLoadDelay);


    }
    void StartCrashSequence()// çarptýgýmýzda roketin kontrol edilmemesi için.Sesi çarpýþmadan sonra kapttýk.
    {
         isTransitioning = true;
        audioSource.Stop();
        
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;

        Invoke("ReloadLevel",LevelLoadDelay);// levelin geri yüklenmesi için gecikme

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

