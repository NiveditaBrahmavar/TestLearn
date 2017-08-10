using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel;

namespace WebApplicationTest.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Address")]
        [MaxWordAttributes(10, ErrorMessage = "There are too many words in {0}.")]
        // [MaxWordAttributes(10, ErrorMessage = "There are too many words in {0}.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Enter Age")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Select gender")]
        public string Gender { get; set; }
        
        public bool IsGenderSelected { get; set; }

        //[Required(ErrorMessage = "Enter other Details")]
        [RequiredIf("Gender", ErrorMessage = "Enter other Details")]
        //[RequiredIf("UserType", UserType.Admin, ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(ResourceString))]
        public string OtherDetails { get; set; }
    }
    
    public class MaxWordAttributes : ValidationAttribute
    {
        private readonly int _maxwords;
        public MaxWordAttributes(int maxwords): base("{0} has too many words")
        {
            _maxwords = maxwords;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var textvalue = value.ToString();
            if (textvalue.Length <= _maxwords) return ValidationResult.Success;
            var errmessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errmessage);
        }
    }

    public class RequiredIfAttribute: ValidationAttribute
    {
        private string _propertyname;
        private object _propertyvalue;
       // private string _property;
        public RequiredIfAttribute(string propertyname)
        {
            _propertyname = propertyname;
         //   _propertyvalue = propertyvalue;
        //    _property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(_propertyname);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property {_propertyname}");
            }

            object othervalue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (othervalue.ToString() == "F")
            { // if other property has the configured value
              // PropertyInfo prop = validationContext.ObjectType.GetProperty(_propertyname);
              //ChangeableRequired attrib = (ChangeableRequired)prop.Attributes;

                return ValidationResult.Success;

            }

            var errmessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errmessage);

            // return null;
            // return base.IsValid(value, validationContext);
        }
    }

    public class ChangeableRequired : RequiredAttribute
    {
        public bool Disabled { get; set; }

        public override bool IsValid(object value)
        {
            if (Disabled)
            {
                return true;
            }

            return base.IsValid(value);
        }
    }




}