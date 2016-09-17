using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Marten;
using AspNetCore.Identity.Marten.Internal;
using AspNetCore.Identity.Marten.MvcSample.Models;
using AspNetCore.Identity.Marten.MvcSample.Services;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNetCore.Identity.Marten.MvcSample
{
	public static class ApplicationServiceCollectionExtensions
	{
		public static IServiceCollection AddApplicationDocumentStore(this IServiceCollection services, IConfigurationRoot configuration)
		{
			var connectionString = configuration["Data:Store:ConnectionString"];
			var password = configuration["Data:Store:Password"];
			if (!String.IsNullOrWhiteSpace(connectionString) && connectionString.Contains("{{DATABASE_PASSWORD}}") && !String.IsNullOrWhiteSpace(password))
				connectionString = connectionString.Replace("{{DATABASE_PASSWORD}}", password);

			services.AddSingleton<IDocumentStore>(provider => DocumentStore.For(o =>
			{
				o.Connection(connectionString);

				o.Logger(new ConsoleMartenLogger());

				var optionsAccessor = provider.GetRequiredService<IOptions<IdentityOptions>>();
				o.ConfigureIdentityStoreOptions<ApplicationUser, Guid>(optionsAccessor.Value);
			}));

			return services;
		}

		public static IServiceCollection AddApplicationDocumentSession(this IServiceCollection services)
		{
			services.AddScoped<IDocumentSession>(p => p.GetRequiredService<IDocumentStore>().OpenSession());
			return services;
		}

		public static IServiceCollection AddApplicationSystemClock(this IServiceCollection services)
		{
			services.AddSingleton<ISystemClock>(new SystemClock());
			return services;
		}

		public static IdentityBuilder AddApplicationIdentity(this IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				// This is the Default value for ExternalCookieAuthenticationScheme
				options.SignInScheme = new IdentityCookieOptions().ExternalCookieAuthenticationScheme;
			});

			// Hosting doesn't add IHttpContextAccessor by default
			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			// Identity services
			services.TryAddSingleton<IdentityMarkerService>();
			services.TryAddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
			services.TryAddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
			services.TryAddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
			services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
			// No interface for the error describer so we can add errors without rev'ing the interface
			services.TryAddScoped<IdentityErrorDescriber>();
			services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
			services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();
			services.TryAddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
			services.TryAddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();

			return new IdentityBuilder(typeof(ApplicationUser), null, services);
		}
	}
}
