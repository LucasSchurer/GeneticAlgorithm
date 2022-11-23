using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class PopulationManager : MonoBehaviour
    {
        [SerializeField]
        private FitnessProperties _fitnessProperties;

        public static PopulationManager Instance { get; private set; }
        public FitnessProperties FitnessProperties => _fitnessProperties;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _fitnessProperties.BalancePropertiesWeights();
            }

            DontDestroyOnLoad(gameObject);
        }
    } 
}
