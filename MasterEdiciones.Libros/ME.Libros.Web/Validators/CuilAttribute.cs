using System.ComponentModel.DataAnnotations;

namespace ME.Libros.Web.Validators
{
    public class CuilAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (ValidaCuil(value.ToString()))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(ErrorMessages.Invalido, validationContext.DisplayName));
        }

        private static bool ValidaCuil(string cuil)
        {
            cuil = cuil.Replace("-", string.Empty);
            if (cuil.Length != 11)
            {
                return false;
            }

            var calculado = CalcularDigitoCuil(cuil);
            var digito = int.Parse(cuil.Substring(10));
            return calculado == digito;
        }

        private static int CalcularDigitoCuil(string cuil)
        {
            var mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            var nums = cuil.ToCharArray();
            var total = 0;
            for (var i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }
    }
}