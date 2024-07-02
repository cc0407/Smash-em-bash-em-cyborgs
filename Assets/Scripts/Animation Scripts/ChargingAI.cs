using UnityEngine;

public class ChargingAI : StateMachineBehaviour
{
    // Move towards charging
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Broadcast("AIMoveToCharger");
    }

    private void Broadcast(string functionName, object arg = null)
    {
        var gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (var go in gameObjects)
        {
            // Only root game object.
            if (go && go.transform.parent == null)
            {
                go.BroadcastMessage(functionName, arg, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
