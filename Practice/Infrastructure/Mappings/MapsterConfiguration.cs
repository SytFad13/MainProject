using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PersonManagement.Domain.POCO;
using PersonManagement.MVC.ViewModels;

namespace PersonManagement.MVC.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
			TypeAdapterConfig<AcademyEvent, AcademyEventViewModel>
			.NewConfig()
			.TwoWays();

			TypeAdapterConfig<IdentityUser, UserViewModel>
			.NewConfig()
			.TwoWays();
		}
    }
}
