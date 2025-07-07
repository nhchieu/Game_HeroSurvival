using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isBlocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
            isBlocked = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger)
            isBlocked = false;
    }
}