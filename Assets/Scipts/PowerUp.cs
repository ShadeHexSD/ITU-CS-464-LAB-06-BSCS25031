using UnityEngine;

public enum PowerUpType { Speed, Shield }

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpType type = PowerUpType.Speed;
    [SerializeField] float multiplier = 1.6f;
    [SerializeField] float duration = 5f;

    bool collected;

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        if (type == PowerUpType.Speed)
        {
            if (!other.TryGetComponent(out PlayerController movement))
                return;

            movement.BoostSpeed(multiplier, duration);
        }
        else
        {
            if (!other.TryGetComponent(out Health health))
                return;

            health.shielded = true;
        }

        collected = true;
        Destroy(gameObject);
    }
}
