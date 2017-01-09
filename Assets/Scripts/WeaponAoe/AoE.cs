using UnityEngine;
using System.Collections;

public class AoE : MonoBehaviour {

    public Vector3 target;
    public bool targetInRange;

    string playerNum;
    TankShooting tankShooting;
    WeaponSystem weaponSystem;

	// Use this for initialization
	void Awake () {
        weaponSystem = GetComponentInParent<WeaponSystem>();
        playerNum = weaponSystem.playerNum;
        tankShooting = GetComponentInParent<TankShooting>();
	}

    public void OnTriggerEnter(Collider other) {
        //place in weapon system target array
        if (IsEnemy(other))
        {
            if (IsNewEnemy(other))
            {
                weaponSystem.AddTarget(other.gameObject);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        //turn on the targetting UI of opposing enemy
        //track enemy's position
        targetInRange = IsEnemy(other);
        if (targetInRange)
        {
            //StartCoroutine(LoadMissile(other));
            LoadMissile(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //remove from weapon system target array
    }

    public void OnDisable() {
        target = new Vector3();
        weaponSystem.ClearTargetArray();
        //flush out Weapon System's target Array
    }

    public bool IsEnemy(Collider other)
    {

        bool itIsEnemy = false;
        if (!other.gameObject.CompareTag("Player"))
        {
            itIsEnemy = false;
        }
        else if (other.GetComponent<TankShooting>().m_PlayerNumber != tankShooting.m_PlayerNumber)
        {
            itIsEnemy = true;
        }

        return itIsEnemy;
    }

    //IEnumerator LoadMissile(Collider other)
    //{
    //    //Debug.Log("Loading"+playerNum);
    //    //Debug.Log("Targeting: " + other.transform.position);
    //    target = other.transform.position;
    //    yield return new WaitForSeconds(10f);
    //}

    void LoadMissile(Collider other)
    {
        //Debug.Log("Loading"+playerNum);
        //Debug.Log("Targeting: " + other.transform.position);
        target = other.transform.position;
    }

    bool IsNewEnemy(Collider other) {
        //if the enemy is already in this AoE's array, then don't worry
        bool itIsNewEnemy = false;
        if (!weaponSystem.IsTargetInArray(other.gameObject)) {
            itIsNewEnemy = true;
        }
        return itIsNewEnemy;
    }

    
}
