using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyChromosome chromosome;

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

    public float timeAlive = 0f;
    public float damageTaken = 0f;
    public float damageDealt = 0f;

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
        /*        Color currentColor = chromosome.GetColor();

                Vector3 currentColorPosition = new Vector3(currentColor.r, currentColor.g, currentColor.b);
                Vector3 desiredColorPosition = new Vector3(_desiredColor.r, _desiredColor.g, _desiredColor.b);

                float distance = Vector3.Distance(currentColorPosition.normalized, desiredColorPosition.normalized);

                _fitness = 1 / distance;*/

        _fitness = damageDealt;
    }

    private void WhenDamageTaken(Entity self, Entity other, float damage)
    {
        damageTaken += damage;
    }

    private void WhenDamageDealt(Entity self, Entity other, float damage)
    {
        damageDealt += damage;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
    }

    private void OnEnable()
    {
        whenHit += WhenDamageTaken;
        onHit += WhenDamageDealt;
    }

    private void OnDisable()
    {
        whenHit -= WhenDamageTaken;
        onHit -= WhenDamageDealt;
    }
}
