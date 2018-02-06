using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;
using Ops.Infra.Sql;

namespace Ops.Infra
{
    public class OnStart
    {
        public static void Start()
        {
            ReadModelRegistration.Register();
            TableReadmodelInterface.CheckForTables();
            EventStoreInterface.StartConnection();
            EventStoreInterface.ReadSavedEvents();
        }
    }
}
