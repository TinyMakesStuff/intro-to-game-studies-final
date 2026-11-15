using System.Collections;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public Transform target;
    public float life;
    public int damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(timeToDeath());
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 5 * Time.deltaTime);
    }

    IEnumerator timeToDeath()
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }
}
