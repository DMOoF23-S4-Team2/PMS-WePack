using FluentValidation;
using PMS.Core.Entities;

namespace PMS.Application.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.ShopifyId)
                .MaximumLength(255);
                
            RuleFor(product => product.Sku)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(product => product.Ean)
                .MaximumLength(255);

            RuleFor(product => product.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(product => product.Description)
                .MaximumLength(5000);

            RuleFor(product => product.Color)
                .MaximumLength(255);

            RuleFor(product => product.Material);

            RuleFor(product => product.ProductType)
                .MaximumLength(255);

            RuleFor(product => product.ProductGroup)
                .MaximumLength(255);

            RuleFor(product => product.Currency)
                .MaximumLength(10);

            RuleFor(product => product.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a positive number");

            RuleFor(product => product.SpecialPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Special Price must be a positive number");
        }
    }
}