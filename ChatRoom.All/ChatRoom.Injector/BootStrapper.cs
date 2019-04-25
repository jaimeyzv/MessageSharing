using ChatRoom.Business;
using ChatRoom.Business.Interfaces;
using ChatRoom.DataAccess;
using ChatRoom.DataAccess.Interfaces;
using ChatRoom.DataAccess.Repositories;
using ChatRoom.Insfrastucture.Interfaces.Mappers;
using ChatRoom.Insfrastucture.Interfaces.Services;
using ChatRoom.Insfrastucture.Interfaces.Wrappers;
using ChatRoom.Insfrastucture.Mappers;
using ChatRoom.Insfrastucture.Services;
using ChatRoom.Insfrastucture.Wrappers;
using ChatRoom.MessageBroker;
using ChatRoom.MessageBroker.Interfaces;
using Unity;

namespace ChatRoom.Injector
{
    public class BootStrapper
    {
        public IUnityContainer RegisterComponents()
        {
            var container = new UnityContainer();
            RegisterInsfrastructure(container);
            RegisterRepositories(container);
            RegisterBusiness(container);
            RegisterMessageBroker(container);
            SvcLocator.Container = container;

            return container;
        }

        private void RegisterInsfrastructure(UnityContainer container)
        {
            container.RegisterType<IConfigurationManagerWrapper, ConfigurationManagerWrapper>();
            container.RegisterType<IConnectionStringProvider, ConnectionStringProvider>();
            container.RegisterType<IDbFactory, DbFactory>();
            container.RegisterType<IMapper, Mapper>();
        }

        private void RegisterRepositories(UnityContainer container)
        {
            container.RegisterType<IProfileRepository, ProfileRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IChatroomRepository, ChatroomRepository>();
            container.RegisterType<IMessageRepository, MessageRepository>();
        }

        private void RegisterBusiness(UnityContainer container)
        {
            container.RegisterType<IProfileBusiness, ProfileBusiness>();
            container.RegisterType<IUserBusiness, UserBusiness>();
            container.RegisterType<IChatroomBusiness, ChatroomBusiness>();
            container.RegisterType<IMessageBusiness, MessageBusiness>();
        }

        private void RegisterMessageBroker(UnityContainer container)
        {
            container.RegisterType<IQueueHandler, QueueHandler>();
        }
    }
}