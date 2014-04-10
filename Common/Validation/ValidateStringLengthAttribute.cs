using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Common.Validation
{
    enum Error
    {
        StringIsTooShort,
        StringIsTooLong,
        StringIsNull,
        NoError
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateStringLengthAttribute : ValidationAttribute
    {
        #region Constants

        private const string DefaultErrorMessage = "Incorrect value";
        private const string ShortStringErrorMessage = "'{0}' must be at least {1} characters long.";
        private const string LongStringErrorMessage = "'{0}' must be maximum {1} characters long.";

        #endregion

        #region Private members

        private Error error;
        private int minCharacters;
        private int maxCharacters;
        private bool allowNull;

        #endregion

        #region Constructor

        public ValidateStringLengthAttribute(int minCharacters, int maxCharacters, bool allowNull)
            : base(DefaultErrorMessage)
        {
            this.minCharacters = minCharacters;
            this.maxCharacters = maxCharacters;
            this.allowNull = allowNull;
        }

        #endregion

        #region Overriden properties

        public override string FormatErrorMessage(string name)
        {
            if (error == Error.StringIsTooShort)
                return String.Format(CultureInfo.CurrentUICulture, ShortStringErrorMessage, name, minCharacters);

            if (error == Error.StringIsTooLong)
                return String.Format(CultureInfo.CurrentUICulture, LongStringErrorMessage, name, maxCharacters);

            if (error == Error.StringIsNull)
                return DefaultErrorMessage;

            return String.Empty;
        }

        public override bool IsValid(object value)
        {
            String propertyValue = value as String;

            if (propertyValue == null)
            {
                this.error = (allowNull) ? Error.NoError : Error.StringIsNull;
                return allowNull;
            }

            if (propertyValue.Length < this.minCharacters)
            {
                this.error = Error.StringIsTooShort;
                return false;
            }

            if (propertyValue.Length > this.maxCharacters)
            {
                this.error = Error.StringIsTooLong;
                return false;
            }

            this.error = Error.NoError;
            return true;
        }

        #endregion
    }
}