using UnityEngine;

public class BlockController : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float defaultFallSpeed = 0.5f;  // Velocidad caida lenta controlada
    public float dropFallSpeed = 2.0f;  // Velocidad de caida durante "drop down"
    public float moveDistance = 2.0f;   // Mov en modo edición
    public float joystickMoveSpeed = 1.0f; // Velocidad de movimiento con el joystick
    public float rotateDegrees = 22.5f;

    [Header("Edición")]
    public Collider physCollider;

    private Rigidbody _rb;
    private MeshRenderer _renderer;
    private bool isFalling = true;
    private bool isControllable = true;
    private float currentFallSpeed;

    private Collider floorCollider;

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

        _renderer = GetComponent<MeshRenderer>();

        currentFallSpeed = defaultFallSpeed;
        
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        if (floor != null)
        {
            floorCollider = floor.GetComponent<Collider>();
        }
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

    // --- FUNCIONES DE CONTROL ---
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

    public void EditMoveLeft() { if (isControllable) transform.Translate(Vector3.left * moveDistance * Time.deltaTime, Space.World); }
    public void EditMoveRight() { if (isControllable) transform.Translate(Vector3.right * moveDistance * Time.deltaTime, Space.World); }
    public void EditMoveForward() { if (isControllable) transform.Translate(Vector3.forward * moveDistance * Time.deltaTime, Space.World); }
    public void EditMoveBackward() { if (isControllable) transform.Translate(Vector3.back * moveDistance * Time.deltaTime, Space.World); }
    public void EditMoveUp() { if (isControllable) transform.Translate(Vector3.up * moveDistance * Time.deltaTime, Space.World); }
    public void EditMoveDown() {
        if (isControllable)
        {
            Vector3 downMovement = Vector3.down * moveDistance * Time.deltaTime;
            transform.Translate(downMovement, Space.World);

            if (floorCollider != null && physCollider != null)
            {
                float lowestBlockPoint = physCollider.bounds.min.y;
                float floorSurface = floorCollider.bounds.max.y;

                if (lowestBlockPoint < floorSurface)
                {
                    float penetrationDepth = floorSurface - lowestBlockPoint;
                    transform.position -= downMovement;
                }
            }
        }
    }
    public void EditRotateYLeft() { if (isControllable) transform.Rotate(0, -rotateDegrees, 0); }
    public void EditRotateYRight() { if (isControllable) transform.Rotate(0, rotateDegrees, 0); }
    public void EditRotateXLeft() { if (isControllable) transform.Rotate(-rotateDegrees, 0, 0); }
    public void EditRotateXRight() { if (isControllable) transform.Rotate(rotateDegrees, 0, 0); }
    public void EditRotateZLeft() { if (isControllable) transform.Rotate(0, 0, -rotateDegrees); }
    public void EditRotateZRight() { if (isControllable) transform.Rotate(0, 0, rotateDegrees); }

    // --- FUNCIONES ESTADO EDICIÓN ---
    public void EnableEditMode()
    {
        isControllable = true;
        GetComponent<Outline>().enabled = true;

        // Congelamos sus físicas y desactivamos colisiones.
        _rb.isKinematic = true;
        //physCollider.isTrigger = true;
    }
    public void DisableEditMode()
    {
        isControllable = false;
        GetComponent<Outline>().enabled = false;

        // Descongelar y devolver colisiones
        _rb.isKinematic = false;
        //physCollider.isTrigger = false;
    }

    // --- EVENTOS ---
    private void OnTriggerEnter(Collider other)
    {
        if (isFalling && (other.gameObject.CompareTag("Floor") || other.gameObject.CompareTag("Landed")))
        {
            LandBlock();
        }
    }

    private void OnMouseDown()
    {
        // Buscamos el spawner
        SpawnerManager manager = FindFirstObjectByType<SpawnerManager>();

        // Solo se puede seleccionar si esta landed y no hay otro bloque cayendo.
        if (gameObject.CompareTag("Landed") && manager.CanSelectBlock())
        {
            manager.StartEditingBlock(this);
        }
    }
}