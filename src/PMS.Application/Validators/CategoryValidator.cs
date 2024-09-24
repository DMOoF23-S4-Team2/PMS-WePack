using FluentValidation;
using PMS.Core.Entities;

namespace PMS.Application.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(255).WithMessage("Category name must not exceed 255 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Category description is required")
                .MaximumLength(5000).WithMessage("Category description must not exceed 5000 characters");

            RuleFor(x => x.BottomDescription)
                .MaximumLength(65535).WithMessage("Bottom description must not exceed 65535 characters");

            RuleForEach(x => x.Products)
                .SetValidator(new ProductValidator());
        }
    }
}