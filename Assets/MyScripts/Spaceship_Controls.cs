using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Controls : MonoBehaviour
{   
    public Gameplay gameplay;
    public Rigidbody2D rb;
    public float thrust;
    public float turnThrust;

    public float screenTOP;
    public float screenBOTTOM;
    public float screenLEFT;
    public float screenRIGHT;

    private float thrustInput;
    private float turnInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checking input from keyboard.
        // These are WASD and ARROW KEYS.
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        //Shoot bullets

        //Wrapping the ship around the screen
        Vector3 newPos = transform.position;

        if(transform.position.y > screenTOP){
            newPos.y = screenBOTTOM;
        }
        if(transform.position.y < screenBOTTOM){
            newPos.y = screenTOP;
        }
        if(transform.position.x < screenRIGHT){
            newPos.x = screenLEFT;
        }
        if(transform.position.x > screenLEFT){
            newPos.x = screenRIGHT;
        }

        newPos.z = -1;
        transform.position = newPos;

    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustInput * thrust);
        rb.rotation += (-turnInput * turnThrust);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        gameplay.RocketFail();
    }
}
