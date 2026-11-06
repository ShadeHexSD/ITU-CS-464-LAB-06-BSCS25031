using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerController target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -7f);
    [SerializeField, Min(0.01f)] private float sharpness = 5f;
    [SerializeField, Min(0f)] private float lookAhead = 0.35f;
    [SerializeField] private float lookHeight = 1f;

    private void Awake()
    {
        if (target == null)
            target = FindAnyObjectByType<PlayerController>();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            target = FindAnyObjectByType<PlayerController>();
            if (target == null) return;
        }

        Rigidbody body = target.PlayerRigidbody;
        if (body == null) return;

#if UNITY_6000_0_OR_NEWER
        Vector3 flatVelocity = body.linearVelocity;
#else
        Vector3 flatVelocity = body.velocity;
#endif
        flatVelocity.y = 0f;

        Vector3 aim = target.transform.position
                      + Vector3.up * lookHeight
                      + flatVelocity * lookAhead;
        Vector3 wanted = aim + offset;

        float blend = 1f - Mathf.Exp(-sharpness * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, wanted, blend);
        transform.LookAt(aim);
    }
}
