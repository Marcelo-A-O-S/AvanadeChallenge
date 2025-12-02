using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SaleService.Application.Validators
{
    public class MinCollectionCountAttribute : ValidationAttribute
    {
        private readonly int _minCount;

        public MinCollectionCountAttribute(int minCount)
        {
            _minCount = minCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ICollection collection)
            {
                if (collection.Count < _minCount)
                {
                    return new ValidationResult(
                        ErrorMessage ?? $"A coleção deve conter pelo menos {_minCount} item(ns).");
                }
                return ValidationResult.Success!;
            }

            // Caso o valor não seja uma coleção, é inválido
            return new ValidationResult("O valor informado não é uma coleção válida.");
        }
    }
}