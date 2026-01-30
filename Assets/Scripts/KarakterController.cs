using UnityEngine;
using UnityEngine.InputSystem;

public class KarakterController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float jumpForce = 10f; // Zıplama gücü

    [Header("Bileşenler")]
    public Rigidbody2D rb;
    public Animator anim;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck; // Ayak ucuna koyacağın boş nesne
    public float checkRadius = 0.2f; // Kontrol dairesinin boyutu
    public LayerMask whatIsGround; // Nelerin "Zemin" olduğunu seçeceğiz

    public bool ayakYerde;

    private float moveInput;
    private bool isGrounded;
    public bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. ZEMİN KONTROLÜ
        // Ayak ucundaki groundCheck noktasında bir daire oluşturup zemin katmanına bakar
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        anim.SetBool("isGrounded", isGrounded);

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // 2. HAREKET VE SHIFT
        moveInput = 0;
        if (keyboard.aKey.isPressed) moveInput = -1;
        if (keyboard.dKey.isPressed) moveInput = 1;

        bool isShiftPressed = keyboard.leftShiftKey.isPressed;
        float currentSpeed = isShiftPressed ? runSpeed : walkSpeed;

        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);

        // 3. ANİMASYONLAR
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
        anim.SetBool("isRunning", isShiftPressed && moveInput != 0);

        if (keyboard.wKey.wasPressedThisFrame && ayakYerde)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
           
        }
        // 5. FLIP
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);

        if (Input.GetKeyDown(KeyCode.W))
        {
            isJumping=true;
            anim.SetTrigger("JumpTrigger");

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ayakYerde = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ayakYerde = false;
        }
    }
}