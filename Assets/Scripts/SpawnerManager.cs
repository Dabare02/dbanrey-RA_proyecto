using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject blockPrefab;  // Prefab bloques
    public Transform spawnPoint;    // Punto spawneo bloques

    private BlockController currentBlock;   // Bloque controlado actualmente

    public void SpawnBlock()
    {
        if (currentBlock == null || !currentBlock.IsFalling)
        {
            GameObject newBlock = Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
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
    public void OnClickRotate()
    {
        if (currentBlock != null) currentBlock.RotateBlock();
    }
}
