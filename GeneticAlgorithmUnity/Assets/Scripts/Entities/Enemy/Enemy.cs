/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyChromosome chromosome;

    [SerializeField]
    private Rigidbody[] _bodyParts;

    [SerializeField]
    private float _fitness;

    public float Fitness => _fitness;

    [SerializeField]
    private float _mutationRate = 0.2f;

    [SerializeField]
    private BehaviourType _behaviour;

    [SerializeField]
    private Color _desiredColor = Color.green;

    public BehaviourType Behaviour => _behaviour;

    public void Initialize(EnemyChromosome chromosome = null)
    {
        if (chromosome == null)
        {
            this.chromosome = new EnemyChromosome(_mutationRate, true);
        } else
        {
            this.chromosome = (EnemyChromosome)chromosome.Copy();
        }

        UpdateValues();
    }

    /// <summary>
    /// Update data based on chromosome's genes.
    /// </summary>
    public void UpdateValues()
    {
        GetComponent<MeshRenderer>().material.color = this.chromosome.GetColor();
        _behaviour = chromosome.GetBehaviour();
        
        if (weapon != null)
        {
            Destroy(weapon.gameObject);
        }

        foreach (Rigidbody rb in _bodyParts)
        {
            rb.GetComponent<MeshRenderer>().material.color = chromosome.GetColor();
            rb.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", chromosome.GetColor() * 1.7f);
        }

        WeaponManager.Instance.CreateWeapon(this, chromosome.GetWeapon());
    }

    public void UpdateFitness()
    {
        *//*_fitness = _statistics.damageTaken > 0 ? (1 / _statistics.damageTaken) : 1;*//*

        _fitness = _statistics.Accuracy;
    }

    private void Update()
    {
        _statistics.timeAlive += Time.deltaTime;
    }

    private IEnumerator DeathAnimation(ProjectileOld projectile = null)
    {
        Vector3 impactPoint;
        float explosionForce;
        float explosionRadius;

        if (projectile != null)
        {
            impactPoint = projectile.transform.position;
            explosionForce = projectile.weapon.ProjectileData.damage;
        } else
        {
            impactPoint = transform.position;
            explosionForce = 5;
        }

        explosionRadius = Random.Range(5, 20) * 3;

        isDead = true;

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        weapon.gameObject.SetActive(false);

        foreach (Rigidbody rb in _bodyParts)
        {
            rb.isKinematic = false;
            rb.GetComponent<Collider>().enabled = true;
            rb.AddExplosionForce(explosionForce * 600, impactPoint, explosionRadius);
            rb.gameObject.layer = LayerMask.NameToLayer("Nothing");
        }

        gameObject.layer = LayerMask.NameToLayer("Nothing");

        yield return new WaitForSeconds(4f);
    }

    protected override void OnHitEvent(Entity target, float damage, ProjectileOld projectile = null)
    {
        if (target.tag == tag)
        {
            _statistics.friendlyFireHitCount++;
        } else
        {
            _statistics.hitCount++;
            _statistics.damageDealt += damage;
        }
    }

    protected override void WhenHitEvent(Entity attacker, float damage, ProjectileOld projectile = null)
    {
        if (attacker.tag != tag)
        {
            ReceiveDamage(attacker, damage, projectile);
            _statistics.damageTaken += damage;
        }
    }

    protected override void OnKillEvent(Entity target, ProjectileOld projectile = null)
    {
        _statistics.killCount++;
    }

    protected override void WhenKilledEvent(Entity attacker, ProjectileOld projectile = null)
    {
        isDead = true;
        _statistics.deathCount++;
        StartCoroutine(DeathAnimation(projectile));
    }

    protected override void OnWeaponFiredEvent()
    {
        _statistics.projectilesFired++;
    }
}
*/