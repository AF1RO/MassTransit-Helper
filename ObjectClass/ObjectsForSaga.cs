using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace ObjectClass
{
    public class ObjectsForSaga : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectDescription { get; set; }
        public string State { get; set; }
    }
}