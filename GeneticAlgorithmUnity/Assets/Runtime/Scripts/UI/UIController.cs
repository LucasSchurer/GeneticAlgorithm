using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _generationText;
    [SerializeField]
    private TextMeshProUGUI _populationFitnessText;
    [SerializeField]
    private TextMeshProUGUI _averagePopulationFitnessText;

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
