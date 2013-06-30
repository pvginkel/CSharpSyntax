using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Roslyn.Compilers.CSharp;

namespace CSharpSyntax.Generate
{
    internal class Generator
    {
        public static readonly HashSet<Type> IgnoredTypes = new HashSet<Type>(new[]
        {
            typeof(BadDirectiveTriviaSyntax),
            typeof(SkippedTokensTriviaSyntax),
            //typeof(BracketedArgumentListSyntax),
            typeof(PointerTypeSyntax),
            typeof(UnsafeStatementSyntax),
            typeof(FixedStatementSyntax),
            typeof(AttributeTargetSpecifierSyntax), // Replaced with an enum
            typeof(GlobalStatementSyntax),
            typeof(IncompleteMemberSyntax),
            typeof(MakeRefExpressionSyntax),
            typeof(RefValueExpressionSyntax),
            typeof(RefTypeExpressionSyntax),
            typeof(StackAllocArrayCreationExpressionSyntax)
        });

        private static readonly HashSet<Type> _anonymousMethodTypes = new HashSet<Type>(new[]
        {
            typeof(AnonymousMethodExpressionSyntax),
            typeof(SimpleLambdaExpressionSyntax),
            typeof(ParenthesizedLambdaExpressionSyntax)
        });

        private static readonly HashSet<Type> _allowTriviaTypes = new HashSet<Type>(new[]
        {
            typeof(MemberDeclarationSyntax),
            typeof(StatementSyntax),
            typeof(AccessorDeclarationSyntax),
            typeof(CompilationUnitSyntax),
            typeof(ExternAliasDirectiveSyntax),
            typeof(UsingDirectiveSyntax),
            typeof(SwitchSectionSyntax)
        });

        private static SyntaxType _awaitType;

