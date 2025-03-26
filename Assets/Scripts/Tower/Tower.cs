using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int price;

    public float range;
    public bool canShoot = false;
    public float shootCooldown;
    public int penetrationInt = 1;
    public float damage;
    public float projectileSpeed = 3f;

    private Enemy targetEnemy;
    private bool enemyIsInRange = false;

    public GameObject projectile;

    public Sprite[] skins;

    private float savedTime;

    public GameObject rangeIndicator;

    public AudioSource hitAudioSource;

    private void Start()
    {
        savedIndicatorScale = rangeIndicator.transform.localScale;
        RefreshRangeIndicator();
        GameManager.AskFor.towers.Add(this);
        RandomizeSkin();
    }

    private void Update()
    {
        SearchForEnemies();
        if (shootCooldown + savedTime < Time.time)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!canShoot) return;
        if (targetEnemy == null) return;
        if (Vector2.Distance(targetEnemy.transform.position, this.transform.position) > range)
            return;
        Projectile p = Instantiate(projectile, transform.position, transform.rotation).GetComponent<Projectile>();
        p.targetDirection = ((Vector2)targetEnemy.transform.position - (Vector2)this.transform.position).normalized;
        p.damage = this.damage;
        p.penetrationInt = this.penetrationInt;
        p.speed = this.projectileSpeed;
        p.audioSource = this.hitAudioSource;

        savedTime = Time.time;
    }

    public void SearchForEnemies()
    {
        targetEnemy = null;
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < GameManager.AskFor.enemies.Count; i++)
        {
            if (Vector2.Distance(GameManager.AskFor.enemies[i].transform.position, this.transform.position) < range)
            {
                if (targetEnemy == null) targetEnemy = GameManager.AskFor.enemies[i];

                // targeting: first
                if (GameManager.AskFor.enemies[i].id < targetEnemy.id) targetEnemy = GameManager.AskFor.enemies[i]; 

            }
        }
        if (targetEnemy != null)
        {
            enemyIsInRange = true;
            this.GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.2f, 1f);
        }
    }

    private Vector3 savedIndicatorScale;
    public void RefreshRangeIndicator()
    {
        rangeIndicator.transform.localScale = savedIndicatorScale * range;
    }

    public void RandomizeSkin()
    {
        this.GetComponent<SpriteRenderer>().sprite = skins[Random.Range(0, skins.Length)];
    }
}
