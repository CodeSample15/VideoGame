using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    [Space]

    [Tooltip("How fast the player moves")]
    [SerializeField] private float MovementSpeed;
    [Tooltip("How far away the camera is from the center of the bean collider on the Y axis")]
    [SerializeField] private float cameraYOffset;

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private float xInput = 0;
    private float zInput = 0;
    private float camX = 0; //location of mouse used to move camera
    private float camY = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //get user input
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        camX -= Input.GetAxis("Mouse Y");
        camY += Input.GetAxis("Mouse X");

        if (camX > 90)
            camX = 90;
        else if (camX < -90)
            camX = -90;
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

        camera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
        camera.transform.eulerAngles = new Vector3(camX, camY, 0);
    }
}
