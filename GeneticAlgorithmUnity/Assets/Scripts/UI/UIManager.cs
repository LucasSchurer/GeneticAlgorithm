using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _generationText;
    [SerializeField]
    private TextMeshProUGUI _populationFitnessText;
    [SerializeField]
    private TextMeshProUGUI _averagePopulationFitnessText;

    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private string GenericText(string text) => text.Substring(0, text.IndexOf(':') + 1) + " ";

    public void SetGeneration(int generation)
    {
        _generationText.text = GenericText(_generationText.text) + generation.ToString();
    }

    public void SetPopulationFitness(float populationFitness, float averagePopulationFitness)
    {
        _populationFitnessText.text = GenericText(_populationFitnessText.text) + populationFitness.ToString();
        _averagePopulationFitnessText.text = GenericText(_averagePopulationFitnessText.text) + averagePopulationFitness.ToString();
    }
}
