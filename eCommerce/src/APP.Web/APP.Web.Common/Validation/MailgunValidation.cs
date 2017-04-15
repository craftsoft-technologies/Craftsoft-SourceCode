using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APP.Web.Common.Validation
{
    public class MailgunValidation : ValidationAttribute, IClientValidatable
    {
        public MailgunValidation() : base()
        {

        }

        public override bool IsValid(object value)
        {
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //Here we will set validation rule for client side validation
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = APP.Web.Common.Properties.ErrorMessage.EMAIL_INVALID, //FormatErrorMessage(metadata.DisplayName),
                ValidationType = "mailgunvalidation"     // must be all in lower case, spacial char not allowed in the validation type name
            };
            rule.ValidationParameters["email"] = "";
            yield return rule;
        }
    }
}
