using MassTransit;
using MassTransitApp3.Sagas.States;
using ObjectClass;

namespace MassTransitApp3.Sagas
{
    public class SagaStateMachine : MassTransitStateMachine<ObjectsForSaga>
    {
        public State Made { get; private set; }
        public State Transfered { get; private set; }
        public State Processed { get; private set; }
        public State Completed { get; private set; }

        public Event<ObjectMade> ObjectMade { get; private set; }
        public Event<ObjectTransfered> ObjectTransfered { get; private set; }
        public Event<ObjectProcessed> ObjectProcessed { get; private set; }
        public Event<ObjectCompleted> ObjectCompleted { get; private set; }

        public SagaStateMachine(ILogger<SagaStateMachine> logger)
        {
            try
            {
                InstanceState(x => x.State);

                Event(() => ObjectMade, x => x.CorrelateById(context => context.Message.CorrelationId));
                Event(() => ObjectTransfered, x => x.CorrelateById(context => context.Message.CorrelationId));
                Event(() => ObjectProcessed, x => x.CorrelateById(context => context.Message.CorrelationId));
                Event(() => ObjectCompleted, x => x.CorrelateById(context => context.Message.CorrelationId));

                Initially(
                    When(ObjectMade)
                    .Then(context =>
                    {
                        logger.LogInformation($"\nObject Made: " +
                            $"\n{context.Message.CorrelationId}" +
                            $"\n{context.Message.ObjectName}" +
                            $"\n{context.Message.ObjectDescription}");
                        context.Saga.CorrelationId = context.Message.CorrelationId;
                        context.Saga.ObjectName = context.Message.ObjectName;
                        context.Saga.ObjectDescription = context.Message.ObjectDescription;
                    })
                    .TransitionTo(Made)
                    .Publish(context => new ObjectTransfered
                    {
                        CorrelationId = context.Message.CorrelationId,
                        ObjectName = context.Message.ObjectName,
                        ObjectDescription = context.Message.ObjectDescription
                    })
                );

                During(Made,
                    When(ObjectTransfered)
                    .Then(context =>
                    {
                        logger.LogInformation($"\nObject Transfered: " +
                            $"\n{context.Message.CorrelationId}" +
                            $"\n{context.Message.ObjectName}" +
                            $"\n{context.Message.ObjectDescription}");
                        context.Saga.CorrelationId = context.Message.CorrelationId;
                        context.Saga.ObjectName = context.Message.ObjectName;
                        context.Saga.ObjectDescription = context.Message.ObjectDescription;
                    })
                    .TransitionTo(Transfered)
                    .Publish(context => new ObjectProcessed
                    {
                        CorrelationId = context.Message.CorrelationId,
                        ObjectName = context.Message.ObjectName,
                        ObjectDescription = context.Message.ObjectDescription
                    })
                );

                During(Transfered,
                    When(ObjectProcessed)
                    .Then(context =>
                    {
                        logger.LogInformation($"\nObject Processed: " +
                            $"\n{context.Message.CorrelationId}" +
                            $"\n{context.Message.ObjectName}" +
                            $"\n{context.Message.ObjectDescription}");
                        context.Saga.CorrelationId = context.Message.CorrelationId;
                        context.Saga.ObjectName = context.Message.ObjectName;
                        context.Saga.ObjectDescription = context.Message.ObjectDescription;
                    })
                    .TransitionTo(Processed)
                    .Publish(context => new ObjectCompleted
                    {
                        CorrelationId = context.Message.CorrelationId,
                        ObjectName = context.Message.ObjectName,
                        ObjectDescription = context.Message.ObjectDescription
                    })
                );

                During(Processed,
                    When(ObjectCompleted)
                    .Then(context =>
                    {
                        logger.LogInformation($"\nObject Completed: " +
                            $"\n{context.Message.CorrelationId}" +
                            $"\n{context.Message.ObjectName}" +
                            $"\n{context.Message.ObjectDescription}");
                        context.Saga.CorrelationId = context.Message.CorrelationId;
                        context.Saga.ObjectName = context.Message.ObjectName;
                        context.Saga.ObjectDescription = context.Message.ObjectDescription;
                    })
                    .TransitionTo(Completed)
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
