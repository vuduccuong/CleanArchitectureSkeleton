using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApplicationDI).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationDI).Assembly);
            services.AddMediatR(typeof(ApplicationDI).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnHandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerfomanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));

            return services;
        }
    }
}
