using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ComunitateaMea.Models;
using System.Threading.Tasks;

namespace ComunitateaMea.Authorization
{
    public class ComunitateaMeaAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Ticket>
    {
        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context,
                                   OperationAuthorizationRequirement requirement,
                                   Ticket resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            // If not asking for approval/reject, return.
            if (requirement.Name != Constants.ApproveOperationName &&
                requirement.Name != Constants.RejectOperationName  &&
                requirement.Name != Constants.InProgressOperationName &&
                requirement.Name != Constants.DoneOperationName &&
                requirement.Name != Constants.NotPossibleOperationName)
            {
                return Task.CompletedTask;
            }

            // Managers can approve or reject.
            if (context.User.IsInRole(Constants.TicketManagersRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
