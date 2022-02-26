using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    

    void Start()
    {
        rb=GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();


    }

   
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        
    }
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up*mainThrust*Time.deltaTime);//relative olmasýnýn nedeni nesnemiz rotate olsa bile olmus haline göre güç uygular eksenlere baðlý güç.Vector(0,1,0) ile vector3.up ayný þey
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);// movement oldugunda motor sesi çýkaralým.
            }
            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    void ProcessRotation()
    {

        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
            if (!rightThrusterParticles.isPlaying)
            {
                rightThrusterParticles.Play();
            }

        }
       else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
            if (!leftThrusterParticles.isPlaying)
            {
                leftThrusterParticles.Play();
            }
            }
        else
        {
            rightThrusterParticles.Stop();
            leftThrusterParticles.Stop();
        }
    }
   public void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation= true;// rotasyonu donduruyoruzki manuel olarak rotasyon ettirebilelim.
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;// rotasyonu fizik sistemi kullanabilmesi için geri açtýk.
    }
}
