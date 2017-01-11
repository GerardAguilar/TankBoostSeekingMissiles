//Missile destroys itself before triggering the explosion.

using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponSystem : MonoBehaviour {

    public string playerNum;
    public TankShooting tankShooting;

    public Rigidbody missile;
    public Transform fireTransform;
    public List<GameObject> targetObjects;


    private GameObject aoe;
    private AoE aoeScript;
    //private Vector3 target;

	// Use this for initialization
	void Start () {
        tankShooting = GetComponentInParent<TankShooting>();
        fireTransform = tankShooting.m_FireTransform;
        playerNum = tankShooting.m_PlayerNumber + "";
        aoe = tankShooting.aoe;
        aoeScript = aoe.GetComponent<AoE>();
        aoe.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Missile" + playerNum))
        {
            //this is where we aim and collect
            Debug.Log("Missile" + playerNum + " was pressed.");
            aoe.SetActive(true);
        }
        else if (Input.GetButtonUp("Missile" + playerNum))
        {
            //This is also where we launch the missiles
            if (targetObjects.Count != 0) {
                Debug.Log("Target: " + aoeScript.target);
                FireMissile(targetObjects[0].transform.position);             
            }
            aoe.SetActive(false);
            ClearTargetArray();
        }
    }

    void FireMissile(Vector3 target) {
        target = new Vector3(target.x, target.y + .85f, target.z);
        Debug.Log("Fire Missile at " + target);

        //Need to instantiate missile
        Rigidbody shellInstance = Instantiate(missile, fireTransform.position, fireTransform.rotation) as Rigidbody;
        //Lerp the position
        //shellInstance.velocity = 5f * fireTransform.forward;

        shellInstance.GetComponent<Missile>().SetTarget(target);
        shellInstance.GetComponent<ShellExplosion>().flagOwner(playerNum);
        //the lock on should be time-limited
    }

    public void AddTarget(GameObject target) {
        targetObjects.Add(target);
    }

    public bool IsTargetInArray(GameObject target) {
        bool targetIsInArray = false;

        for (int i = 0; i < targetObjects.Count; i++) {
            if (target == targetObjects[i]) {
                targetIsInArray = true;
                break;
            }
        }

        return targetIsInArray;
    }

    public void ClearTargetArray() {
        targetObjects = new List<GameObject>();
        
    }

}
