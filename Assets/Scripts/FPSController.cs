using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    // references
    CharacterController controller;
    [SerializeField] GameObject cam;
    [SerializeField] Transform gunHold;
    [SerializeField] Gun initialGun;

    // stats
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float lookSensitivityX = 1.0f;
    [SerializeField] float lookSensitivityY = 1.0f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;

    // private variables
    Vector3 origin;
    Vector3 velocity;
    bool grounded;
    float xRotation;
    List<Gun> equippedGuns = new List<Gun>();
    int gunIndex = 0;
    Gun currentGun = null;
    Vector2 m_move;
    Vector2 lookInput;
    bool jump = false;
    bool m_sprint = false;

    // properties
    public GameObject Cam { get { return cam; } }
    

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // start with a gun
        if(initialGun != null)
            AddGun(initialGun);

        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Look();

        // always go back to "no velocity"
        // "velocity" is for movement speed that we gain in addition to our movement (falling, knockback, etc.)
        Vector3 noVelocity = new Vector3(0, velocity.y, 0);
        velocity = Vector3.Lerp(velocity, noVelocity, 5 * Time.deltaTime);
    }

    void Movement()
    {
        grounded = controller.isGrounded;

        if(grounded && velocity.y < 0)
        {
            velocity.y = -1;
        }

        Vector3 move = transform.right * m_move.x + transform.forward * m_move.y;
        controller.Move(move * movementSpeed * (GetSprint() ? 2 : 1) * Time.deltaTime);

        if (jump && grounded)
        {
            velocity.y += Mathf.Sqrt (jumpForce * -1 * gravity);
            jump = false;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        float lookX = lookInput.x * lookSensitivityX * Time.deltaTime;
        float lookY = lookInput.y * lookSensitivityY * Time.deltaTime;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookX);
    }

    void OnWeaponSwap(InputValue v)
    {
        if (equippedGuns.Count == 0)
            return;

        if(v.Get<float>() > 0)
        {
            gunIndex++;
            if (gunIndex > equippedGuns.Count - 1)
                gunIndex = 0;

            EquipGun(equippedGuns[gunIndex]);
        }

        else if (v.Get<float>() < 0)
        {
            gunIndex--;
            if (gunIndex < 0)
                gunIndex = equippedGuns.Count - 1;

            EquipGun(equippedGuns[gunIndex]);
        }
    }


    public void OnFire()
    {
        if (currentGun == null)
            return;

        currentGun?.AttemptFire();
    }


    public void OnMove(InputValue v)
    {
        m_move = v.Get<Vector2>();
    }

    public void OnJump()
    {
        if (grounded)
        {
            jump = true;
        }
    }

    public void OnSprint(InputValue v)
    {
        m_sprint = v.isPressed;
    }

    public void OnLook(InputValue v)
    {
        lookInput = v.Get<Vector2>();
    }

    

    void EquipGun(Gun g)
    {
        // disable current gun, if there is one
        currentGun?.Unequip();
        currentGun?.gameObject.SetActive(false);

        // enable the new gun
        g.gameObject.SetActive(true);
        g.transform.parent = gunHold;
        g.transform.localPosition = Vector3.zero;
        currentGun = g;

        g.Equip(this);
    }

    // public methods

    public void AddGun(Gun g)
    {
        // add new gun to the list
        equippedGuns.Add(g);

        // our index is the last one/new one
        gunIndex = equippedGuns.Count - 1;

        // put gun in the right place
        EquipGun(g);
    }

    public void RemoveGun(Gun g)
    {
        currentGun.Unequip();
        currentGun = null;
        equippedGuns.Remove(g);
        Destroy(g.gameObject);
    }

    public void IncreaseAmmo(int amount)
    {
        currentGun.AddAmmo(amount);
    }

    public void Respawn()
    {
        transform.position = origin;
    }

    // Input methods

    bool GetSprint()
    {
        return m_sprint;
    }

    // Collision methods

    // Character Controller can't use OnCollisionEnter :D thanks Unity
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<Damager>())
        {
            var collisionPoint = hit.collider.ClosestPoint(transform.position);
            var knockbackAngle = (transform.position - collisionPoint).normalized;
            velocity = (20 * knockbackAngle);
        }

        if (hit.gameObject.GetComponent <KillZone>())
        {
            Respawn();
        }
    }


}
