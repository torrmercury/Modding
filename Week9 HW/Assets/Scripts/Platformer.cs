using UnityEngine;
using System.Collections;

//3D platformer example: Mario 64
//3rd person cam: parent the camera behind the player
public class Platformer : MonoBehaviour
{
    Vector3 inputVector;
    float speed = 1f;
    float turnSpeed = 120f;
    float jumpSpeed = 30f;
    bool grounded = false;
    float movement;
    float turning;
    public AudioClip Collide;
    public AudioClip Jump;

    void Update()
    {
        /*  All of this is the usage of Input.GetKey. Replacing with GetAxis.
        //We're putting the entirity of the player movement input into
        //inputVector before moving it to FixedUpdate (which moves the player)

            //MOVE FORWARD
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += transform.forward;
        }

            //MOVE BACKWARD
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += -transform.forward;
        }
        
            //turning left and right
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        }

        */


        inputVector = Vector3.zero;

        //This is the usage of Input.GetAxis

        if (Input.GetAxis("Vertical") != 0)
        {
            //movement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            //transform.Translate(0, 0, movement);
            inputVector += transform.forward * Input.GetAxis("Vertical") * speed;
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            turning = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            transform.Rotate(0, turning, 0);
        }
        

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            inputVector += Vector3.up * jumpSpeed;
            grounded = false;
            audio.clip = Jump;
            audio.Play();
        }
    }
    
    void FixedUpdate()
    {
        
        //Applying physics
        if (inputVector != Vector3.zero)
        {
            rigidbody.AddForce(inputVector * speed, ForceMode.VelocityChange);
        }

        else
        {
            rigidbody.AddForce(-rigidbody.velocity, ForceMode.Acceleration);
        }
    }

    void OnCollisionEnter()
    {
        grounded = true;
        audio.clip = Collide;
        audio.Play();
        particleSystem.Play();
    }

    void OnTriggerEnter()
    {
        transform.position = new Vector3(0f, 20f, 0f);
    }
}