        public void Generate(string target)
        {
            var enums = new EnumDeclarationCollection();
            var nonAbstractTypes = new HashSet<string>();

            var factories = GetFactories();
            var writtenTypes = new HashSet<SyntaxType>();

            foreach (var type in GetTypes())
            {
                if (!type.IsAbstract)
                    nonAbstractTypes.Add(type.Name);

                var builder = new CodeFragmentBuilder();
                var initializeLists = new Dictionary<string, string>();
                var getChildrenGenerators = new List<IGetChildrenGenerator>();

                builder.AppendLine("using System;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using System.Linq;");
                builder.AppendLine("using System.Text;");

                // We only need this when we're implementing an Accept.
                if (!type.IsAbstract)
                    builder.AppendLine("using System.Diagnostics;");

                builder.AppendLine();
                builder.AppendLine("namespace CSharpSyntax");
                builder.AppendLine("{");
                builder.Indent();

                string baseType = type.BaseType.Name;

                if (_allowTriviaTypes.Any(p => p.Name == type.Name))
                {
                    Debug.Assert(baseType == "SyntaxNode");

                    baseType = "SyntaxTriviaNode";
                }

                builder.AppendLine("public {0} partial class {1} : {2}", type.IsAbstract ? "abstract" : "sealed", type.Name, baseType);
                builder.AppendLine("{");
                builder.Indent();

                foreach (var property in
                    from property in type.Properties
                    where property.DeclaringType == type
                    orderby property.Name
                    select property)
                {
                    if (
                        type.BaseType.Properties.Contains(property.Name) ||
                        property.Name == "ParentTrivia" ||
                        property.Name == "AllowsAnyExpression" ||
                        property.Name == "IsConst" ||
                        property.Name == "IsFixed"
                    )
                        continue;

                    // These are not supported because they are associated with errors.

                    //if (typeof(BracketedArgumentListSyntax).IsAssignableFrom(property.PropertyType))
                    //    continue;

                    string propertyType = CodeUtil.Encode(property.PropertyType);
                    string propertyName = property.Name;

                    // These are hand coded to keep the complexity of the generator down.

                    if (new[] { "Arity", "IsVar", "Rank" }.Contains(property.Name))
                        continue;

                    if (property.Name == "Target")
                    {
                        propertyType = "AttributeTarget";
                        enums.Add(new EnumDeclaration(
                            propertyType,
                            new[] { "None", "Assembly", "Field", "Event", "Method", "Module", "Param", "Property", "Return", "Type" }
                        ));
                    }
                    else if (
                        property.PropertyType == typeof(SyntaxTrivia) ||
                        property.PropertyType == typeof(SyntaxToken) ||
                        property.PropertyType == typeof(SyntaxTokenList)
                    )
                    {
                        if (property.Name == "TextTokens")
                        {
                            propertyName = "Text";
                            propertyType = "string";
                        }
                        else if (property.Name == "Modifiers")
                        {
                            if (type.Name == "ParameterSyntax")
                            {
                                propertyType = "ParameterModifier";
                                propertyName = "Modifier";
                            }
                            else
                            {
                                propertyType = "Modifiers";
                                if (!enums.Contains(propertyType))
                                {
                                    enums.Add(new FlagsEnumDeclaration(
                                        propertyType,
                                        new[] { "Async", "Abstract", "Const", "Extern", "Internal", "New", "Override", "Partial", "Private", "Protected", "Public", "ReadOnly", "Sealed", "Static", "Virtual", "Volatile" },
                                        true
                                    ));
                                }
                            }
                        }
                        else if (property.Name == "Keyword")
                        {
                            propertyType = null;
                            var items = new List<string>();

                            switch (type.Name)
                            {
                                case "PredefinedTypeSyntax":
                                    propertyName = "Type";
                                    propertyType = "PredefinedType";
                                    items.AddRange(new[] { "Bool", "Byte", "SByte", "Short", "UShort", "Int", "UInt", "Long", "ULong", "Double", "Float", "Decimal", "String", "Char", "Void", "Object" });
                                    break;

                                case "TypeDeclarationSyntax":
                                case "MakeRefExpressionSyntax":
                                case "RefTypeExpressionSyntax":
                                case "RefValueExpressionSyntax":
                                case "DefaultExpressionSyntax":
                                case "TypeOfExpressionSyntax":
                                case "SizeOfExpressionSyntax":
                                    break;

                                case "CheckedExpressionSyntax":
                                case "CheckedStatementSyntax":
                                    propertyName = "Kind";
                                    propertyType = "CheckedOrUnchecked";
                                    items.AddRange(new[] { "Checked", "Unchecked" });
                                    break;

                                case "AccessorDeclarationSyntax":
                                    propertyName = "Kind";
                                    propertyType = "AccessorDeclarationKind";
                                    items.AddRange(new[] { "Get", "Set", "Add", "Remove" });
                                    break;

                                default:
                                    throw new NotSupportedException();
                            }

                            if (propertyType == null)
                                continue;
                            else if (!enums.Contains(propertyType))
                                enums.Add(new EnumDeclaration(propertyType, items.ToArray()));
                        }
                        else if (property.Name == "OperatorToken")
                        {
                            propertyType = null;
                            propertyName = "Operator";
                            var items = new List<string>();

                            if (type.Name == "OperatorDeclarationSyntax")
                            {
                                propertyType = "Operator";
                                items.AddRange(new[] { "Plus", "Minus", "Exclamation", "Tilde", "PlusPlus", "MinusMinus", "Asterisk", "Slash", "Percent", "LessThanLessThan", "GreaterThanGreaterThan", "Bar", "Ampersand", "Caret", "EqualsEquals", "ExclamationEquals", "LessThan", "LessThanEquals", "GreaterThan", "GreaterThanEquals", "False", "True" });
                            }
                            else
                            {
                                switch (type.Name)
                                {
                                    case "BinaryExpressionSyntax":
                                        propertyType = "BinaryOperator";
                                        items.AddRange(new[] { "Percent", "Caret", "Ampersand", "Asterisk", "Minus", "Plus", "Equals", "Bar", "LessThan", "GreaterThan", "Slash", "BarBar", "AmpersandAmpersand", "QuestionQuestion", "ExclamationEquals", "EqualsEquals", "LessThanEquals", "LessThanLessThan", "LessThanLessThanEquals", "GreaterThanEquals", "GreaterThanGreaterThan", "GreaterThanGreaterThanEquals", "SlashEquals", "AsteriskEquals", "BarEquals", "AmpersandEquals", "PlusEquals", "MinusEquals", "CaretEquals", "PercentEquals", "IsKeyword", "AsKeyword" });
                                        break;

                                    case "PrefixUnaryExpressionSyntax":
                                        propertyType = "PrefixUnaryOperator";
                                        items.AddRange(new[] { "Tilde", "Exclamation", "Ampersand", "Asterisk", "Minus", "Plus", "MinusMinus", "PlusPlus" });
                                        break;

                                    case "PostfixUnaryExpressionSyntax":
                                        propertyType = "PostfixUnaryOperator";
                                        items.AddRange(new[] { "MinusMinus", "PlusPlus" });
                                        break;

                                    case "MemberAccessExpressionSyntax":
                                        // The operator token on this is ignored.
                                        break;

                                    default:
                                        throw new InvalidOperationException();
                                }
                            }

                            if (propertyType == null)
                                continue;
                            else if (!enums.Contains(propertyType))
                                enums.Add(new EnumDeclaration(propertyType, items.ToArray()));
                        }
                        else if (
                            type.Name == "LiteralExpressionSyntax" &&
                            property.Name == "Token"
                        )
                        {
                            propertyType = "object";
                            propertyName = "Value";
                        }
                        else if (
                            property.Name.EndsWith("Token") ||
                            property.Name == "EndOfComment" || // DocumentationComment
                            property.Name == "Comma" // RefValueExpression
                        )
                        {
                            continue;
                        }
                        else if (property.Name.EndsWith("Keyword"))
                        {
                            string trimmedName = StripPostfix(property.Name, "Keyword");

                            string[] items;

                            if (trimmedName == "Variance")
                            {
                                propertyName = trimmedName;
                                items = new[] { "None", "In", "Out" };
                            }
                            else if (SplitNames(trimmedName).Contains("Or"))
                            {
                                if (trimmedName == "RefOrOut")
                                {
                                    propertyName = "Modifier";
                                    trimmedName = "ParameterModifier";
                                    items = new[] { "None", "Ref", "Out" };
                                }
                                else
                                {
                                    propertyName = "Kind";
                                    items = trimmedName.Split(new[] { "Or" }, StringSplitOptions.None);
                                }
                            }
                            else
                            {
                                continue;
                            }

                            propertyType = trimmedName;

                            if (!enums.Contains(propertyType))
                                enums.Add(new EnumDeclaration(propertyType, items.ToArray()));
                        }
                        else
                        {
                            propertyType = null;

                            switch (property.Name)
                            {
                                case "Guid":
                                case "File":
                                case "Line":
                                case "Bytes":
                                case "Identifier":
                                case "LocalName":
                                case "Name":
                                case "Prefix":
                                    propertyType = "string";
                                    break;

                                case "Commas":
                                    propertyType = "int?";
                                    break;
                            }

                            if (propertyType == null)
                                throw new InvalidOperationException();
                        }
                    }

                    if (
                        type.Name == "GotoStatementSyntax" &&
                        propertyType == "CaseOrDefault"
                    )
                        propertyType += "?";

                    if (
                        property.PropertyType.Name.StartsWith("SyntaxList`") ||
                        property.PropertyType.Name.StartsWith("SeparatedSyntaxList`")
                    )
                    {
                        if (propertyType.StartsWith("Separated"))
                            propertyType = propertyType.Substring("Separated".Length);

                        getChildrenGenerators.Add(new ListGetChildrenGenerator(propertyName));

                        builder.AppendLine("public {0} {1} {{ get; private set; }}", propertyType, propertyName);

                        initializeLists.Add(propertyName, propertyType);
                    }
                    else if (
                        typeof(SyntaxNode).IsAssignableFrom(property.PropertyType) &&
                        propertyType == property.PropertyType.Name
                    )
                    {
                        getChildrenGenerators.Add(new SimpleGetChildrenGenerator(propertyName));

                        string fieldName = MakeFieldName(propertyName);

                        builder.AppendLine("private {0} {1};", propertyType, fieldName);
                        builder.AppendLine("public {0} {1}", propertyType, propertyName);
                        builder.AppendLine("{");
                        builder.Indent();

                        builder.AppendLine("get {{ return {0}; }}", fieldName);
                        builder.AppendLine("set");
                        builder.AppendLine("{");
                        builder.Indent();

                        builder.AppendLine("if ({0} != null)", fieldName);
                        builder.Indent();
                        builder.AppendLine("RemoveChild({0});", fieldName);
                        builder.Unindent();
                        builder.AppendLine();
                        builder.AppendLine("{0} = value;", fieldName);
                        builder.AppendLine();

                        builder.AppendLine("if ({0} != null)", fieldName);
                        builder.Indent();
                        builder.AppendLine("AddChild({0});", fieldName);
                        builder.Unindent();

                        builder.Unindent();
                        builder.AppendLine("}");

                        builder.Unindent();
                        builder.AppendLine("}");
                    }
                    else
                    {
                        builder.AppendLine("public {0} {1} {{ get; set; }}", propertyType, propertyName);
                    }

                    foreach (var item in factories.Where(p => type.IsAssignableFrom(p.Key)))
                    {
                        FixupFactories(item.Value, property, propertyName, propertyType);
                    }

                    builder.AppendLine();
                }

                if (type.IsAbstract)
                {
                    builder.AppendLine("internal {0}(SyntaxKind syntaxKind)", type.Name);
                    builder.Indent();
                    builder.AppendLine(": base(syntaxKind)");
                    builder.Unindent();
                }
                else
                {
                    builder.AppendLine("public {0}()", type.Name);
                    builder.Indent();
                    builder.AppendLine(": base(SyntaxKind.{0})", StripPostfix(type.Name, "Syntax"));
                    builder.Unindent();
                }

                builder.AppendLine("{");
                builder.Indent();

                foreach (var item in initializeLists.OrderBy(p => p.Key))
                {
                    builder.AppendLine("{0} = new {1}(this);", item.Key, item.Value);
                }

                builder.Unindent();
                builder.AppendLine("}");

                if (getChildrenGenerators.Count > 0)
                {
                    builder.AppendLine();

                    builder.AppendLine("public override IEnumerable<SyntaxNode> ChildNodes()");
                    builder.AppendLine("{");
                    builder.Indent();

                    bool hadOneChild = false;

                    if (baseType != "SyntaxNode" && baseType != "SyntaxTriviaNode")
                    {
                        builder.AppendLine("foreach (var child in base.ChildNodes())");
                        builder.AppendLine("{");
                        builder.Indent();
                        builder.AppendLine("yield return child;");
                        builder.Unindent();
                        builder.AppendLine("}");

                        hadOneChild = true;
                    }

                    foreach (var generator in getChildrenGenerators)
                    {
                        if (hadOneChild)
                            builder.AppendLine();
                        else
                            hadOneChild = true;

                        generator.Build(builder);
                    }

                    builder.Unindent();
                    builder.AppendLine("}");
                }

                if (!type.IsAbstract)
                {
                    builder.AppendLine();

                    builder.AppendLine("[DebuggerStepThrough]");
                    builder.AppendLine("public override void Accept(ISyntaxVisitor visitor)");
                    builder.AppendLine("{");
                    builder.Indent();

                    builder.AppendLine("if (!visitor.Done)");
                    builder.Indent();
                    builder.AppendLine("visitor.Visit{0}(this);", StripPostfix(type.Name, "Syntax"));
                    builder.Unindent();

                    builder.Unindent();
                    builder.AppendLine("}");

                    builder.AppendLine();

                    builder.AppendLine("[DebuggerStepThrough]");
                    builder.AppendLine("public override T Accept<T>(ISyntaxVisitor<T> visitor)");
                    builder.AppendLine("{");
                    builder.Indent();

                    builder.AppendLine("return visitor.Visit{0}(this);", StripPostfix(type.Name, "Syntax"));

                    builder.Unindent();
                    builder.AppendLine("}");
                }

                builder.Unindent();
                builder.AppendLine("}");

                builder.Unindent();
                builder.AppendLine("}");

                writtenTypes.Add(type);

                WriteFile(target, type.Name, builder);
            }

            var syntaxKinds = new List<string>();

            syntaxKinds.Add("UserDefined");
            syntaxKinds.AddRange(nonAbstractTypes.OrderBy(p => p).Select(p => StripPostfix(p, "Syntax")));

            enums.Add(new EnumDeclaration("SyntaxKind", syntaxKinds.ToArray()));

            foreach (var enumType in enums.OrderBy(p => p.Name))
            {
                var builder = new CodeFragmentBuilder();

                builder.AppendLine("using System;");
                builder.AppendLine("using System.Collections.Generic;");
                builder.AppendLine("using System.Linq;");
                builder.AppendLine("using System.Text;");
                builder.AppendLine();
                builder.AppendLine("namespace CSharpSyntax");
                builder.AppendLine("{");
                builder.Indent();

                enumType.Build(builder);

                builder.Unindent();
                builder.AppendLine("}");

                WriteFile(target, enumType.Name, builder);
            }

            BuildVisitor(target, nonAbstractTypes);
            BuildBaseVisitor(target, nonAbstractTypes);
            BuildFactories(target, factories, writtenTypes);
        }

