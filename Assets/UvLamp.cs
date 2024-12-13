using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UvLamp : SmartHomeConsumer
{
    public Material onMaterial;
    public Material offMaterial;
    private MeshRenderer meshRenderer;

    private bool on = false;
    public override void UpdateFromState(State state)
    {

        on = state.state == "on";
        List<Material> materialsCopy = meshRenderer.materials.ToList();
        if (on) {
            materialsCopy[0] = onMaterial;
        } else {
            materialsCopy[0] = offMaterial;
        }
        meshRenderer.SetMaterials(materialsCopy);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
