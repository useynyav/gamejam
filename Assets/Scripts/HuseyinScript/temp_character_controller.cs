using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input System paketi için gerekli

[RequireComponent(typeof(Rigidbody2D))] // Rigidbody2D yoksa otomatik ekler
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;

    [Header("Zemin Kontrolü")]
    public string groundTag = "Ground";

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        // Rigidbody referansını alıyoruz
        rb = GetComponent<Rigidbody2D>();

        // Karakterin havada takla atmaması için rotasyonu donduruyoruz
        rb.freezeRotation = true;
    }

    // Yeni Input System'den gelen Hareket mesajı (A-D veya Sol-Sağ ok tuşları)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<float>();
    }

    // Yeni Input System'den gelen Zıplama mesajı (Space tuşu)
    public void OnJump(InputValue value)
    {
        // Sadece tuşa basıldığında ve yerdeyken zıpla
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Update()
    {
        // Yatay hızı uygula (Dikey hızı yani yerçekimini koruyarak)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Karakterin baktığı yönü değiştirme (Görsel flip)
        FlipCharacter();
    }

    private void FlipCharacter()
    {
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // Zemin Kontrolü (Collision tabanlı)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }
}