        private static IEnumerable<SyntaxType> GetTypes()
        {
            foreach (var type in typeof(SyntaxNode).Assembly.GetExportedTypes())
            {
                if (
                    !typeof(SyntaxNode).IsAssignableFrom(type) ||
                    type == typeof(SyntaxNode) ||
                    typeof(StructuredTriviaSyntax).IsAssignableFrom(type) ||
                    type.Name.StartsWith("Xml")
                )
                    continue;

                if (IgnoredTypes.Any(p => p.IsAssignableFrom(type)))
                    continue;

                var result = SyntaxType.GetType(type);

                if (_anonymousMethodTypes.Contains(type))
                {
                    result.Properties.Add(new SyntaxProperty
                    {
                        Name = "Modifiers",
                        DeclaringType = result,
                        PropertyType = typeof(SyntaxTokenList)
                    });
                }

                yield return result;
            }

            yield return GetAwaitType();
        }

        private static SyntaxType GetAwaitType()
        {
            if (_awaitType == null)
            {
                _awaitType = new SyntaxType
                {
                    Name = "AwaitExpressionSyntax",
                    BaseType = SyntaxType.GetType(typeof(ExpressionSyntax))
                };

                _awaitType.Properties.Add(new SyntaxProperty
                {
                    DeclaringType = _awaitType,
                    Name = "Expression",
                    PropertyType = typeof(ExpressionSyntax)
                });
            }

            return _awaitType;
        }

