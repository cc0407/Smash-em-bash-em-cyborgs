using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    PlayerController controller;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsFullBattery", controller.status.IsFullBattery());
        animator.SetBool("IsDazed", !controller.IsControllable); // This gets set to false inside the dazed coroutine, then sets back to true after some time. Used to stun the player in place
        if (!controller.IsControllable) // Don't queue any actions if the player is dazed
        {
            ResetAllTriggers();
        }
        animator.SetBool("IsLowBattery", controller.status.IsLowBattery());
        animator.SetBool("Activated", controller.activated);

        if (Input.GetButtonDown("Block Left"))
        {
            ResetAllTriggers();
            animator.SetTrigger("BlockLeft");
        }

        if (Input.GetButtonDown("Block Right"))
        {
            ResetAllTriggers();
            animator.SetTrigger("BlockRight");
        }

        if (Input.GetButtonDown("Attack Left"))
        {
            ResetAllTriggers();
            animator.SetTrigger("AttackLeft");
        }

        if (Input.GetButtonDown("Attack Right"))
        {
            ResetAllTriggers();
            animator.SetTrigger("AttackRight");
        }

        if (Input.GetButtonDown("Super") && controller.status.CanSuper())
        {
            ResetAllTriggers();
            animator.SetTrigger("Super");
        }

        if (Input.GetButtonDown("Retreat"))
        {
            ResetAllTriggers();
            animator.SetTrigger("Retreat");
        }

        if (Input.GetButtonDown("Advance"))
        {
            ResetAllTriggers();
            animator.SetTrigger("Advance");
        }

    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("AttackLeft");
        animator.ResetTrigger("AttackRight");
        animator.ResetTrigger("BlockLeft");
        animator.ResetTrigger("BlockRight");
        animator.ResetTrigger("Retreat");
        animator.ResetTrigger("Advance");
        animator.ResetTrigger("Super");
    }
}
