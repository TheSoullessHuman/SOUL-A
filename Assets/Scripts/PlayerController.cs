using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed;
    public Transform cam;
    public float puntosdevida;
    public float vidaMax;
    public Image Vida;
    public GameManager gameManager;
    


    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public bool groundedPlayer = false;
    public float jumpHeight;
    public float jumpForce = 5;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private Rigidbody MyRb;
    public bool DeadPlayer;

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

        if (direction.magnitude >= 0.1f && DeadPlayer==false)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (puntosdevida <= 0)
        {
            gameManager.Gameover();
            DeadPlayer = true;
            animator.SetBool("Dead", true);

        }
  
        Vector3 floor = transform.TransformDirection(Vector3.down);
        if (Physics.Raycast(transform.position, floor, 1.13f))
        {
            groundedPlayer = true;
            animator.SetBool("Saltar", false);
            
            
        }
        else
        {
            groundedPlayer = false;
            
        }



        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer == true && DeadPlayer==false)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("Saltar", true);
            Debug.Log("Saltando");

            //MyRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            
            
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        TiempoBala velocidad = GetComponent<TiempoBala>();
        speed = velocidad.speedy;

       // TiempoBala gravity = GetComponent<TiempoBala>();
        // gravityValue = gravity.gravity;


        medidor();



    }
    public void medidor()
    {
        Vida.fillAmount = puntosdevida / vidaMax;
        if (puntosdevida >= 25f)
        {
            Vida.color = Color.green;
        }
        else if (puntosdevida <= 25f)
        {
            Vida.color = Color.red;
        }
    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if(puntosdevida > 0)
            {
                puntosdevida = puntosdevida - 10 * Time.deltaTime;
            }  
        }

        if (other.tag == "Curación")
        {
            if (puntosdevida < vidaMax)
            {
                puntosdevida = puntosdevida + 10 * Time.deltaTime;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (puntosdevida > 0)
            {
                puntosdevida = puntosdevida - 10 * Time.deltaTime;
            }
        }

        if (other.tag == "Curación")
        {
            if (puntosdevida < vidaMax)
            {
                puntosdevida = puntosdevida + 10 * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag ("Enemy"))
        {
            if (puntosdevida > 0)
            {
                puntosdevida = puntosdevida - 10 * Time.deltaTime;
            }
        }

        if (collision.collider.CompareTag ("Curación"))
        {
            if (puntosdevida < vidaMax)
            {
                puntosdevida = puntosdevida + 10 * Time.deltaTime;
            }
        }
    }
}
