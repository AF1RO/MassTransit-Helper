namespace MassTransitApp3.Sagas.States
{
    public class ObjectMade
    {
        public Guid CorrelationId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectDescription { get; set; }
    }
}
