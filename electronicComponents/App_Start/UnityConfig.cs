using electronicComponents.Service;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace electronicComponents
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<IMemberService, MemberService>();
            container.RegisterType<ICartService, CartService>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}