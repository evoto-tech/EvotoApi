using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Registrar.Models
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

        public static CustomUserField GetFieldForType(EUserFieldType type)
        {
            var fieldType = typeof(CustomUserField);
            var fieldName = fieldType.Namespace + "." + fieldType.Name + "_" + type;
            var fieldClass = typeof(CustomUserField).Assembly.GetType(fieldName);
            var constructor = fieldClass.GetConstructor(new Type[] {});
            if (constructor == null)
                return null;

            return (CustomUserField) constructor.Invoke(new object[] {});
        }

        public abstract object GetValidationProperties();

        public abstract bool SetValidationProperties(dynamic props);

        public abstract bool IsValid(string value);

        public abstract bool IsValid(string value, out List<string> errors);
    }

    public class CustomUserField_Date : CustomUserField
    {
        public DateTime? MaxDate { get; set; } = new DateTime?();
        public DateTime? MinDate { get; set; } = new DateTime?();

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

            DateTime date;
            if (!DateTime.TryParse(value, out date))
            {
                errors.Add("Invalid Date string");
                return false;
            }

            if (MinDate.HasValue && (date < MinDate.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Min Date: {MinDate.Value}, Provided Date: {value}");
            }

            if (MaxDate.HasValue && (date > MaxDate.Value))
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
                MaxDate,
                MinDate
            };
        }

        public override bool SetValidationProperties(dynamic props)
        {
            if (props == null)
                return true;

            DateTime date;
            if (props.MaxDate != null && !string.IsNullOrWhiteSpace(props.MaxDate.ToString()))
            {
                if (DateTime.TryParse(props.MaxDate.ToString(), out date))
                    MaxDate = date;
                else return false;
            }
            if (props.MinDate != null && !string.IsNullOrWhiteSpace(props.MinDate.ToString()))
            {
                if (DateTime.TryParse(props.MinDate.ToString(), out date))
                    MinDate = date;
                else return false;
            }

            return true;
        }
    }

    public class CustomUserField_Email : CustomUserField
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

        public override bool SetValidationProperties(dynamic props)
        {
            return true;
        }
    }

    public class CustomUserField_Number : CustomUserField
    {
        public double? Max { get; set; } = new double?();
        public double? Min { get; set; } = new double?();

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

            double number;
            if (!double.TryParse(value, out number))
            {
                errors.Add("Invalid number string");
                return false;
            }

            if (Min.HasValue && (number < Min.Value))
            {
                if (!showErrors)
                    return false;

                errors.Add($"Min Value: {Min.Value}, Actual Value: {value}");
            }

            if (Max.HasValue && (number > Max.Value))
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
                Max,
                Min
            };
        }

        public override bool SetValidationProperties(dynamic props)
        {
            if (props == null)
                return true;

            double num;
            if (props.Max != null && string.IsNullOrWhiteSpace(props.Max.ToString()))
            {
                if (double.TryParse(props.Max.ToString(), out num))
                    Max = num;
                else return false;
            }
            if (props.Min != null && string.IsNullOrWhiteSpace(props.Min.ToString()))
            {
                if (double.TryParse(props.Min.ToString(), out num))
                    Min = num;
                else return false;
            }

            return true;
        }
    }

    public class CustomUserField_String : CustomUserField
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
                MaxLength,
                MinLength,
                Regex
            };
        }

        public override bool SetValidationProperties(dynamic props)
        {
            if (props == null)
                return true;

            int i;
            if (props.MaxLength != null && !string.IsNullOrWhiteSpace(props.MaxLength.ToString()))
            {
                if (int.TryParse(props.MaxLength.ToString(), out i))
                    MaxLength = i;
                else return false;
            }
            if (props.MinLength != null && !string.IsNullOrWhiteSpace(props.MinLength.ToString()))
            {
                if (int.TryParse(props.MinLength.ToString(), out i))
                    MinLength = i;
                else return false;
            }

            // Min > Max is invalid
            if (MinLength.HasValue && MaxLength.HasValue && MinLength.Value > MaxLength.Value)
                return false;

            try
            {
                if (props.Regex != null && string.IsNullOrWhiteSpace(props.Regex.ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(props.Regex.ToString()))
                        Regex = new Regex(props.Regex.ToString());
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}