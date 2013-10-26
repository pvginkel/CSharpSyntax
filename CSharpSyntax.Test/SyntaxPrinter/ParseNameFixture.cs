using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinter
{
    [TestFixture]
    public class ParseNameFixture : TestBase
    {
        [TestCase("Class")]
        [TestCase("経済協力開発機構")]
        [TestCase("Sébastien")]
        public void Identifier(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase(" Class ", "Class")]
        [TestCase("\tClass ", "Class")]
        [TestCase("\r\nClass ", "Class")]
        public void WhiteSpace(string code, string expected)
        {
            TestAndCompare(expected, code);
        }

        [TestCase("1")]
        [TestCase("~")]
        [TestCase("a::")]
        [TestCase("::b")]
        [ExpectedException]
        public void InvalidIdentifier(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("Class Class")]
        [ExpectedException]
        public void TrailingCode(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("AClass<BClass>")]
        [TestCase("AClass<BClass, CClass>")]
        [TestCase("AClass<BClass, CClass, EClass>")]
        [TestCase("AClass<BClass<>, CClass, EClass>")]
        [TestCase("AClass<BClass<FClass>, CClass, EClass>")]
        [TestCase("AClass<>")]
        [TestCase("AClass<, >")]
        [TestCase("AClass<, , >")]
        public void Generic(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("a::Class")]
        [TestCase("global::Class")]
        [TestCase("global::Namespace.Class")]
        public void AliasQualified(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("Namespace.Class")]
        [TestCase("Namespace1.Namespace2.Class")]
        public void Qualified(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("b::Namespace.BClass")]
        [TestCase("b::Namespace1.Namespace2.BClass")]
        [TestCase("b::Namespace1.Namespace2.Namespace3.BClass")]
        [TestCase("a::AClass<b::Namespace.BClass<>>")]
        [TestCase("a::c<b::d.e<>>")]
        public void Combinations(string code)
        {
            TestAndCompare(code, code);
        }

        [Test]
        public void Var()
        {
            var parsed = Syntax.ParseName("var");

            Assert.IsInstanceOf<IdentifierNameSyntax>(parsed);
            Assert.IsTrue(parsed.IsVar);

            Test("var", parsed);
        }

        [TestCase("bool", PredefinedType.Bool)]
        [TestCase("byte", PredefinedType.Byte)]
        [TestCase("sbyte", PredefinedType.SByte)]
        [TestCase("short", PredefinedType.Short)]
        [TestCase("ushort", PredefinedType.UShort)]
        [TestCase("int", PredefinedType.Int)]
        [TestCase("uint", PredefinedType.UInt)]
        [TestCase("long", PredefinedType.Long)]
        [TestCase("ulong", PredefinedType.ULong)]
        [TestCase("double", PredefinedType.Double)]
        [TestCase("float", PredefinedType.Float)]
        [TestCase("decimal", PredefinedType.Decimal)]
        [TestCase("string", PredefinedType.String)]
        [TestCase("char", PredefinedType.Char)]
        [TestCase("void", PredefinedType.Void)]
        [TestCase("object", PredefinedType.Object)]
        public void Predefined(string code, PredefinedType predefinedType)
        {
            var parsed = Syntax.ParseName(code);

            Assert.IsInstanceOf<PredefinedTypeSyntax>(parsed);
            Assert.AreEqual(predefinedType, ((PredefinedTypeSyntax)parsed).Type);

            Test(code, parsed);
        }

        [TestCase("int[]")]
        [TestCase("int[,]")]
        [TestCase("int[,,]")]
        [TestCase("Class<int[]>")]
        [TestCase("int[][]")]
        [TestCase("int[,][,]")]
        [TestCase("int[,,][,,]")]
        [TestCase("Class<int[][]>")]
        [TestCase("int[][][]")]
        [TestCase("int[,][,][,]")]
        [TestCase("int[,,][,,][,,]")]
        [TestCase("Class<int[][][]>")]
        [TestCase("Class<int>[]")]
        [TestCase("Class<int>[,]")]
        [TestCase("global::Class[]")]
        [TestCase("global::Namespace.Class[]")]
        [TestCase("Class<global::Class>[,]")]
        [TestCase("Namespace.Class<global::Class>[,]")]
        public void ArrayTypes(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("int?")]
        [TestCase("Class?")]
        [TestCase("Class<>?")]
        [TestCase("int?[]")]
        public void Nullable(string code)
        {
            TestAndCompare(code, code);
        }

        [TestCase("int[]?")]
        [ExpectedException]
        public void InvalidNullable(string code)
        {
            TestAndCompare(code, code);
        }

        private void TestAndCompare(string expected, string code)
        {
            Test(expected, Syntax.ParseName(code));
        }
    }
}
