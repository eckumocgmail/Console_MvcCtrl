using API;

using eckumoc_common_api.CommonExtensionsTest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
public class ExtensionsUnit : TestingUnit
{
    public ExtensionsUnit()
    {
        
        Push(new TypeExtensionsTest());
        Push(new ActionExtensionsTest());
        Push(new AssemblyExtensionsTest());
        Push(new CollectionsExtensionsTest());
        Push(new CollectionsIQuerableExtensionsTest());
        Push(new SuperDbSetExtensionsTest());
        Push(new HaffmanAlgExtensionsTest());
        Push(new CollectionsExtensionsTest());

    }
}
namespace eckumoc_common_api.CommonExtensionsTest
{



    public class ActionExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class AssemblyExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            var executing = System.Reflection.Assembly.GetExecutingAssembly();

            new CommonHttp.CommandLine.CommandLineAssembly(executing);
            throw new NotImplementedException();
        }
    }
    public class CollectionsExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class CollectionsIQuerableExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class SuperDbSetExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class HaffmanAlgExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class IntRandomExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class InvokeHelperTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class MiddlewareExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class ModelBuilderExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class ObjectCompileExpExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class ObjectInputExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class RandomExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextConvertExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextCountingExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextEncodingExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextFactoryExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextHttpExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextIntExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextIOExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextLangExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    
    public class TextNamingExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextParserExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TextTypeExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class ThrowableExtensionsTest : TestingElement{
        protected override void OnTest()
        {
            throw new NotImplementedException();
        }
    }
    public class TypeAttributesExtensionTest : TestingElement{
        protected override void OnTest()
        {
           
        }
    }
    
    public class TypeExtensionsTest : TestingElement{
        protected override void OnTest()
        {

             

            //typeof(ICommonDictionary<ICollection<int>>)
                //.GetDeclaration().
            //TypeExtensions.IsExtendsFrom()
        }
    }
}
