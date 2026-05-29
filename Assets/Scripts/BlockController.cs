using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float fallSpeed = 0.5f;  // Velocidad caida lenta controlada
    public float moveDistance = 0.2f;   // Cuando se mueve hacia los lados en cada pulsacion de boton

    private Rigidbody _rb;
    private bool isFalling = true;

    public bool IsFalling
    {
        get { return isFalling; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // Desactivar gravedad para que no se mueva automáticamente
        _rb.useGravity = false;
        // Colisiones continuas para que no atraviese el suelo
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            // Movimiento hacia abajo
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        }
    }

    // --- FUNCIONES DE CONTROL ---
    public void MoveLeft()
    {
        if (!isFalling) return;
        transform.Translate(Vector3.left * moveDistance * Time.deltaTime, Space.World);
    }
    public void MoveRight()
    {
        if (!isFalling) return;
        transform.Translate(Vector3.right * moveDistance * Time.deltaTime, Space.World);
    }
    public void MoveForward()
    {
        if (!isFalling) return;
        transform.Translate(Vector3.forward * moveDistance * Time.deltaTime, Space.World);
    }
    public void MoveBackward()
    {
        if (!isFalling) return;
        transform.Translate(Vector3.back * moveDistance * Time.deltaTime, Space.World);
    }
    public void RotateBlock()
    {
        if (!isFalling) return;
        transform.Rotate(0, 45, 0);
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
        gameObject.tag = "Landed";
        _rb.useGravity = true; // Habilitar gravedad para que el bloque pueda caer normalmente
    }
}
