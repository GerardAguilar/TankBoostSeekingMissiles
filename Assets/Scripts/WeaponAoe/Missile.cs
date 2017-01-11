using UnityEngine;
using System.Collections;
using System;

public class Missile : MonoBehaviour {



    //Using this lerp tutorial: 
    //http://www.blueraja.com/blog/404/how-to-use-unity-3ds-linear-interpolation-vector3-lerp-correctly

    public float lerpDuration = 10f;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float timeSinceStarted;
    public float percentageComplete;
    //public float lerpSpeed = .5f;
    public float lerpStart;
    private bool isLerping;

    //Using this lookat tutorial:
    //http://answers.unity3d.com/questions/254130/how-do-i-rotate-an-object-towards-a-vector3-point.html
    public float RotationSpeed = 10f;
    private Quaternion lookRotation;
    private Vector3 direction;

    private Rigidbody rb;
    public float ascentForce= 20f;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lerpStart = Time.time;
        rb.velocity = ascentForce * Vector3.up;
    }

    // Update is called once per frame
    void Update () {
        //Should lerp from the very beginning, no need to wait for a button press.
        StartLerping();

        Vector3 roundedPosition = new Vector3(
            Mathf.RoundToInt(transform.position.x*2),
            Mathf.RoundToInt(transform.position.y*2),
            Mathf.RoundToInt(transform.position.z*2));

        Vector3 roundedEndPosition = new Vector3(
            Mathf.RoundToInt(endPosition.x*2),
            Mathf.RoundToInt(endPosition.y*2),
            Mathf.RoundToInt(endPosition.z*2));

        if (roundedPosition == roundedEndPosition) {
            GetComponent<ShellExplosion>().Explode();
        }
    }

    void FixedUpdate()
    {
        if (isLerping)
        {
            //set movement
            timeSinceStarted = Time.time - lerpStart;//Gives a baseline with lerpDuration being 100%
            percentageComplete = timeSinceStarted / lerpDuration;

            //start moving
            transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);

            //set rotation
            direction = (endPosition - transform.position).normalized;//the vector pointing from our position to the target
            lookRotation = Quaternion.LookRotation(direction);//the resulting rotation required

            //start rotating
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * RotationSpeed);
        }
    }

    void StartLerping() {
        isLerping = true;
        startPosition = transform.position;//missile's position as instantiated by Weapon System
        //endPosition should be set already
    }

    public void SetTarget(Vector3 newTarget) {
        endPosition = newTarget;
    }
}
