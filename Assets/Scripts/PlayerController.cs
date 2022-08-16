using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    public Transform cam;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    private bool groundedPlayer = false;
    public float jumpHeight;
    public float jumpForce = 5;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private Rigidbody MyRb;

    public Animator animator;
    public float x, y;





    void Start()
    {
        MyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
  
        Vector3 floor = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, floor, 1.15f))
        {
            groundedPlayer = true;
            print("contacto con el suelo");

        }
        else
        {
            groundedPlayer = false;
            print("no contacto con el suelo");
        }



        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer == true)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            Debug.Log("Saltando");
            //MyRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("no salto");
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Horizontal");

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);





    }


}
