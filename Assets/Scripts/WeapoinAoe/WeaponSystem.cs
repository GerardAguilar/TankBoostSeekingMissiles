using UnityEngine;
using System.Collections;

public class WeaponSystem : MonoBehaviour {

    public string playerNum;
    public TankShooting tankShooting;
    private GameObject aoe;

	// Use this for initialization
	void Start () {
        tankShooting = GetComponentInParent<TankShooting>();
        playerNum = tankShooting.m_PlayerNumber + "";
        aoe = tankShooting.aoe;
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
        else if (Input.GetButtonUp("Missile" + playerNum)) {
            //This is also where we launch the missiles
            aoe.SetActive(false);
        }
    }
}
