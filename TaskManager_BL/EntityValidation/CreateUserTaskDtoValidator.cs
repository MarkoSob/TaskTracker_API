using FluentValidation;
using TaskTracker_BL.DTOs;

namespace TaskTracker_BL.EntityValidation
{
    public class CreateUserTaskDtoValidator : AbstractValidator<CreateUserTaskDto>
    {
        public CreateUserTaskDtoValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty()
                .WithMessage("Enter the title please");

            RuleFor(task => task.StartDate)
                .NotEmpty()
                .WithMessage("Enter the start date please");

            RuleFor(task => task.EndDate)
                .NotEmpty()
                .WithMessage("Enter the end date please");
        }
    }
}
