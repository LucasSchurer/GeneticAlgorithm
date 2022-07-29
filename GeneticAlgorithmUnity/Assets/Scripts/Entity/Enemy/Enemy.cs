using System.Collections;
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

        WeaponManager.Instance.CreateWeapon(this, chromosome.GetWeapon());
    }

    public void UpdateFitness()
    {
        _fitness = _statistics.damageDealt;
    }

    private void Update()
    {
        _statistics.timeAlive += Time.deltaTime;
    }

    private IEnumerator DeathAnimation()
    {
        isDead = true;

        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        weapon.gameObject.SetActive(false);

        foreach (Rigidbody rb in _bodyParts)
        {
            rb.isKinematic = false;
            rb.GetComponent<Collider>().enabled = true;
            rb.AddExplosionForce(Random.Range(5, 10) * 100, transform.position, Random.Range(5, 10) * 10);
            rb.gameObject.layer = LayerMask.NameToLayer("Nothing");
        }

        gameObject.layer = LayerMask.NameToLayer("Nothing");

        yield return new WaitForSeconds(4f);
    }

    protected override void OnHitEvent(Entity target, float damage, Projectile projectile = null)
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

    protected override void WhenHitEvent(Entity attacker, float damage, Projectile projectile = null)
    {
        if (attacker.tag != tag)
        {
            ReceiveDamage(attacker, damage, projectile);
            _statistics.damageTaken += damage;
        }
    }

    protected override void OnKillEvent(Entity target, Projectile projectile = null)
    {
        _statistics.killCount++;
    }

    protected override void WhenKilledEvent(Entity attacker, Projectile projectile = null)
    {
        isDead = true;
        _statistics.deathCount++;
        StartCoroutine(DeathAnimation());
    }

    protected override void OnWeaponFiredEvent()
    {
        _statistics.projectilesFired++;
    }
}
