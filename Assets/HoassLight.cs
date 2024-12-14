using UnityEngine;

public class HoassLight : SmartHomeConsumer
{
    public HoassLight()
    {

    }

    public override void UpdateFromState(State state)
    {
        gameObject.SetActive(state.state == "on");
    }

    void OnMouseDown() {
        Debug.Log("Mousedown");
    }

    void Start() {}
}