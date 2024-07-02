using UnityEngine;

public class BeDazed : StateMachineBehaviour
{
    public GameObject star;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 startPos = new Vector3(animator.rootPosition.x, 1.2f, animator.rootPosition.z);
        Destroy(Instantiate(star, startPos, Quaternion.identity), .5f);
    }
}
