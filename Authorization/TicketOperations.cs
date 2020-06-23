using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ComunitateaMea.Authorization
{
    public static class TicketOperations
    {
        public static OperationAuthorizationRequirement Create =
          new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
          new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
          new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
          new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
        public static OperationAuthorizationRequirement Approve =
          new OperationAuthorizationRequirement { Name = Constants.ApproveOperationName };
        public static OperationAuthorizationRequirement Reject =
          new OperationAuthorizationRequirement { Name = Constants.RejectOperationName };
        public static OperationAuthorizationRequirement InProgress =
          new OperationAuthorizationRequirement { Name = Constants.InProgressOperationName };
        public static OperationAuthorizationRequirement Done =
          new OperationAuthorizationRequirement { Name = Constants.DoneOperationName };
        public static OperationAuthorizationRequirement NotPossible =
          new OperationAuthorizationRequirement { Name = Constants.NotPossibleOperationName };
        public static OperationAuthorizationRequirement TicketAdministrators =
          new OperationAuthorizationRequirement { Name = Constants.TicketAdministratorsRole };
        public static OperationAuthorizationRequirement TicketManagers =
          new OperationAuthorizationRequirement { Name = Constants.TicketManagersRole };
    }

    public class Constants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string ApproveOperationName = "Approve";
        public static readonly string RejectOperationName = "Reject";
        public static readonly string InProgressOperationName = "InProgress";
        public static readonly string DoneOperationName = "Done";
        public static readonly string NotPossibleOperationName = "NotPossible";
        public static readonly string TicketAdministratorsRole = "TicketAdministrators";
        public static readonly string TicketManagersRole = "TicketManagers";
    }
}
