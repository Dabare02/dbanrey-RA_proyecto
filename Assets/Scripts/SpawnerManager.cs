using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Generación")]
    public GameObject[] blockPrefabs;  // Prefabs bloques
    public Transform spawnPoint;    // Punto spawneo bloques

    [Header("Controles")]
    public Joystick movementStick;

    private BlockController currentBlock;   // Bloque controlado actualmente

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
        // Comprobamos si podemos spawnear
        if (currentBlock == null || !currentBlock.IsFalling)
        {
            // Elegimos un bloque aleatorio
            int randIndex = Random.Range(0, blockPrefabs.Length);

            GameObject newBlock = Instantiate(blockPrefabs[randIndex], spawnPoint.position, Quaternion.identity);
            currentBlock = newBlock.GetComponent<BlockController>();
        }
    }

    // --- FUNCIONES UI ---
    // public void OnClickLeft()
    // {
    //     if (currentBlock != null) currentBlock.MoveLeft();
    // }
    // public void OnClickRight()
    // {
    //     if (currentBlock != null) currentBlock.MoveRight();
    // }
    // public void OnClickForward()
    // {
    //     if (currentBlock != null) currentBlock.MoveForward();
    // }
    // public void OnClickBackward()
    // {
    //     if (currentBlock != null) currentBlock.MoveBackward();
    // }
    public void OnClickRotateLeft()
    {
        if (currentBlock != null) currentBlock.RotateBlockLeft();
    }
    public void OnClickRotateRight()
    {
        if (currentBlock != null) currentBlock.RotateBlockRight();
    }
    public void OnClickDropDown()
    {
        if (currentBlock != null) currentBlock.DropDown();
    }
}
