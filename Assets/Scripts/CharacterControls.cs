using UnityEngine;
using System.Collections;
 
public class CharacterControls : MonoBehaviour
{
    public CharacterController characterControler; //Obiekt odpowiedzialny za ruch gracza.
    public Animator animator; //Obiekt odpowiedzialny za animacje modelu gracza.
    public GameObject TppCamera;

    public float predkoscPoruszania = 9.0f; //Prędkość poruszania się gracza.
    public float wysokoscSkoku = 2.0f; //Wysokość skoku.
    public float aktualnaWysokoscSkoku = 0f;  //Aktualna wysokosc skoku.
    public float predkoscBiegania = 7.0f;   //Predkosc biegania.

    //Sensitivity
    public float czuloscMyszki = 3.0f;
    public float myszGoraDol = 0.0f;
    
    public float zakresMyszyGoraDol = 90.0f; //Zakres patrzenia w górę i dół.
    public float predkosc = 0; //Szybkość poruszania (bezczynność, chód, bieg)


    void Start()
    {
        characterControler = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(TppCamera.GetComponentInChildren<Camera>().enabled)
        {
            klawiatura();
            myszka();
        }
    }

    /**
     * Metoda odpowiedzialna za poruszanie się na klawiaturze.
     */
    private void klawiatura()
    {
        predkosc = 0;
        float rochPrzodTyl = Input.GetAxis("Vertical") * predkoscPoruszania; //Pobranie prędkości poruszania się przód/tył. Jeżeli wartość dodatnia to poruszamy się do przodu, a jeżeli wartość ujemna to poruszamy się do tyłu.
        float rochLewoPrawo = Input.GetAxis("Horizontal") * predkoscPoruszania; //Pobranie prędkości poruszania się lewo/prawo. Jeżeli wartość dodatnia to poruszamy się w prawo, jeżeli wartość ujemna to poruszamy się w lewo.

        if (Input.GetAxis("Vertical") != 0) predkosc = 2;

        //Skakanie
        if (characterControler.isGrounded && Input.GetButton("Jump"))  // Jeżeli znajdujemy się na ziemi i została naciśnięta spacja (skok)
        {
            //aktualnaWysokoscSkoku = wysokoscSkoku;
            animator.SetTrigger("skok");
        }
        else if (!characterControler.isGrounded) //Jezeli jestesmy w powietrzu(skok)
            aktualnaWysokoscSkoku += Physics.gravity.y * Time.deltaTime; //Fizyka odpowiadająca za grawitacje (os Y).

        //Bieganie
        if (Input.GetKey("left shift")) predkosc = 5;
        if (Input.GetKeyDown("left shift"))
            predkoscPoruszania += predkoscBiegania;
        else if (Input.GetKeyUp("left shift"))
            predkoscPoruszania -= predkoscBiegania;

        animator.SetFloat("predkosc", predkosc);

        Vector3 ruch = new Vector3(rochLewoPrawo, aktualnaWysokoscSkoku, rochPrzodTyl); //Tworzymy wektor odpowiedzialny za ruch. (lewo/prawo, góra/dół, przód/tył)
        
        ruch = transform.rotation * ruch;  //Aktualny obrót gracza razy kierunek w którym sie poruszamy (poprawka na obrót myszką abyśmy szli w kierunku w którym patrzymy).

        characterControler.Move(ruch * Time.deltaTime);
    }

    /**
     * Metoda odpowiedzialna za ruch myszką.
     */
    private void myszka()
    {
        float myszLewoPrawo = Input.GetAxis("Mouse X") * czuloscMyszki; //Pobranie wartości ruchu myszki lewo/prawo. Jeżeli wartość dodatnia to poruszamy w prawo, a jeżeli wartość ujemna to poruszamy w lewo.
        transform.Rotate(0, myszLewoPrawo, 0);

        myszGoraDol -= Input.GetAxis("Mouse Y") * czuloscMyszki; //Pobranie wartości ruchu myszki góra/dół. Jeżeli wartość dodatnia to poruszamy w górę, a jeżeli wartość ujemna to poruszamy w dół.

        myszGoraDol = Mathf.Clamp(myszGoraDol, -zakresMyszyGoraDol, zakresMyszyGoraDol); //Funkcja nie pozwala aby wartość przekroczyła dane zakresy.

        if (Input.GetKey("mouse 0") && characterControler.isGrounded) animator.SetTrigger("atak");

        //Camera.main.transform.localRotation = Quaternion.Euler(myszGoraDol, 0, 0); //Ponieważ CharacterController nie obraca się góra/dół obracamy tylko kamerę.
        TppCamera.transform.localRotation = Quaternion.Euler(myszGoraDol, 0, 0);
    }

}