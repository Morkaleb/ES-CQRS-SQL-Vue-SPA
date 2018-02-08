using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ops.Infra.ReadModels
{
    [Route("r/")]

    public class ReadModelController : Controller
    {
        [HttpGet("{key}")]
        [Route("/api/r/{key}")]
        public JsonResult Get(string key)
        {
            var queryString = Request.QueryString.ToString();

            var blah = Book.book;
            if (Book.book.ContainsKey(key))
            {
                List<ReadModelData> returnable = Book.book[key];
                if (!string.IsNullOrEmpty(queryString))
                {
                    string[] splitQueries = queryString.Split("&");
                    string param1 = splitQueries[0].Split("=")[0];
                    param1 = param1.TrimStart('?');
                    string value1 = splitQueries[0].Split("=")[1];
                    List<ReadModelData> filtered = new List<ReadModelData>();
                    foreach (ReadModelData data in returnable)
                    {
                        Type dataType = data.GetType();
                        foreach (PropertyInfo propertyInfo in dataType.GetProperties())
                        {
                            var DataValue = getValues(data, propertyInfo.Name).ToString();
                            if (propertyInfo.Name.ToLower() == param1.ToLower() && DataValue == value1)
                            {
                                filtered.Add(data);
                            }
                        }
                    }
                    if (splitQueries.Length > 1)
                    {
                        for (int i = 1; i <= splitQueries.Length - 1; i++)
                        {
                            List<ReadModelData> refiltered = new List<ReadModelData>();
                            foreach (ReadModelData refining in filtered)
                            {
                                Type dataType = refining.GetType();
                                string param = splitQueries[i].Split("=")[0];
                                string value = splitQueries[i].Split("=")[1];
                                param = param.TrimStart('?');
                                foreach (PropertyInfo propertyInfo in dataType.GetProperties())
                                {

                                    var DataValue = getValues(refining, propertyInfo.Name).ToString();
                                    if (propertyInfo.Name.ToLower() == param.ToLower() && DataValue == value)
                                    {
                                        refiltered.Add(refining);
                                    }
                                }
                            }
                            filtered = refiltered;
                        }
                    }
                    return Json(filtered);
                }
                else { return Json(returnable); }
            }
            else { return Json(Book.book); }
        }

       
        private static string getValues(object anEvent, string name)
        {
            var value = anEvent.GetType().GetProperties()
                .Single(pi => pi.Name == name)
                .GetValue(anEvent, null);
            if (value == null) { return ""; }
            return value.ToString();
        }
    }
}
