using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal connectedPortal;
    public float cooldown = 5f;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;

        if (other.CompareTag("Player"))
        {
            Teleport(other.transform);
        }
    }

    void Teleport(Transform player)
    {
        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        player.position = connectedPortal.transform.position + connectedPortal.transform.forward * 2f;

        if (controller != null)
        {
            controller.enabled = true;
        }

        canTeleport = false;
        connectedPortal.canTeleport = false;

        Invoke(nameof(ResetTeleport), cooldown);
        connectedPortal.Invoke(nameof(ResetTeleport), cooldown);
    }


    void ResetTeleport()
    {
        canTeleport = true;
    }
}
