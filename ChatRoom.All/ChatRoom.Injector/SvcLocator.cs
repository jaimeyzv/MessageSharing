using Unity;
using Unity.Resolution;

namespace ChatRoom.Injector
{
    public static class SvcLocator
    {
        public static UnityContainer Container { get; set; }

        public static T GetInstance<T>()
        {
            return Container.Resolve<T>();
        }

        public static T GetInstance<T>(ParameterOverride p)
        {
            return Container.Resolve<T>(p);
        }

        public static T GetInstance<T>(ParameterOverrides p)
        {
            return Container.Resolve<T>(p);
        }

        public static T GetInstance<T>(string name)
        {
            return Container.Resolve<T>(name);
        }
    }
}