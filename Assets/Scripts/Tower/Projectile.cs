using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int penetrationInt = 1;
    private int currentlyPenetrated = 0;

    public float damage;
    [HideInInspector] public Vector3 targetDirection;
    public float timeBeforeDeath = 20f;
    private float timeAtSpawn = 0f;
    public AudioSource audioSource;

    private void Start()
    {
        timeAtSpawn = Time.time;
    }
    private void Update()
    {
        this.transform.position += targetDirection * speed * Time.deltaTime;

        if (timeBeforeDeath + timeAtSpawn < Time.time) Death();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            currentlyPenetrated++;
            //sound of hit
            audioSource.pitch = Random.Range(2f, 2.5f);
            audioSource.Play();
            if (currentlyPenetrated >= penetrationInt) Death();
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
