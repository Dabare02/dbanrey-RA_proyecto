using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float defaultFallSpeed = 0.5f;  // Velocidad caida lenta controlada
    public float dropFallSpeed = 2.0f;  // Velocidad de caida durante "drop down"
    //public float moveDistance = 2.0f;
    public float joystickMoveSpeed = 1.0f; // Velocidad de movimiento con el joystick
    public float rotateDegrees = 22.5f;

    private Rigidbody _rb;
    private bool isFalling = true;
    private bool isControllable = true;
    private float currentFallSpeed;

    public bool IsFalling
    {
        get { return isFalling; }
    }
    public bool IsControllable
    {
        get { return isControllable; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // Desactivar gravedad para que no se mueva automáticamente
        _rb.useGravity = false;
        // Colisiones continuas para que no atraviese el suelo
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentFallSpeed = defaultFallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            // Movimiento hacia abajo
            transform.Translate(Vector3.down * currentFallSpeed * Time.deltaTime, Space.World);
        }
    }

    // --- FUNCIONES DE CONTROL ---
    // public void MoveLeft()
    // {
    //     if (!isFalling || !isControllable) return;
    //     transform.Translate(Vector3.left * moveDistance * Time.deltaTime, Space.World);
    // }
    // public void MoveRight()
    // {
    //     if (!isFalling || !isControllable) return;
    //     transform.Translate(Vector3.right * moveDistance * Time.deltaTime, Space.World);
    // }
    // public void MoveForward()
    // {
    //     if (!isFalling || !isControllable) return;
    //     transform.Translate(Vector3.forward * moveDistance * Time.deltaTime, Space.World);
    // }
    // public void MoveBackward()
    // {
    //     if (!isFalling || !isControllable) return;
    //     transform.Translate(Vector3.back * moveDistance * Time.deltaTime, Space.World);
    // }
    public void MoveConJoystick(float horizontal, float vertical)
    {
        if (!isFalling || !isControllable) return;
        
        // Creamos un vector de dirección basado en la inclinación del joystick
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        
        // Movemos el bloque de forma fluida
        transform.Translate(direction * joystickMoveSpeed * Time.deltaTime, Space.World);
    }
    public void RotateBlockLeft()
    {
        if (!isFalling || !isControllable) return;
        transform.Rotate(0, -rotateDegrees, 0);
    }
    public void RotateBlockRight()
    {
        if (!isFalling || !isControllable) return;
        transform.Rotate(0, rotateDegrees, 0);
    }
    public void DropDown()
    {
        if (!isFalling || !isControllable) return;
        isControllable = false;
        currentFallSpeed = dropFallSpeed;
    }

    // --- DETECCION ATERRIZAJE ---
    private void OnTriggerEnter(Collider other)
    {
        if (isFalling && (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Landed")))
        {
            LandBlock();
        }
    }
    private void LandBlock()
    {
        isFalling = false;
        currentFallSpeed = defaultFallSpeed;
        gameObject.tag = "Landed";

        // Frenar en seco la inercia acumulada
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.useGravity = true; // Habilitar gravedad para que el bloque pueda caer normalmente
    }
}