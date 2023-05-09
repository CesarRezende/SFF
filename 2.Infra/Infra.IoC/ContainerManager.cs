using DryIoc;


namespace Infra.IoC
{
    public static class ContainerManager
    {
        private static IContainer _container;

        public static IContainer CreateContainer()
        {
            _container = new Container(rules =>

                rules.With(FactoryMethod.ConstructorWithResolvableArguments)
                     .WithFactorySelector(Rules.SelectLastRegisteredFactory())
                     .WithoutThrowIfDependencyHasShorterReuseLifespan(),
                     scopeContext: new AsyncExecutionFlowScopeContext()
            );

            return _container;
        }

        public static IContainer GetContainer()
        {
            return _container;
        }

        public static void SetContainer(IContainer container)
        {
            _container = container;
        }
    }
}