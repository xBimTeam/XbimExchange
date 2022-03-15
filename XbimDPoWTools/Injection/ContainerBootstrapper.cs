using Unity;
using Xbim.WindowsUI.DPoWValidation.ViewModels;
using XbimDPoWTools;

namespace Xbim.WindowsUI.DPoWValidation.Injection
{
    internal class ContainerBootstrapper
    {
        private static ContainerBootstrapper _instance;

        private IUnityContainer _container;

        private ContainerBootstrapper()
        {
            _container = new UnityContainer();
            _container.RegisterType<ISaveFileSelector, SaveFileSelector>();
            _container.RegisterType<VerificationViewModel, VerificationViewModel>();
            _container.RegisterType<ICanShow, MainWindow>();
        }

        public IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }


        public static ContainerBootstrapper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ContainerBootstrapper();
                }
                return _instance;
            }
        }
    }
}