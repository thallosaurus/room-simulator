public class HoassLight : SmartHomeConsumer
{
    public HoassLight()
    {

    }

    public override void UpdateFromState(State state)
    {
        gameObject.SetActive(state.state == "on");
    }

    void Start() {}
}