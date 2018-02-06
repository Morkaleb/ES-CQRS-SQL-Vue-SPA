using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ops.Infra.ReadModels;
using Ops.Infra.Sql;

namespace Ops.Infra.EventStore
{
    public class EventDistributor
    {
        public static void Publish(EventFromES anEvent)
        {
            dynamic readmodelData;
            string nspace = "Manager.src.ReadModels";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t;
            foreach (var readModel in q)
            {
                string[] rMName = readModel.Name.ToString().Split("Data");
                string[] rMName2 = readModel.Name.ToString().Split("Read");
                if (rMName.Length == 1 && rMName2.Length > 1)
                {
                    try
                    {
                        MethodInfo theMethod = typeof(ReadModels.ReadModel).GetMethod("EventPublish", new[] { typeof(EventFromES) });
                        string methodName = readModel.Name.ToString();
                        string nameSpace = readModel.Namespace.ToString();
                        string key = methodName.Split("Read")[0];
                        if (Book.book.ContainsKey(key))
                        {
                            string fullClassName = nameSpace + "." + methodName;
                            object readModelToInvoke = Activator.CreateInstance(Type.GetType(fullClassName));
                            readmodelData = theMethod.Invoke(readModelToInvoke, new EventFromES[] { anEvent });
                        }
                        else
                        {
                            Book.book.Add(key, new List<ReadModelData>());
                            string fullClassName = nameSpace + "." + methodName;
                            object readModelToInvoke = Activator.CreateInstance(Type.GetType(fullClassName));
                            readmodelData = theMethod.Invoke(readModelToInvoke, new EventFromES[] { anEvent });
                        }
                        if (key.Split("Table").Length > 1 && readmodelData != null)
                        {
                            TableReadmodelInterface.UpdateTable(readmodelData, key);
                        }
                    }
                    catch (Exception e)
                    {

                        Console.Write(e);
                    }
                }


            }

        }
    }
}
