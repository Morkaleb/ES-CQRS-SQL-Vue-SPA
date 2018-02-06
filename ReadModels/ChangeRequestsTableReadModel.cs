using Ops.Infra.EventStore;
using Ops.Infra.ReadModels;

namespace Ops.ReadModels
{
    public class ChangeRequestsTableReadModel : ReadModel
    {
        public ChangeRequestsTableReadModel() { }
        public override dynamic EventPublish(EventFromES anEvent)
        {
            var readModelColleciton = Book.book;
            var data = anEvent.Data;

            switch (anEvent.EventType)
            {
                case "ManagerDayScheduleChangeRequested":
                    ChangeRequestsTableData request = new ChangeRequestsTableData
                    {
                        Id = data["RequestId"],
                        FromId=data["ManagerId"],
                        Status = "Pending",
                        Reason = data["Reason"],
                        ShiftDate = data["ShiftDate"],
                        FromName = data["ManagerFromName"],
                        ToName = data["ManagerToName"],
                        ShiftCode = data["ShiftCode"],
                        EOW = data["EOW"],
                        LocationId = data["LocationId"]
                    };
                    readModelColleciton["ChangeRequestsTable"].Add(request);
                    return request;

                case "PayrollApprovalRequire":
                    string Id = data["RequestId"];
                    int index = readModelColleciton["ChangeRequestsTable"].FindIndex(r => r.Id == Id);
                    ChangeRequestsTableData thisRequest = (ChangeRequestsTableData)readModelColleciton["ChangeRequestsTable"][index];
                    thisRequest.Status = "Pending Payroll";
                    return thisRequest;


                case "GMApprovedScheduleChange":
                    Id = data["RequestId"];
                    index = readModelColleciton["ChangeRequestsTable"].FindIndex(r => r.Id == Id);
                    thisRequest = (ChangeRequestsTableData)readModelColleciton["ChangeRequestsTable"][index];
                    thisRequest.Status = "Approved";
                    return thisRequest;

                case "GMRejectedScheduleChange":
                    Id = data["RequestId"];
                    index = readModelColleciton["ChangeRequestsTable"].FindIndex(r => r.Id == Id);
                    thisRequest = (ChangeRequestsTableData)readModelColleciton["ChangeRequestsTable"][index];
                    thisRequest.Status = "Denied";
                    return thisRequest;

                case "ManagerDayScheduleChanged":
                    request = new ChangeRequestsTableData
                    {
                        Id = data["RequestId"],
                        FromId = data["ManagerId"],
                        Status = "",
                        Reason = data["Reason"],
                        ShiftDate = data["ShiftDate"],
                        FromName = data["ManagerFromName"],
                        ToName = data["ManagerToName"],
                        ShiftCode = data["ShiftCode"],
                        EOW = data["EOW"],
                        LocationId = data["LocationId"]
                    };
                    readModelColleciton["ChangeRequestsTable"].Add(request);
                    return request;
            }
            return null;
        }
    }

    public class ChangeRequestsTableData : ReadModelData
    {
        public string Status { get; set; }
        public string Reason { get; set; }
        public string ShiftDate { get; set; }
        public string FromName { get; set; }
        public string FromId { get; set; }
        public string ShiftCode { get; set; }
        public string ToName { get; set; }
        public string EOW { get; set; }
        public string LocationId { get; set; }
    }
}
