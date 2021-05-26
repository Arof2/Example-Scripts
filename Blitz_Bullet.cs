using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blitz_Bullet : MonoBehaviour
{
    public Vector3 Seperation_Position;
    private float dis;
    public float explosions_distanz = 0.2f, amount = 10, speed = 5;
    public GameObject prefab;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
        transform.rotation *= Quaternion.Euler(90, 0, 0);
    }

    void Update()
    {
        
        dis = Vector3.Distance(transform.position, Seperation_Position);
        if(dis <= explosions_distanz)
        {
            transform.position = Seperation_Position;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            for (int i = 0; i < amount;i++)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
