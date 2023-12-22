using BankMVC.Assemblers;
using BankMVC.Repository;
using BankMVC.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace BankMVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            //container.RegisterType<IUserRepository, UserRepository>();
            //container.RegisterType<IUserService, UserService>();
            //container.RegisterType<UserAssembler>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<UserAssembler>();
            container.RegisterType<ITransactionRepository, TransactionRepository>();
            container.RegisterType<ITransactionService, TransactionService>();
            container.RegisterType<TransactionAssembler>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<RoleAssembler>();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<ICustomerService, CustomerService>();
            container.RegisterType<CustomerAssembler>();
            container.RegisterType<IDocumentRepository, DocumentRepository>();
            container.RegisterType<IDocumentService, DocumentService>();
            container.RegisterType<DocumentAssembler>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<AccountAssembler>();
            container.RegisterType<IAccountTypeRepository, AccountTypeRepository>();
            container.RegisterType<IAccountTypeService, AccountTypeService>();
            container.RegisterType<AccountTypeAssembler>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}