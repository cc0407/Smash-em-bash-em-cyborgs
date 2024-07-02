using System.Collections;
using UnityEngine;

public abstract class BotController : MonoBehaviour
{
    public Animator anim { get; protected set; }
    public CharacterController controller { get; protected set; }
    public Vector3 startPos { get; protected set; }

    public bool activated { get; protected set; } = false;
    public bool IsControllable { get; protected set; } = true;
    public GameObject ChargingBlock;

    public void horizontalMovement(Vector3 tgt)
    {
        if (tgt != null)
        {
            Vector3 target = new Vector3(tgt.x, controller.transform.position.y, tgt.z);
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, target, 3f * Time.deltaTime);
        }
    }

    public abstract IEnumerator StartDazed();

    // Gets broadcasted to at the start of the match
    public void Activate()
    {
        activated = true;
    }

    // Gets broadcasted to at the end of the match
    public void Deactivate()
    {
        activated = false;
    }


}
