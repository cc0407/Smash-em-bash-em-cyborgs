using UnityEngine;

public class AIAnimationController : MonoBehaviour
{
    AIController controller;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<AIController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("CurrentState", (int)controller.currentState.ID);
        anim.SetBool("IsDazed", !controller.IsControllable);
        anim.SetBool("Activated", controller.activated);
        anim.SetBool("IsSupered", controller.IsSupered);
    }
}
