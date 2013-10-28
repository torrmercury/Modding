using UnityEngine;
using System.Collections;

//3D platformer example: Mario 64
//3rd person cam: parent the camera behind the player
public class Platformer : MonoBehaviour
{
    Vector3 inputVector;
    float speed = 5f;
    float turnSpeed = 120f;
    float jumpSpeed = 2f;
    float fallSpeed = 6f;
    bool grounded = false;
    float movement;
    float turning;

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
            movement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.Translate(0, 0, movement);
        }

        turning = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        //Jumping
        if (Physics.Raycast(transform.position, -transform.up, 1.3f) == true)
        {
            grounded = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inputVector += Vector3.up * jumpSpeed;
            }
        }
        else
        {
            grounded = false;
        }


        //Debug.Log(Input.GetAxis("Vertical"));
        //Debug.Log(movement);

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
            rigidbody.AddForce(-rigidbody.velocity + Physics.gravity * fallSpeed, ForceMode.Acceleration);
        }
        

        //Usage of GetAxis turning
        transform.Rotate(0, turning, 0);
        
        //movement = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        
    }
}