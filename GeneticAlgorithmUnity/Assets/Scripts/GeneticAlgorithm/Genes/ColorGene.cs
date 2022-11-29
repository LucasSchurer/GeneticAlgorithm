using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA
{
    public class ColorGene : Gene
    {
        public Color color;
        public float value;

        public ColorGene(Color color)
        {
            this.color = color;
        }

        public override void Apply(CreatureController creature)
        {
            MeshRenderer meshRenderer = creature.GetComponent<MeshRenderer>();

            if (meshRenderer)
            {
                meshRenderer.material.color = color;
                meshRenderer.material.SetColor("_EmissionColor", color * 1.7f);
            }

            foreach (Rigidbody rb in creature.GetComponentsInChildren<Rigidbody>())
            {
                MeshRenderer rbMeshRenderer = rb.GetComponent<MeshRenderer>();

                if (rbMeshRenderer)
                {
                    rbMeshRenderer.material.color = color;
                    rbMeshRenderer.material.SetColor("_EmissionColor", color * 1.7f);
                }
            }
        }

        public override Gene Copy()
        {
            ColorGene copy = new ColorGene(color);
            return copy;
        }

        public override void Mutate()
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        }

        public override void Randomize()
        {
            color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    } 
}

