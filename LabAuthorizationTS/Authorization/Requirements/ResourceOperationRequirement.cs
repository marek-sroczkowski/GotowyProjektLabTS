using LabAuthorizationTS.Authorization.Utils;
using Microsoft.AspNetCore.Authorization;

namespace LabAuthorizationTS.Authorization.Requirements
{
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public OperationType OperationType { get; }

        public ResourceOperationRequirement(OperationType operationType)
        {
            OperationType = operationType;
        }
    }
}