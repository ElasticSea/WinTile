using System.Globalization;
using System.Windows.Controls;
using ElasticSea.Wintile.Utils;

namespace ElasticSea.Wintile.View.Rules
{
    public class IntegerValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var integer = (value as string).ToInt(0);
            if (integer < Min) return new ValidationResult(false, $"Number is smaller than {Min}");
            if (integer > Max) return new ValidationResult(false, $"Number is larger than {Max}");
            return new ValidationResult(true, null);
        }
    }
}