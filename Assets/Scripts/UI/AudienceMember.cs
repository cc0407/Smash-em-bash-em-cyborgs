using System.Collections;
using UnityEngine;

public class AudienceMember : MonoBehaviour
{

    public Material[] potentialMaterials;
    public MeshRenderer MaterialRenderer;
    public Rigidbody rb;

    private float maxJumpAmt = 10f;
    private float maxDelay = 0.2f;

    // Sets a random colour for this audience member
    void Start()
    {
        MaterialRenderer.material = GetRandomMaterial();
    }

    // Picks one material out of $potentialMaterials
    Material GetRandomMaterial()
    {
        int rndIndex = Util.rnd.Next(0, potentialMaterials.Length);
        return potentialMaterials[rndIndex];
    }

    // Waits up to $maxDelay, then applies a random force up to $maxJumpAmt. Used when reacting to hits and blocks
    public IEnumerator React()
    {
        float del = (float)Util.rnd.NextDouble() * maxDelay;
        yield return new WaitForSeconds(del);
        float jumpAmt = (float)Util.rnd.NextDouble() * maxJumpAmt;
        rb.AddForce(Vector3.up * jumpAmt, ForceMode.Impulse);
    }
}