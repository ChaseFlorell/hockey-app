using HockeyApp.Modules;

namespace HockeyApp
{
    public class ChainBuilder
    {
        private IModule _module;

        public ChainBuilder AddModule<TModule>() where TModule : IModule, new()
        {
            var module = new TModule();
            if (_module == null)
            {
                _module = module;
                return this;
            }

            SetLastSuccessor(_module, module);

            return this;
        }

        private void SetLastSuccessor(IModule module, IModule successor)
        {
            while (true)
            {
                if (module.Successor == null)
                {
                    module.SetSuccessor(successor);
                }
                else
                {
                    var module1 = module;
                    module = module.Successor;
                    successor = module1;
                    continue;
                }
                break;
            }
        }

        public IModule Build()
        {
            return _module;
        }
    }
}