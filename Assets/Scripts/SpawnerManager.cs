using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    //TODO: Funciones pasua/continuar para cuando se desenfoque el image target
    //TODO: Añadir drop shadow a bloques

    [Header("Generación")]
    public GameObject[] blockPrefabs;  // Prefabs bloques
    public Transform spawnPoint;    // Punto spawneo bloques

    [Header("Controles")]
    public Joystick movementStick;
    public GameObject normalUI; // El panel normal (joystick y generar bloques)
    public GameObject editUI;   // El panel de edición (flechas, rotación, Done)

    private BlockController currentBlock;   // Bloque controlado actualmente

    // Devuelve si se puede seleccionar un bloque
    public bool CanSelectBlock()
    {
        return currentBlock == null || !currentBlock.IsFalling;
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
        if (currentBlock != null) currentBlock.RotateBlockLeft();
    }
    public void OnClickRotateRight()
    {
        if (currentBlock != null) currentBlock.RotateBlockRight();
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
        if (currentBlock != null) currentBlock.EditRotateYLeft();
    }
    public void OnClickEditRotateZRight()
    {
        if (currentBlock != null) currentBlock.EditRotateYRight();
    }
}
