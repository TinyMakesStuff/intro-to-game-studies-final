using UnityEngine;

public class GunTurretLogic : MonoBehaviour
{
    // VARIABLES
    public int range;
    public LayerMask enemyLayer;
    public GameObject bullet;
    public float attackCoolDown = 2f;
    public float coolDownTimer = 0;
    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coolDownTimer -= Time.deltaTime;
        if(coolDownTimer <= 0f)
        {
            Transform target = getClosestEnemy();
            if(target != null)
            {
                attack(target);
                coolDownTimer = attackCoolDown;
            }
        }
    }

    Transform getClosestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);

        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach(Collider col in hitColliders)
        {
            Vector3 directionToTarget = col.transform.position - transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = col.transform;
            }
        }

        return closestEnemy;
    }

    void attack(Transform target)
    {
        // then, spawn a projectile
        // Debug.Log(target);
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.position = transform.position;

        newBullet.GetComponent<BulletMove>().target = target;
        newBullet.GetComponent<BulletMove>().life = 1f;
        newBullet.GetComponent<BulletMove>().damage = damage;
    }
}
