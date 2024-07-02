using System;
using System.Collections;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    private Vector3[] positions = new Vector3[5]
    {
        new Vector3(50, 20, 0), new Vector3(7.5f, 10f, -7.5f), new Vector3(0, 13, -18), new Vector3(0, 40, 0), new Vector3(-20, 40, -20)
    };
    private Vector3[] rotations = new Vector3[5]
    {
        new Vector3(10, -90, 0), new Vector3(0, -45f, 0), new Vector3(5, 0, 0), new Vector3(0, 45, 0), new Vector3(0, -135, 0)
    };

    public enum cameraPositions : ushort
    {
        MAINMENU = 0,
        FIGHTERSELECT = 1,
        ARENASTART = 2,
        INFO = 3,
        SETTINGS = 4,
    }

    private int target = 0;
    private float panSpeed = 3f;
    private Vector3 currentRotation;

    private void Start()
    {
        transform.position = positions[target];
        transform.rotation = Quaternion.Euler(rotations[target]);

        currentRotation = rotations[target];
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, positions[target], panSpeed * Time.deltaTime);
        smoothAngle(); // update the current rotation with new lerp
        transform.eulerAngles = currentRotation;
    }

    // This will target the next position & rotation in the list
    public void GoNext()
    {
        target++;
        target %= positions.Length; // WRAPS POSITIONS
    }

    // This will target a specific position & rotation in the list
    public void SetTarget(cameraPositions pos)
    {
        if ((int)pos < positions.Length && (int)pos >= 0)
        {
            target = (int)pos;
        }
    }

    // like lerp but for rotations. Calls lerp on each component of the angle vector then sets it to a global var for continuous access
    private void smoothAngle() {
        currentRotation = new Vector3(
            Mathf.LerpAngle(currentRotation.x, rotations[target].x, panSpeed * Time.deltaTime),
            Mathf.LerpAngle(currentRotation.y, rotations[target].y, panSpeed * Time.deltaTime),
            Mathf.LerpAngle(currentRotation.z, rotations[target].z, panSpeed * Time.deltaTime)
        );
    }

    // If targetOfInterest matches current target, then it will wait until the camera gets close before doing something
    // If targetOfInterest doesnt match, or no longer matches, then it trashes this and returns nothing as a failsafe.
    public IEnumerator WaitForPosition(Action callback, cameraPositions targetOfInterest)
    {
        while (
                (transform.position - positions[target]).magnitude > .3f &&
                (currentRotation - rotations[target]).magnitude > .3f &&
                (int)targetOfInterest == target
                
            )
        {
            yield return null;
        }

        if((int)targetOfInterest == target)
            callback();
    }

}
