using UnityEngine;
using System.Collections;
 
public class CharacterControls : MonoBehaviour
{
    public CharacterController characterControler; //Obiekt odpowiedzialny za ruch gracza.
    public Animator animator; //Obiekt odpowiedzialny za animacje modelu gracza.
    public GameObject TppCamera;

    public float Velocity = 2.0f; //Prędkość poruszania się gracza.
    public float JumpHeight = 4.0f; //Wysokość skoku.
    public float ActualHeight = 0f;  //Aktualna wysokosc skoku.
    public float RunningVelocity = 2.0f;   //Predkosc biegania.

    public float SensitivityOfMouse = 3.0f;
    public float MouseUpDown = 0.0f;
    
    public float RangeOfMouseUpDown = 90.0f; //Zakres patrzenia w górę i dół.

    void Start()
    {
        characterControler = GetComponent<CharacterController>();
    }

    void Update()
    {
        Keyboard();
        if (TppCamera.GetComponentInChildren<Camera>().enabled)
        {
            Mouse();
        }
    }

    /**
     * Metoda odpowiedzialna za poruszanie się na klawiaturze.
     */
    private void Keyboard()
    {
        //Jumping
        if (characterControler.isGrounded && Input.GetButton("Jump"))  // Jeżeli znajdujemy się na ziemi i została naciśnięta spacja (skok)
        {
            ActualHeight = JumpHeight;
            animator.SetTrigger("skok");
        }
        else if (!characterControler.isGrounded) //Jezeli jestesmy w powietrzu(skok)
            ActualHeight += Physics.gravity.y * Time.deltaTime; //Fizyka odpowiadająca za grawitacje (os Y).

        //Running
        if (Input.GetKeyDown("left shift"))
            Velocity += RunningVelocity;
        else if (Input.GetKeyUp("left shift"))
            Velocity -= RunningVelocity;    

        Vector3 Move = GetInput(Velocity); //Tworzymy wektor odpowiedzialny za ruch. (lewo/prawo, góra/dół, przód/tył)

        if (!TppCamera.GetComponentInChildren<Camera>().enabled && (Move.x != 0 || Move.z != 0)) //Rotating our character in Tactical Mode
        {
            transform.localRotation = Quaternion.LookRotation(new Vector3(Move.x, 0, Move.z), Vector3.up);
        }
        else Move = transform.rotation * Move;

        ToAnimator(Move);
        characterControler.Move(Move * Time.deltaTime);
    }

    /**
     * Metoda odpowiedzialna za myszkę.
     */
    private void Mouse()
    {
        float MouseLeftRight = Input.GetAxis("Mouse X") * SensitivityOfMouse;
        transform.Rotate(0, MouseLeftRight, 0);

        MouseUpDown -= Input.GetAxis("Mouse Y") * SensitivityOfMouse;
        MouseUpDown = Mathf.Clamp(MouseUpDown, -RangeOfMouseUpDown, RangeOfMouseUpDown);
        TppCamera.transform.localRotation = Quaternion.Euler(MouseUpDown, 0, 0);

        if (Input.GetKeyDown("mouse 0") && characterControler.isGrounded) animator.SetTrigger("atak");

        //Camera.main.transform.localRotation = Quaternion.Euler(myszGoraDol, 0, 0); //Ponieważ CharacterController nie obraca się góra/dół obracamy tylko kamerę.
    }

    private void ToAnimator(Vector3 v)
    {
        int velocity = 0;
        if (v.x != 0 || v.z != 0)
        {
            velocity++;
            if (Input.GetKey("left shift")) velocity = 2;
        }
        animator.SetInteger("velocity", velocity);
    }

    private Vector3 GetInput(float v) //returns the basic values, if it's 0 than it's not active.
    {
        Vector3 p_Velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
            p_Velocity += new Vector3(0, 0, v);
        if (Input.GetKey(KeyCode.S))
            p_Velocity += new Vector3(0, 0, -v);
        if (Input.GetKey(KeyCode.A))
            p_Velocity += new Vector3(-v, 0, 0);
        if (Input.GetKey(KeyCode.D))
            p_Velocity += new Vector3(v, 0, 0);
        p_Velocity += new Vector3(0, ActualHeight, 0);
        return p_Velocity;
    }
}