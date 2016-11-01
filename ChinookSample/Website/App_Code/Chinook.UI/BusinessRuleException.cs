using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Going to be used in the BLL. Going to store business rule violations 
/// the contents that will be placed in an instance of this class will come from user code within your application source (ie BLL excetion rule exception)
/// Going to be used by the user cntrol to display the message to the user, kind of like a DTO class
/// </summary>
[Serializable]
public class BusinessRuleException : Exception
{
    public List<string> RuleDetails { get; set; }
    public BusinessRuleException(string message, List<string> reasons)
        : base(message)
    {
        this.RuleDetails = reasons;
    }
}