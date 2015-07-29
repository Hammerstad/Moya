namespace Moya.IoC
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class Container : IContainer
    {
        private static readonly Lazy<Container> defaultInstance;

		private readonly Dictionary<Type, object> services;

		static Container() 
        {
			defaultInstance = new Lazy<Container>(() => new Container());
		}

        private Container()
        {
            services = new Dictionary<Type, object>();
		}

        public static Container DefaultInstance
        {
            get
            {
                return defaultInstance.Value;
            }
        }

        public TClass Register<TClass>(TClass element) where TClass : class 
        {
			Register<TClass, TClass>(element);
		    return element;
        }

        public TClass Register<TInterface, TClass>(TClass element) where TClass : class, TInterface 
        {
            if (Contains<TInterface>())
            {
                throw new ContainerException(String.Format("Container already contains {0}", typeof(TInterface).FullName));
            }

			services.Add(typeof(TInterface), element);
            return element;
        }

		public TInterface Resolve<TInterface>()
        {
			return (TInterface)services[typeof(TInterface)];
		}

        public bool Contains<TInterface>()
        {
            return services.ContainsKey(typeof(TInterface));
        }

        public TClass TryRegister<TInterface, TClass>(TClass element) where TClass : class, TInterface
        {
            if (Contains<TInterface>())
            {
                return (TClass)Resolve<TInterface>();
            }

            Register<TInterface, TClass>(element);
            return (TClass)Resolve<TInterface>();
        }
    }
}