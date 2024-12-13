using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TvScript : SmartHomeConsumer
{
    private MeshRenderer meshRenderer;
    public bool on = false;
    public Material onMaterial;
    public Material offMaterial;
    public override void UpdateFromState(State state)
    {
        Debug.Log(state);
        on = state.state == "on";
        List<Material> materialsCopy = meshRenderer.materials.ToList();

        if (on) {
            materialsCopy[1] = onMaterial;
        } else {
            materialsCopy[1] = offMaterial;
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
