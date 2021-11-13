using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validator)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator.Any()) return await next().ConfigureAwait(false);

            var context = new ValidationContext<TRequest>(request);

            var validationResult = await Task.WhenAll(_validator.Select(v => v.ValidateAsync(context, cancellationToken))).ConfigureAwait(false);

            var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Count > 0)
            {
                throw new ValidationException(failures);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
