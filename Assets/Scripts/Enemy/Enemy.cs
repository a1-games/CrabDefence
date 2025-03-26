using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private float maxHealth;
    public float damage = 10f;
    public float speed = 2f;
    public int bounty = 10;

    [HideInInspector] public int id;

    public Transform target;
    private TrailPoint currentPoint;

    private SpriteRenderer sr;
    public Transform spriteMask;
    public GameObject deathSoundObject;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        maxHealth = health;
    }
    private void Update()
    {
        Movement();
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            if (target == TrailPointManager.AskFor.points[TrailPointManager.AskFor.points.Length - 1])
            {
                GameManager.AskFor.PlayerTakesDamage(damage);
                SoundManager.AskFor.PlayerDamage();
                DeathBySuicide();
                return;
            }
            GetTrailPointScript();
            target = currentPoint.nextPosition;
        }
    }

    public void Movement()
    {
        Vector3 targetDir = target.position - transform.position;
        targetDir.Normalize();
        this.transform.position += targetDir * speed * Time.deltaTime;
    }

    private bool isDead = false;
    public void TakeDamage(float damage)
    {
        health -= damage;
        spriteMask.localScale = new Vector3((float)health/maxHealth, 1f, 1f);

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if (isDead) return;
        isDead = true;
        GameManager.AskFor.enemies.Remove(this);
        GameManager.AskFor.AddCoins(bounty);
        GameManager.AskFor.AddToScore(8);
        AudioSource a = Instantiate(deathSoundObject).GetComponent<AudioSource>();
        a.pitch = Random.Range(0.78f, 0.96f);
        a.Play();
        Destroy(this.gameObject);
    }
    private void DeathBySuicide()
    {
        if (isDead) return;
        isDead = true;
        GameManager.AskFor.enemies.Remove(this);
        Destroy(this.gameObject);
    }

    private void GetTrailPointScript()
    {
        currentPoint = target.GetComponent<TrailPoint>();
    }
}
