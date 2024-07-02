using UnityEngine;

public class ChargingHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == "Christian_Robot")
        {
            collisionInfo.gameObject.BroadcastMessage("ChangeBattery", 4f * Time.deltaTime);

            // Player handles boot out differently
            if (collisionInfo.gameObject.tag != "Player")
                collisionInfo.gameObject.BroadcastMessage("AttemptBootOut");
        }
    }
}
