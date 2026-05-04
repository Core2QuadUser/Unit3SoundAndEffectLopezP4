using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool doubleSpeed = false;

    private Animator playerAnim;

    private Rigidbody playerRb;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;

    public ParticleSystem explosionParticle;

    public ParticleSystem dirtParticle;

    public bool gameOver = false;

    public AudioClip jumpSound;
    public AudioClip crashSound;

    private AudioSource playerAudio;
    private AudioSource cameraAudioSource;

    public GameObject camera;

    public bool doubleJumpUsed = false;
    public float doubleJumpForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;

        cameraAudioSource = camera.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            doubleJumpUsed = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && !doubleJumpUsed)
        {
            doubleJumpUsed = true;
            playerRb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
            playerAnim.Play("Running_Jump", 3, 0f);
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            playerAnim.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            playerAnim.SetFloat("Speed_Multiplier", 1.0f);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (gameOver == true)
            {
                return;
            }
            Debug.Log("Game Over!");
            gameOver = true;
            dirtParticle.Stop();
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            cameraAudioSource.Stop();
        }

    }
}
