using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer(other);
        }
    }

    private void KillPlayer(Collider2D player)
    {
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            respawn.Die();
        }
    }
}