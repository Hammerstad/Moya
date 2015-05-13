//using System;
//using System.Linq;
//using System.Reflection;
//using Moya.Attributes;

//namespace Moya.Runner
//{
//    public class AttributeDiscoverer :IAttributeDiscoverer
//    {
//        public OldITestMetadataContainer TestRunner { get; private set; }

//        public AttributeDiscoverer(OldITestMetadataContainer testRunner)
//        {
//            TestRunner = testRunner;
//        }

//        public void DiscoverAttributesForClass(Type type)
//        {
//            MethodInfo[] methods = type.GetMethods();
//            foreach (var method in methods)
//            {
//                DiscoverAttributesForMethod(method, type);
//            }
//        }

//        public void DiscoverAttributesForMethod(MethodInfo method, Type type = null)
//        {
//            var attributes = Attribute.GetCustomAttributes(method);

//            foreach (IMoyaAttribute attribute in attributes.OfType<IMoyaAttribute>())
//            {
//                HandleAttribute(method, attribute, type);
//            }
//        }

//        private void HandleAttribute(MethodInfo method, IMoyaAttribute attribute, Type type = null)
//        {
//            Console.WriteLine(type);
//            type = type ?? method.GetType();
//            if (attribute is LoadTestAttribute)
//            {
//                var loadTestAttribute = (LoadTestAttribute)attribute;
//                TestRunner.RegisterAttributeFromMethod<int>(method, new AttributeMetaData
//                {
//                    AttributeName = "LoadTestAttribute",
//                    AttributeKey = "Runners",
//                    AttributeValue = loadTestAttribute.Runners,
//                    AttributeType = type
//                });
//                TestRunner.RegisterAttributeFromMethod<int>(method, new AttributeMetaData
//                {
//                    AttributeName = "LoadTestAttribute",
//                    AttributeKey = "Times",
//                    AttributeValue = loadTestAttribute.Times,
//                    AttributeType = type
//                });
//            }
//            else if (attribute is DurationShouldBeAttribute)
//            {
//                var durationAttribute = (DurationShouldBeAttribute)attribute;
//                TestRunner.RegisterAttributeFromMethod<int>(method, new AttributeMetaData
//                {
//                    AttributeName = "DurationShouldBeAttribute",
//                    AttributeKey = "LessThanOrEqualTo",
//                    AttributeValue = durationAttribute.LessThanOrEqualTo,
//                    AttributeType = type
//                });
//                TestRunner.RegisterAttributeFromMethod<int>(method, new AttributeMetaData
//                {
//                    AttributeName = "DurationShouldBeAttribute",
//                    AttributeKey = "GreaterThanOrEqualTo",
//                    AttributeValue = durationAttribute.GreaterThanOrEqualTo,
//                    AttributeType = type
//                });
//            }
//        }
//    }
//}