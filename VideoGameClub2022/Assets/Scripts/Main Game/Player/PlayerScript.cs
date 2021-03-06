using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private GameObject camera;
    [Tooltip("How far away the camera is from the center of the bean collider on the Y axis")]
    [SerializeField] private float cameraYOffset;
    [Tooltip("How much the player's head bobs when moving")]
    [SerializeField] private float headBobIntensityY;
    [Tooltip("How fast the player's head bobs")]
    [SerializeField] private float headBobSpeed;
    [Tooltip("How fast the player's head returns to normal position when not moving")]
    [SerializeField] private float headReturnSpeed = 1.5f;

    [Space]

    [Header("Walking Settings")]
    [Tooltip("How fast the player moves")]
    [SerializeField] private float MovementSpeed;
    [Tooltip("How fast the player changes velocity")]
    [SerializeField] private float MovementImpulseSpeed = .02f;

    [Space]

    [Header("Mouse Settings")]
    [Range(1f, 10f)]
    [SerializeField] private float MouseSensitivity = 5;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private float xInput = 0;
    private float zInput = 0;
    private float camX = 0; //location of mouse used to move camera
    private float camY = 0;

    //offset variables for the camera when moving for head-bobbing effect
    private float headBobY;
    private float movementTimer; //used to calculate the sin waves of the head bobbing
    private float headVelocity; //for moving the player head back into place when not moving

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        headBobY = 0;
        movementTimer = 0;
        headVelocity = 0;
    }

    void Update()
    {
        //get user input
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        camX -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        camY += Input.GetAxis("Mouse X") * MouseSensitivity;

        if (camX > 90)
            camX = 90;
        else if (camX < -90)
            camX = -90;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Actually pause everything
            Debug.Log("Game Paused");
            GameObject.Find("Pause").GetComponent<Canvas>().enabled = true;
        }

        //handle camera position
        updateCamera();
        //For object interaction
        objectInteraction();
    }

    void FixedUpdate()
    {
        //move the player
        Vector3 targetVel = (transform.forward * MovementSpeed * zInput) + (transform.right * MovementSpeed * xInput);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref velocity, MovementImpulseSpeed);
        transform.eulerAngles = new Vector3(0, camY, 0);
    }

    private void updateCamera()
    {
        //calculate head bobbing
        if (xInput != 0 || zInput != 0) //if the player is moving
        {
            //if the player is moving, bob the camera
            movementTimer += Time.deltaTime * headBobSpeed; //update movement based off of time
            headBobY = Mathf.Sin(movementTimer) * headBobIntensityY;
        }
        else
        {
            headBobY = Mathf.SmoothDamp(headBobY, 0, ref headVelocity, headReturnSpeed);

            movementTimer = 0;
        }

        camera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset + headBobY, transform.position.z);
        camera.transform.eulerAngles = new Vector3(camX, camY, 0);
    }


    private bool Key;
    private GameObject hitObject;
    [SerializeField]
    private Material highlightShader;
    void objectInteraction()        //Point of this function is to put a highlight around object that you can interact with, and allow interactions
    {
        RaycastHit interact;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out interact, 3))  // if Raycast hits something
        {
            if (interact.collider.tag == "Interaction") //If the object is tagged as an interaction object
            {
                
                hitObject = interact.collider.gameObject;   

                //Switches element 1 in the material array with a highlight
                Material[] change = hitObject.GetComponent<MeshRenderer>().materials;
                change[1] = highlightShader;
                hitObject.GetComponent<MeshRenderer>().materials = change;

                //Enable interaction Option on Interaction Script
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hitObject.GetComponent<InteractionScript>().interactOption();
                }

                Key = true;
            }
            else //Any other object
            {   
                Key = false;
            }
        }
        else if (Key == true)   //If not hitting anything, switches second element to equal the first
        {
            Material[] change = hitObject.GetComponent<MeshRenderer>().materials;
            change[1] = change[0];
            hitObject.GetComponent<MeshRenderer>().materials = change;
            Key = false;
        }

        //Actual Interactions

        

    }
    void OnCollisionEnter(Collision col)
    { 
        /*
         * Stairs are currently just an invisible ramp above the actual stairs, 
         * this is because programming stairs is hard and this is an easy fix.
         * However, character moves slower when going up a ramp, so increase
         * player's speed a bit when going up stairs
         */

        if (col.gameObject.tag.Equals("Stairs")){
            MovementSpeed *= 1.5f;
        } 
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag.Equals("Stairs"))
        {
            MovementSpeed /= 1.5f;
        }
    }
}