        private void BuildFactories(string target, Dictionary<SyntaxType, List<Factory>> factories, HashSet<SyntaxType> writtenTypes)
        {
            var builder = new CodeFragmentBuilder();

            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Linq;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine();
            builder.AppendLine("namespace CSharpSyntax");
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("public static partial class Syntax");
            builder.AppendLine("{");
            builder.Indent();

            bool hadOne = false;

            foreach (var item in factories.Where(p => writtenTypes.Contains(p.Key)).OrderBy(p => p.Key.Name))
            {
                foreach (var factory in item.Value)
                {
                    factory.Parameters.RemoveAll(p => p.Type == "SyntaxKind");
                }
            }

            SeedParamsFactories(factories);
            SeedParseNameFactories(factories);

            foreach (var item in factories.Where(p => writtenTypes.Contains(p.Key)).OrderBy(p => p.Key.Name))
            {
                for (int i = 0; i < item.Value.Count; i++)
                {
                    var factory = item.Value[i];
                    bool duplicate = false;

                    for (int j = i + 1; j < item.Value.Count; j++)
                    {
                        if (factory.Equals(item.Value[j]))
                        {
                            duplicate = true;
                            break;
                        }
                    }

                    if (duplicate)
                        continue;
                    if (factory.Parameters.Count == 0 && item.Value.Any(p => p.Parameters.Count > 0))
                        continue;
                    if (FactoryWithMoreParametersExists(factory, item.Value))
                        continue;

                    if (hadOne)
                        builder.AppendLine();
                    else
                        hadOne = true;

                    builder.Append(
                        "public static {0} {1}(",
                        item.Key.Name,
                        factory.Name
                    );

                    bool hadOneChild = false;

                    foreach (var parameter in factory.Parameters)
                    {
                        if (hadOneChild)
                            builder.Append(", ");
                        else
                            hadOneChild = true;

                        if (parameter.IsParams)
                            builder.Append("params ");

                        string type = parameter.Type;

                        switch (type)
                        {
                            case "String": type = "string"; break;
                            case "Object": type = "object"; break;
                        }

                        if (parameter.IsParams && !type.EndsWith("[]"))
                            type += "[]";

                        if (parameter.IsList)
                            builder.Append("IEnumerable<{0}> {1}", type, EscapeParameterName(parameter));
                        else
                            builder.Append("{0} {1}", type, EscapeParameterName(parameter));

                        if (
                            (parameter.IsOptional || true) && // All parameters are currently generated as default
                            !parameter.IsParams
                        )
                        {
                            if (
                                parameter.OriginalType.IsValueType ||
                                parameter.OriginalType == typeof(AttributeTargetSpecifierSyntax)
                            )
                                builder.Append(" = default({0})", parameter.Type);
                            else
                                builder.Append(" = null");
                        }
                    }

                    builder.AppendLine(")");
                    builder.AppendLine("{");
                    builder.Indent();

                    builder.AppendLine("var result = new {0}();", item.Key.Name);
                    builder.AppendLine();

                    foreach (var parameter in factory.Parameters)
                    {
                        if (parameter.Type == "SyntaxKind")
                            continue;

                        string propertyName = parameter.Name.Substring(0, 1).ToUpperInvariant() + parameter.Name.Substring(1);
                        string parameterName = EscapeParameterName(parameter);

                        string parseName = parameter.ParseNameType;

                        if (item.Key.Name == "AliasQualifiedNameSyntax")
                        {
                            if (parameter.Name == "alias" && parameter.Type == "String")
                                parseName = "IdentifierNameSyntax";
                        }
                        else if (item.Key.Name == "IdentifierNameSyntax")
                        {
                            if (parameter.Name == "name")
                                propertyName = "Identifier";
                        }
                        else if (new[] { "NameColonSyntax", "NameEqualsSyntax", "TypeParameterConstraintClauseSyntax" }.Contains(item.Key.Name))
                        {
                            if (parameter.Name == "name" && parameter.Type == "String")
                                parseName = "IdentifierNameSyntax";
                        }

                        if (parseName != null)
                        {
                            builder.AppendLine("if ({0} != null)", parameterName);
                            builder.Indent();

                            if (parseName == "TypeSyntax")
                            {
                                if (parameter.IsList || parameter.IsParams)
                                {
                                    builder.AppendLine(
                                        "result.{0}.AddRange(ParseNames<TypeSyntax>({1}));",
                                        propertyName,
                                        parameterName
                                    );
                                }
                                else
                                {
                                    builder.AppendLine(
                                        "result.{0} = ParseName({1});",
                                        propertyName,
                                        parameterName
                                    );
                                }
                            }
                            else
                            {
                                if (parameter.IsList || parameter.IsParams)
                                {
                                    builder.AppendLine(
                                        "result.{0}.AddRange(ParseNames<{2}>({1}));",
                                        propertyName,
                                        parameterName,
                                        parseName
                                    );
                                }
                                else
                                {
                                    builder.AppendLine(
                                        "result.{0} = ({2})ParseName({1});",
                                        propertyName,
                                        parameterName,
                                        parseName
                                    );
                                }
                            }

                            builder.Unindent();
                        }
                        else if (parameter.IsList || parameter.IsParams)
                        {
                            builder.AppendLine("if ({0} != null)", parameterName);
                            builder.Indent();
                            builder.AppendLine("result.{0}.AddRange({1});", propertyName, parameterName);
                            builder.Unindent();
                        }
                        else
                        {
                            builder.AppendLine(
                                "result.{0} = {1};",
                                propertyName,
                                parameterName
                            );
                        }
                    }

                    builder.AppendLine();
                    builder.AppendLine("return result;");

                    builder.Unindent();
                    builder.AppendLine("}");
                }
            }

            builder.Unindent();
            builder.AppendLine("}");

            builder.Unindent();
            builder.AppendLine("}");

            WriteFile(target, "Syntax", builder);
        }

