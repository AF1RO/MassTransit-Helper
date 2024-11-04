namespace MassTransitApp3.Sagas.States
{
    public class ObjectProcessed
    {
        public Guid CorrelationId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectDescription { get; set; }
    }
}