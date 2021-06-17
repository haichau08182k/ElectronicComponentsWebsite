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
            container.RegisterType<IDiscountCodeService, DiscountCodeService>();
            container.RegisterType<IDiscountCodeDetailService, DiscountCodeDetailService>();
            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IQAService, QAService>();
            container.RegisterType<IEmployeeTypeService, EmloyeeTypeService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<IAccessTimesCountService, AccessTimesCountService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<IImportCouponService, ImportCouponService>();
            container.RegisterType<IImportCouponDetailService, ImportCouponDetailService>();
           
            //container.RegisterType<IRatingService, RatingService>();
            container.RegisterType<IDecentralizationService, DecentralizationService>();
            container.RegisterType<IDiscountCodeService, DiscountCodeService>();
            container.RegisterType<IDiscountCodeDetailService, DiscountCodeDetailService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IOrderDetailService, OrderDetailService>();
            container.RegisterType<IRatingService, RatingService>();
            container.RegisterType<ISupplierService, SupplierService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}