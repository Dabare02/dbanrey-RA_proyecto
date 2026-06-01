using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    //TODO: Funciones pasua/continuar para cuando se desenfoque el image target

    [Header("Generación")]
    public GameObject[] blockPrefabs;  // Prefabs bloques
    public Transform spawnPoint;    // Punto spawneo bloques

    [Header("Controles")]
    public Joystick movementStick;
    public GameObject normalUI; // El panel normal (joystick y generar bloques)
    public GameObject editUI;   // El panel de edición (flechas, rotación, Done)

    [Header("Variables explosión")]
    public float explosionForce = 700f;
    public float explosionRadius = 10f;
    public float explosionUpward = 3f;  // Modificador para que bloques salgan hacia arriba.
    public AudioClip explosionSFX;
    public GameObject explosionVFX;

    private AudioSource _audioSource;

    private BlockController currentBlock;   // Bloque controlado actualmente
    private bool isEditing = false;   // Indica si estamos en modo edición

    // Devuelve si se puede seleccionar un bloque
    public bool CanSelectBlock()
    {
        if (isEditing) return false;

        return currentBlock == null || !currentBlock.IsFalling;
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Si tenemos un bloque cayendo y controlable, leemos el joystick en cada frame
        if (currentBlock != null && currentBlock.IsFalling && currentBlock.IsControllable)
        {
            float horizontal = movementStick.Horizontal;
            float vertical = movementStick.Vertical;

            // Si el jugador está tocando el joystick, movemos el bloque
            if (horizontal != 0 || vertical != 0)
            {
                currentBlock.MoveConJoystick(horizontal, vertical);
            }
        }
    }

    public void SpawnBlock()
    {
        // Elegimos un bloque aleatorio
        int randIndex = Random.Range(0, blockPrefabs.Length);

        GameObject newBlock = Instantiate(blockPrefabs[randIndex], spawnPoint.position, Quaternion.identity, transform);
        currentBlock = newBlock.GetComponent<BlockController>();
    }
    // Llamado desde block al tocarlo
    public void StartEditingBlock(BlockController blockToEdit)
    {
        isEditing = true;

        currentBlock = blockToEdit;
        currentBlock.EnableEditMode();

        // Cambiar la interfaz
        normalUI.SetActive(false);
        editUI.SetActive(true);

        FreezeAllLandedBlocks(true);
    }
    public void FreezeAllLandedBlocks(bool freeze)
    {
        GameObject[] landedBlocks = GameObject.FindGameObjectsWithTag("Landed");
        foreach (GameObject block in landedBlocks)
        {
            // Ignoramos el bloque que se esta editando
            if (block.GetComponent<BlockController>() != currentBlock)
            {
                Rigidbody rb = block.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = freeze;
            }
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    // --- FUNCIONES EXPLOSION ---
    private IEnumerator ExplodeResetRoutine()
    {
        // Buscar los bloques que forman la estructura
        GameObject[] landBlocks = GameObject.FindGameObjectsWithTag("Landed");

        // Determinar pos explosion
        Vector3 explosionCenter = spawnPoint.position + Vector3.down * 2.0f;

        // Efectos
        if (explosionSFX != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(explosionSFX);
        }
        if (explosionVFX != null)
        {
            GameObject fx = Instantiate(explosionVFX, explosionCenter, Quaternion.identity);
            Destroy(fx, 3.0f); // Destruye el efecto después de 3 segundos
        }

        // Aplicar fisicas explosion
        foreach (GameObject b in landBlocks)
        {
            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Por si estan freeze
                rb.AddExplosionForce(explosionForce, explosionCenter, explosionRadius, explosionUpward);
                rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
            }
        }

        // Eliminar inmediatamente bloque cayendo/editado
        if (currentBlock != null && !currentBlock.CompareTag("Landed"))
        {
            Destroy(currentBlock.gameObject);
            currentBlock = null;
        }

        // Pausar un tiempo
        yield return new WaitForSeconds(3.5f);

        // Eliminar el resto de bloques.
        foreach (GameObject b in landBlocks)
        {
            if (b != null)
            {
                Destroy(b);
            }
        }
        
        isEditing = false;
    }

    // --- FUNCIONES UI ---
    public void OnClickDropDown()
    {
        if (currentBlock != null) currentBlock.DropDown();
    }
    public void OnClickSpawnBlock()
    {
        if (currentBlock == null || !currentBlock.IsFalling) SpawnBlock();
    }
    public void OnClickRotateLeft()
    {
        if (currentBlock != null && !isEditing) currentBlock.RotateBlockLeft();
        else if (isEditing) currentBlock.EditRotateYLeft();
    }
    public void OnClickRotateRight()
    {
        if (currentBlock != null && !isEditing) currentBlock.RotateBlockRight();
        else if (isEditing) currentBlock.EditRotateYRight();
    }

    public void OnClickDoneEditing()
    {
        if (currentBlock != null)
        {
            currentBlock.DisableEditMode();
        }

        // Restaruar la interfaz
        editUI.SetActive(false);
        normalUI.SetActive(true);

        FreezeAllLandedBlocks(false);
        isEditing = false;
    }
    public void OnClickEditLeft()
    {
        if (currentBlock != null) currentBlock.EditMoveLeft();
    }
    public void OnClickEditRight()
    {
        if (currentBlock != null) currentBlock.EditMoveRight();
    }
    public void OnClickEditForward()
    {
        if (currentBlock != null) currentBlock.EditMoveForward();
    }
    public void OnClickEditBackward()
    {
        if (currentBlock != null) currentBlock.EditMoveBackward();
    }
    public void OnClickEditUp()
    {
        if (currentBlock != null) currentBlock.EditMoveUp();
    }
    public void OnClickEditDown()
    {
        if (currentBlock != null) currentBlock.EditMoveDown();
    }
    public void OnClickEditRotateXLeft()
    {
        if (currentBlock != null) currentBlock.EditRotateXLeft();
    }
    public void OnClickEditRotateXRight()
    {
        if (currentBlock != null) currentBlock.EditRotateXRight();
    }
    public void OnClickEditRotateZLeft()
    {
        if (currentBlock != null) currentBlock.EditRotateZLeft();
    }
    public void OnClickEditRotateZRight()
    {
        if (currentBlock != null) currentBlock.EditRotateZRight();
    }

    public void OnClickReset()
    {
        StartCoroutine(ExplodeResetRoutine());
    }
}
