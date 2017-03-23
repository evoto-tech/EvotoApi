using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Models
{
    public enum EUserFieldType
    {
        String,
        Number,
        Email,
        Date
    }

    public abstract class CustomUserField
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EUserFieldType Type { get; set; }

        public bool Required { get; set; }

        public abstract object GetValidationProperties();

        public abstract void SetValidationProperties(dynamic props);
    }

    public abstract class CustomUserField<T> : CustomUserField
    {
        public abstract bool IsValid(T value);

        public abstract bool IsValid(T value, out List<string> errors);
    }

    public class CustomUserField_Date : CustomUserField<DateTime>
    {
        public DateTime? MaxDate { get; set; } = new DateTime?();
        public DateTime? MinDate { get; set; } = new DateTime?();

        public override bool IsValid(DateTime value)
        {
            List<string> e;
            return IsValid(value, false, out e);
        }

        public override bool IsValid(DateTime value, out List<string> errors)
        {
            return IsValid(value, true, out errors);
        }

        private bool IsValid(DateTime value, bool showErrors, out List<string> errors)
        {
            errors = new List<string>();

            if (MinDate.HasValue && (value < MinDate.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Min Date: {MinDate.Value}, Provided Date: {value}");
            }

            if (MaxDate.HasValue && (value > MaxDate.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Max Date: {MaxDate.Value}, Provided Date: {value}");
            }

            return !errors.Any();
        }

        public override object GetValidationProperties()
        {
            return new
            {
                MaxDate = MaxDate.ToString(),
                MinDate = MinDate.ToString()
            };
        }

        public override void SetValidationProperties(dynamic props)
        {
            MaxDate = props?.MaxDate;
            MinDate = props?.MinDate;
        }
    }

    public class CustomUserField_Email : CustomUserField<string>
    {
        public override bool IsValid(string value)
        {
            List<string> e;
            return IsValid(value, false, out e);
        }

        public override bool IsValid(string value, out List<string> errors)
        {
            return IsValid(value, true, out errors);
        }

        private static bool IsValid(string value, bool showErrors, out List<string> errors)
        {
            errors = new List<string>();

            var attr = new EmailAddressAttribute();
            if (!attr.IsValid(value))
            {
                if (!showErrors)
                    return false;

                errors.Add("Invalid Email Address");
            }

            return !errors.Any();
        }

        public override object GetValidationProperties()
        {
            return new {};
        }

        public override void SetValidationProperties(dynamic props)
        {
        }
    }

    public class CustomUserField_Number : CustomUserField<double>
    {
        public double? Max { get; set; } = new double?();
        public double? Min { get; set; } = new double?();

        public override bool IsValid(double value)
        {
            List<string> e;
            return IsValid(value, false, out e);
        }

        public override bool IsValid(double value, out List<string> errors)
        {
            return IsValid(value, true, out errors);
        }

        private bool IsValid(double value, bool showErrors, out List<string> errors)
        {
            errors = new List<string>();

            if (Min.HasValue && (value < Min.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Min Value: {Min.Value}, Actual Value: {value}");
            }

            if (Max.HasValue && (value > Max.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Max Value: {Max.Value}, Actual Value: {value}");
            }

            return !errors.Any();
        }

        public override object GetValidationProperties()
        {
            return new
            {
                Max = Max.ToString(),
                Min = Min.ToString()
            };
        }

        public override void SetValidationProperties(dynamic props)
        {
            Max = props?.Max;
            Min = props?.Min;
        }
    }

    public class CustomUserField_String : CustomUserField<string>
    {
        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }
        public Regex Regex { get; set; }

        public override bool IsValid(string value)
        {
            List<string> e;
            return IsValid(value, false, out e);
        }

        public override bool IsValid(string value, out List<string> errors)
        {
            return IsValid(value, true, out errors);
        }

        private bool IsValid(string value, bool showErrors, out List<string> errors)
        {
            errors = new List<string>();

            if (MinLength.HasValue && (value.Length < MinLength.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Min Length: {MinLength.Value}, Given Length: {value.Length}");
            }

            if (MaxLength.HasValue && (value.Length > MaxLength.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Max Length: {MaxLength.Value}, Given Length: {value.Length}");
            }

            if ((Regex != null) && !Regex.IsMatch(value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"{value} does not match the required pattern");
            }

            return !errors.Any();
        }

        public override object GetValidationProperties()
        {
            return new
            {
                MaxLength = MaxLength?.ToString(),
                MinLength = MinLength?.ToString(),
                Regex = Regex?.ToString()
            };
        }

        public override void SetValidationProperties(dynamic props)
        {
            MaxLength = props?.MaxLength;
            MinLength = props?.MinLength;
            Regex = props?.Regex;
        }
    }
}