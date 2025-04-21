using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Switches between idle and walking animations based on rigidbody movement
    void Update()
    {
        CheckForMovement();
        AnimationTestFunction();
    }
    void CheckForMovement()
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }
    // Call after the player takes a non-purity pact to change sprite permanantly
    public void SetPlayerCorruptSprite()
    {
        animator.SetBool("Corrupt", true);
    }
    // Call after taking damage to play hurt animation
    public void PlayPlayerHurtAnim()
    {
        animator.SetTrigger("Hurt");
    }
    // Call after taking lethal damage to play death animation
    public void PlayPlayerDeadAnim()
    {
        animator.SetTrigger("Dead");
    }
    void AnimationTestFunction()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetPlayerCorruptSprite();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayPlayerHurtAnim();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayPlayerDeadAnim();
        }
    }
}