        private void SeedParseNameFactories(Dictionary<SyntaxType, List<Factory>> factories)
        {
            foreach (var item in factories)
            {
                var newFactories = new List<Factory>();

                foreach (var factory in item.Value)
                {
                    if (factory.Parameters.All(p => !typeof(TypeSyntax).IsAssignableFrom(p.OriginalType)))
                        continue;

                    var newFactory = new Factory(factory.Name);

                    foreach (var parameter in factory.Parameters)
                    {
                        FactoryParameter newParameter;

                        if (typeof(TypeSyntax).IsAssignableFrom(parameter.OriginalType))
                        {
                            newParameter = new FactoryParameter(
                                parameter.Name,
                                parameter.OriginalType,
                                "String",
                                parameter.IsList,
                                parameter.IsParams,
                                parameter.IsOptional
                            )
                            {
                                ParseNameType = parameter.OriginalType.Name
                            };
                        }
                        else
                        {
                            newParameter = new FactoryParameter(parameter);
                        }

                        newFactory.Parameters.Add(newParameter);
                    }

                    newFactories.Add(newFactory);
                }

                newFactories.RemoveAll(p => item.Value.Any(p1 => p1.Equals(p)));

                item.Value.AddRange(newFactories);
            }
        }

        private void SeedParamsFactories(Dictionary<SyntaxType, List<Factory>> factories)
        {
            foreach (var item in factories)
            {
                foreach (var factory in item.Value)
                {
                    if (
                        factory.Parameters.Count == 0 ||
                        !factory.Parameters[factory.Parameters.Count - 1].IsList
                    )
                        continue;

                    // Only generate params for factories with a single
                    // parameter. Named arguments won't work otherwise.

                    if (factory.Parameters.Count != 1)
                        continue;

                    var newFactory = new Factory(factory.Name);

                    for (int i = 0; i < factory.Parameters.Count; i++)
                    {
                        var parameter = factory.Parameters[i];

                        if (i == factory.Parameters.Count - 1)
                        {
                            newFactory.Parameters.Add(new FactoryParameter(
                                parameter.Name,
                                parameter.OriginalType,
                                parameter.Type + "[]",
                                false,
                                true,
                                false
                            ));
                        }
                        else
                        {
                            newFactory.Parameters.Add(parameter);
                        }
                    }

                    if (!item.Value.Any(newFactory.Equals))
                    {
                        item.Value.Add(newFactory);
                        break;
                    }
                }
            }
        }

