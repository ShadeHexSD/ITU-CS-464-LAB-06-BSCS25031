using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 90f;

    bool collected;

    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.CompareTag("Player") || GameManager.Instance == null)
            return;

        collected = true;
        GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}
