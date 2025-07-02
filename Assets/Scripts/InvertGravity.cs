using UnityEngine;
using System.Collections;

public class InvertGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool top;
    private bool isRotating = false;
    public float rotationDuration = 0.5f;

    // Ground check
    public Transform groundCheck;
    public float checkRadius = 0.1f;
    public LayerMask whatIsGround;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Проверяем касание поверхности прямо при попытке инверсии
    public void OnInvertGravityButton()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isGrounded && !isRotating)
        {
            rb.gravityScale *= -1;
            StartCoroutine(RotateSmoothly());
        }
    }

    IEnumerator RotateSmoothly()
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = !top ? Quaternion.Euler(0, 0, 90) : Quaternion.Euler(0, 0, 0);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;
        top = !top;
        isRotating = false;
    }

    // Для визуализации радиуса проверки
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}