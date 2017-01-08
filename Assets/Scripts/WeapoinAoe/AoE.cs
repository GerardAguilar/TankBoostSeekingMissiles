using UnityEngine;
using System.Collections;

public class AoE : MonoBehaviour {

    string playerNum;
    TankShooting tankShooting;

	// Use this for initialization
	void Start () {
        playerNum = GetComponentInParent<WeaponSystem>().playerNum;
        tankShooting = GetComponentInParent<TankShooting>();
	}

    public void OnTriggerStay(Collider other)
    {
        if (IsEnemy(other))
        {
            StartCoroutine(LoadMissile());
        }
    }

    private bool IsEnemy(Collider other)
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

    IEnumerator LoadMissile()
    {
        Debug.Log("Loading"+playerNum);
        yield return new WaitForSeconds(2f);
    }
}
