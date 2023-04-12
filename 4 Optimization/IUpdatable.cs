namespace WOE.LiveObjects.Updates
{
    public interface IUpdatable
    {
        public float Delay { get; }
        public float Time { get; set; }

        public void Update();
    }
}
