using Common.Settings;
using ErrorHandler;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using todo_service.Requests;

namespace todo_service.Validations
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        private readonly IErrorOperations _errorOperations;
        private readonly IOptions<APISettings> _APISettings;
        public CreateOrderRequestValidator(IErrorOperations errorOperations, IOptions<APISettings> APISettings)
        {
            _errorOperations = errorOperations ?? throw new ArgumentNullException(nameof(errorOperations));
            _APISettings = APISettings ?? throw new ArgumentNullException(nameof(APISettings));

            // Rules here
            RuleFor(x => x.OrderDetail.Price)
               .NotNull().WithMessage("OrderDetail.Price is mandatory.")
               .NotEmpty().WithMessage("OrderDetail.Price is mandatory.");
            //.WithMessage(_errorRepository.PrepareErrorText("26130", _distributedCache, _APISettings.Value.DCEnabled).Result);

            RuleFor(x => x.Name)
               .NotNull()
              .NotEmpty()
              .WithMessage("Address is mandatory.");

            RuleFor(x => x.Description)
              .NotNull()
              .NotEmpty()
              .WithMessage("Description is mandatory.");
        }
    }
}
