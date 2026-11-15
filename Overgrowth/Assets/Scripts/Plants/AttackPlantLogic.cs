using UnityEngine;

public class AttackPlantLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "player")
        {
            col.gameObject.GetComponent<PlayerMovement>().damage += 2;
        } else if(col.gameObject.tag == "gunTurret")
        {
            col.gameObject.GetComponent<GunTurretLogic>().damage += 2;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "player")
        {
            col.gameObject.GetComponent<PlayerMovement>().damage -= 2;
        }
    }
}
