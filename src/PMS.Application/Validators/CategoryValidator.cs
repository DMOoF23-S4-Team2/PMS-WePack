using FluentValidation;
using PMS.Core.Entities;

namespace PMS.Application.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(5000);

            RuleFor(x => x.BottomDescription)
                .MaximumLength(65535);

            RuleForEach(x => x.Products)
                .SetValidator(new ProductValidator());
        }
    }
}