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
        _fitness = 1 / statistics.projectilesFired;
    }

    private void Update()
    {
        statistics.timeAlive += Time.deltaTime;
    }

    protected override void Killed()
    {
        isDead = true;

        StartCoroutine(DeathAnimation());
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

    protected override void OnHitEvent(Entity entity, float damage, Projectile projectile = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void WhenHitEvent(Entity entity, float damage, Projectile projectile = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnKillEvent(Entity entity, Projectile projectile = null)
    {
        
    }

    protected override void WhenKilledEvent(Entity entity, Projectile projectile = null)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnWeaponFiredEvent()
    {
        throw new System.NotImplementedException();
    }
}
