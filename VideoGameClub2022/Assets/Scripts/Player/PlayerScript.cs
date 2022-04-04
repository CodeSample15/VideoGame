using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [Tooltip("How much the player's head bobs when moving")]
    [SerializeField] private float headBobIntensityX;
    [Tooltip("How much the player's head bobs when moving")]
    [SerializeField] private float headBobIntensityY;

    [Space]

    [Tooltip("How fast the player moves")]
    [SerializeField] private float MovementSpeed;
    [Tooltip("How far away the camera is from the center of the bean collider on the Y axis")]
    [SerializeField] private float cameraYOffset;

    [Space]

    [Range(1f, 10f)]
    [SerializeField] private float MouseSensitivity = 5;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private float xInput = 0;
    private float zInput = 0;
    private float camX = 0; //location of mouse used to move camera
    private float camY = 0;

    //offset variables for the camera when 
    private float headBobX;
    private float headBobY;
    private float movementTimer; //used to calculate the sin waves of the head bobbing

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //get user input
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        camX -= Input.GetAxis("Mouse Y") * MouseSensitivity;
        camY += Input.GetAxis("Mouse X") * MouseSensitivity;

        headBobX = 0;
        headBobY = 0;
        movementTimer = 0;

        if (camX > 90)
            camX = 90;
        else if (camX < -90)
            camX = -90;

        //handle camera position
        updateCamera();
        //For object interaction
        objectInteraction();
    }

    private void updateCamera()
    {
        //calculate head bobbing
        if (xInput != 0 || zInput != 0)
        {
            //if the player is moving, bob the camera
            movementTimer += Time.deltaTime; //update movement based off of time
            //Debug.Log(Mathf.Sin(movementTimer));
            headBobY = Mathf.Sin(movementTimer) * headBobIntensityY;
        }
        else
        {
            headBobY = 0;
            movementTimer = 0;
        }

        camera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
        camera.transform.eulerAngles = new Vector3(camX, camY, 0);

        //head bobbing
        Vector3 cameraPos = camera.transform.localPosition;
        camera.transform.localPosition = new Vector3(cameraPos.x, cameraPos.y + headBobY, cameraPos.z);
    }

    void FixedUpdate()
    {
        //move the player + camera

        //put the camera code in FixedUpdate to prevent moving the camera multiple times per loop in LateUpdate
        //safer to put in LateUpdate if the movement code is put somewhere else other than the same FixedUpdate loop
        //I'm putting it in FixedUpdate anyway since I can control when the camera position is updated and it will
        //save a little bit of processing time (not a lot, but idk it might come in handy later)

        Vector3 targetVel = (transform.forward * MovementSpeed * zInput) + (transform.right * MovementSpeed * xInput);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVel, ref velocity, .02f);
        transform.eulerAngles = new Vector3(0, camY, 0);
    }

    private bool Key;
    private GameObject hitObject;
    void objectInteraction()        //Point of this function is to put a highlight around object that you can interact with, and allow interactions
    {

        RaycastHit interact;
        Physics.Raycast(camera.transform.position, camera.transform.forward, out interact, 3);    //3 is how far the player can interact with objects
        

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out interact, 3))  // if Raycast hits something
        {
            if (interact.collider.tag == "Interaction") //If the object is tagged as an interaction object
            {
                hitObject = interact.collider.gameObject;    
                hitObject.GetComponent<MeshRenderer>().material.color = Color.white;    //Sets the color of object to white, (later change this to a highlight around object)
                Key = true;
            }
            else //Any other object
            {
                hitObject.GetComponent<MeshRenderer>().material.color = Color.grey; //Return to original color (with the highlights, just disable the highlight)
                Key = false;
            }
        }
        else if (Key == true)   //If not hitting anything, return to normal color
        {
            hitObject.GetComponent<MeshRenderer>().material.color = Color.grey; //Return to original color (with the highlights, just disable the highlight)
            Key = false;
        }

        //IMPLEMENT ACTUAL INTERACTIONS

    }
}