        private bool FactoryWithMoreParametersExists(Factory factory, List<Factory> list)
        {
            foreach (var other in list)
            {
                if (factory == other || factory.Parameters.Count >= other.Parameters.Count)
                    continue;

                bool allEqual = true;

                for (int i = 0; i < factory.Parameters.Count; i++)
                {
                    // The first test only skips factories for which the first n
                    // parameters are equal. The second test skips all factories
                    // that have less parameters, but for which all parameters
                    // are equal.

                    if (!factory.Parameters[i].Equals(other.Parameters[i]))
                    //if (!other.Parameters.Any(p => p.Equals(factory.Parameters[i])))
                    {
                        allEqual = false;
                        break;
                    }
                }

                if (allEqual)
                    return true;
            }

            return false;
        }

        private string EscapeParameterName(FactoryParameter parameter)
        {
            switch (parameter.Name)
            {
                case "else":
                case "finally":
                case "operator":
                case "default":
                    return "@" + parameter.Name;

                default:
                    return parameter.Name;
            }
        }

        private void FixupFactories(List<Factory> factories, SyntaxProperty property, string propertyName, string propertyType)
        {
            foreach (var factory in factories)
            {
                foreach (var parameter in factory.Parameters)
                {
                    if (propertyName == "Kind" && parameter.Name == "kind")
                    {
                        parameter.Type = propertyType;
                    }
                    else if (
                        (
                            parameter.OriginalType == property.PropertyType &&
                            parameter.Type != propertyType
                        ) || (
                            parameter.Name == "kind" && new[] { "BinaryOperator", "PrefixUnaryOperator", "PostfixUnaryOperator" }.Contains(propertyType)
                        )
                    )
                    {
                        parameter.Type = propertyType;
                        parameter.Name = propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
                    }
                }
            }
        }

