using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Generación")]
    public GameObject[] blockPrefabs;  // Prefabs bloques
    public Transform spawnPoint;    // Punto spawneo bloques

    private BlockController currentBlock;   // Bloque controlado actualmente

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
    public void OnClickLeft()
    {
        if (currentBlock != null) currentBlock.MoveLeft();
    }
    public void OnClickRight()
    {
        if (currentBlock != null) currentBlock.MoveRight();
    }
    public void OnClickForward()
    {
        if (currentBlock != null) currentBlock.MoveForward();
    }
    public void OnClickBackward()
    {
        if (currentBlock != null) currentBlock.MoveBackward();
    }
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