        private Dictionary<SyntaxType, List<Factory>> GetFactories()
        {
            var result = new Dictionary<SyntaxType, List<Factory>>();
            List<Factory> factories;

            foreach (var method in GetSyntaxMethods())
            {
                Debug.Assert(method.ReturnType.Name == method.Name + "Syntax");

                if (!result.TryGetValue(method.ReturnType, out factories))
                {
                    factories = new List<Factory>();
                    result.Add(method.ReturnType, factories);
                }

                //if (method.GetParameters().Any(p =>
                //    typeof(SyntaxToken).IsAssignableFrom(p.ParameterType) ||
                //    IgnoredTypes.Any(p1 => p1.IsAssignableFrom(p.ParameterType))
                //))
                //    continue;

                var factory = new Factory(method);

                if (!factories.Any(p => p.Equals(factory)))
                    factories.Add(factory);
            }

            result.Remove(result.Keys.Single(p => p.Name == "TypeDeclarationSyntax"));

            factories = result[result.Keys.Single(p => p.Name == "LiteralExpressionSyntax")];

            factories.Clear();
            factories.Add(new Factory("LiteralExpression")
            {
                Parameters =
                {
                    new FactoryParameter("value", typeof(object), "Object", false, false, false)
                }
            });

            AddParametersAndBody(result[result.Keys.Single(p => p.Name == "MethodDeclarationSyntax")]);
            AddParametersAndBody(result[result.Keys.Single(p => p.Name == "DelegateDeclarationSyntax")]);
            AddParametersAndBody(result[result.Keys.Single(p => p.Name == "ConstructorDeclarationSyntax")]);
            AddParametersAndBody(result[result.Keys.Single(p => p.Name == "DestructorDeclarationSyntax")]);

            return result;
        }

        private static void AddParametersAndBody(List<Factory> factories)
        {
            var primary = factories.Single(p => p.Parameters.Count == factories.Min(p1 => p1.Parameters.Count));
            var secondary = factories.Single(p => p.Parameters.Count == factories.Max(p1 => p1.Parameters.Count));

            primary.Parameters.Add(new FactoryParameter(secondary.Parameters.Single(p => p.Name == "parameterList")));

            var bodyParameter = secondary.Parameters.SingleOrDefault(p => p.Name == "body");

            if (bodyParameter != null)
                primary.Parameters.Add(new FactoryParameter(bodyParameter));
        }

        private static IEnumerable<SyntaxMethod> GetSyntaxMethods()
        {
            var methods = (
                from method in typeof(Syntax).GetMethods()
                where
                    !(
                        method.Name.StartsWith("Parse") ||
                        method.Name.StartsWith("Get")
                    ) &&
                    typeof(SyntaxNode).IsAssignableFrom(method.ReturnType)
                select method
            ).ToArray();

            var anonymousMethods = new List<MethodInfo>();

            foreach (var type in _anonymousMethodTypes)
            {
                var maxParameters = methods.Where(p => p.ReturnType == type).Max(p => p.GetParameters().Length);
                anonymousMethods.Add(methods.Single(p => p.ReturnType == type && p.GetParameters().Length == maxParameters));
            }

            foreach (var method in methods)
            {
                var result = new SyntaxMethod(method);

                yield return result;

                if (anonymousMethods.Contains(method))
                {
                    result = new SyntaxMethod(method);

                    result.Parameters.Insert(0, new SyntaxParameter 
                    {
                        Name = "modifiers",
                        ParameterType = typeof(SyntaxTokenList),
                        IsOptional = true
                    });

                    yield return result;
                }
            }

            yield return new SyntaxMethod
            {
                Name = "AwaitExpression",
                ReturnType = GetAwaitType(),
                Parameters =
                {
                    new SyntaxParameter
                    {
                        Name = "expression",
                        ParameterType = typeof(ExpressionSyntax),
                        IsOptional = true
                    }
                }
            };
        }

        private void BuildVisitor(string target, HashSet<string> visitors)
        {
            var builder = new CodeFragmentBuilder();

            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Linq;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine();
            builder.AppendLine("namespace CSharpSyntax");
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("public interface ISyntaxVisitor");
            builder.AppendLine("{");
            builder.Indent();
            builder.AppendLine("bool Done { get; }");

            foreach (string name in visitors.OrderBy(p => p))
            {
                builder.AppendLine();
                builder.AppendLine("void Visit{1}({0} node);", name, StripPostfix(name, "Syntax"));
            }

            builder.Unindent();
            builder.AppendLine("}");
            builder.AppendLine();

            builder.AppendLine("public interface ISyntaxVisitor<T>");
            builder.AppendLine("{");
            builder.Indent();

            bool hadOne = false;

            foreach (string name in visitors.OrderBy(p => p))
            {
                if (hadOne)
                    builder.AppendLine();
                else
                    hadOne = true;

                builder.AppendLine("T Visit{1}({0} node);", name, StripPostfix(name, "Syntax"));
            }

            builder.Unindent();
            builder.AppendLine("}");

            builder.Unindent();
            builder.AppendLine("}");

            WriteFile(target, "ISyntaxVisitor", builder);
        }

        private void BuildBaseVisitor(string target, HashSet<string> visitors)
        {
            var builder = new CodeFragmentBuilder();

            builder.AppendLine("using System;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.Linq;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine("using System.Diagnostics;");
            builder.AppendLine();
            builder.AppendLine("namespace CSharpSyntax");
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("public class SyntaxVisitorBase : ISyntaxVisitor");
            builder.AppendLine("{");
            builder.Indent();
            builder.AppendLine("public bool Done { get; protected set; }");

            foreach (string name in visitors.OrderBy(p => p))
            {
                builder.AppendLine();
                builder.AppendLine("[DebuggerStepThrough]");
                builder.AppendLine("public virtual void Visit{1}({0} node)", name, StripPostfix(name, "Syntax"));
                builder.AppendLine("{");
                builder.Indent();

                builder.AppendLine("DefaultVisit(node);");

                builder.Unindent();
                builder.AppendLine("}");
            }

            builder.AppendLine();
            builder.AppendLine("[DebuggerStepThrough]");
            builder.AppendLine("public virtual void Visit(SyntaxNode node)");
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("if (node != null)");
            builder.Indent();
            builder.AppendLine("node.Accept(this);");
            builder.Unindent();

            builder.Unindent();
            builder.AppendLine("}");

            builder.AppendLine();
            builder.AppendLine("[DebuggerStepThrough]");
            builder.AppendLine("public virtual void DefaultVisit(SyntaxNode node)");
            builder.AppendLine("{");
            builder.AppendLine("}");

            builder.Unindent();
            builder.AppendLine("}");
            builder.AppendLine();

            builder.AppendLine("public class SyntaxVisitorBase<T> : ISyntaxVisitor<T>");
            builder.AppendLine("{");
            builder.Indent();

            foreach (string name in visitors.OrderBy(p => p))
            {
                builder.AppendLine("[DebuggerStepThrough]");
                builder.AppendLine("public virtual T Visit{1}({0} node)", name, StripPostfix(name, "Syntax"));
                builder.AppendLine("{");
                builder.Indent();

                builder.AppendLine("return DefaultVisit(node);");

                builder.Unindent();
                builder.AppendLine("}");

                builder.AppendLine();
            }

            builder.AppendLine("[DebuggerStepThrough]");
            builder.AppendLine("public virtual T Visit(SyntaxNode node)");
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("if (node == null)");
            builder.Indent();
            builder.AppendLine("return default(T);");
            builder.Unindent();
            builder.AppendLine();
            builder.AppendLine("return node.Accept(this);");

            builder.Unindent();
            builder.AppendLine("}");

            builder.AppendLine();

            builder.AppendLine("[DebuggerStepThrough]");
            builder.AppendLine("public virtual T DefaultVisit(SyntaxNode node)");
            builder.AppendLine("{");
            builder.Indent();

            builder.AppendLine("return default(T);");

            builder.Unindent();
            builder.AppendLine("}");

            builder.Unindent();
            builder.AppendLine("}");

            builder.Unindent();
            builder.AppendLine("}");

            WriteFile(target, "SyntaxVisitorBase", builder);
        }

        private string MakeFieldName(string propertyName)
        {
            return "_" + propertyName.Substring(0, 1).ToLowerInvariant() + propertyName.Substring(1);
        }

        private void WriteFile(string target, string fileName, CodeFragmentBuilder builder)
        {
            Directory.CreateDirectory(target);

            File.WriteAllText(
                Path.Combine(target, fileName + ".cs"),
                builder.GetFragment().ToString(0),
                new UTF8Encoding(false)
            );
        }

        private string StripPostfix(string kindName, string postfix)
        {
            if (kindName.EndsWith(postfix))
                return kindName.Substring(0, kindName.Length - postfix.Length);

            return kindName;
        }

        private List<string> SplitNames(string name)
        {
            var sb = new StringBuilder();
            var result = new List<string>();

            foreach (char c in name)
            {
                if (Char.IsUpper(c))
                {
                    if (sb.Length > 0)
                    {
                        result.Add(sb.ToString());
                        sb.Clear();
                    }
                }

                sb.Append(c);
            }

            if (sb.Length > 0)
                result.Add(sb.ToString());

            return result;
        }
    }
}
