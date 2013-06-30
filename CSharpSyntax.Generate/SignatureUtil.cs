// Taken from https://svn.re-motion.org/svn/Remotion-Contrib/MixinXRef/trunk/MixinXRef.

using System;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace CSharpSyntax.Generate
{
    public static class SignatureUtil
    {
        public static string GetMemberSignatur(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Method:
                    var methodInfo = (MethodInfo)memberInfo;
                    return CreateMethodMarkup(GetMethodName(methodInfo.Name), methodInfo.ReturnType, methodInfo.GetParameters(), memberInfo.GetCustomAttribute<ExtensionAttribute>() != null);

                case MemberTypes.Constructor:
                    var constructorInfo = (ConstructorInfo)memberInfo;
                    return CreateConstructorMarkup(GetAbreviatedTypeName(memberInfo.DeclaringType), constructorInfo.GetParameters());

                case MemberTypes.Event:
                    var eventInfo = (EventInfo)memberInfo;
                    return CreateEventMarkup(eventInfo.Name, eventInfo.EventHandlerType);

                case MemberTypes.Field:
                    var fieldInfo = (FieldInfo)memberInfo;
                    return CreateFieldMarkup(fieldInfo.Name, fieldInfo.FieldType);

                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberInfo;
                    return CreatePropertyMarkup(propertyInfo.Name, propertyInfo.PropertyType, propertyInfo);

                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                    var nestedType = (Type)memberInfo;

                    if (typeof(Delegate).IsAssignableFrom(nestedType))
                        return CreateDelegateMarkup(nestedType);
                    else
                        return CreateNestedTypeMarkup(nestedType);

                default:
                    throw new Exception("unknown member type");
            }
        }

        private static string GetMethodName(string name)
        {
            switch (name)
            {
                case "op_Decrement": return "operator --";
                case "op_Increment": return "operator ++";
                case "op_Negation": return "operator !";
                case "op_UnaryNegation": return "operator -";
                case "op_UnaryPlus": return "operator +";
                case "op_Addition": return "operator +";
                case "op_Assign": return "operator =";
                case "op_BitwiseAnd": return "operator &";
                case "op_BitwiseOr": return "operator |";
                case "op_Division": return "operator /";
                case "op_Equality": return "operator ==";
                case "op_ExclusiveOr": return "operator ^";
                case "op_GreaterThan": return "operator >";
                case "op_GreaterThanOrEqual": return "operator >=";
                case "op_Inequality": return "operator !=";
                case "op_LeftShift": return "operator <<";
                case "op_LessThan": return "operator <";
                case "op_LessThanOrEqual": return "operator <=";
                case "op_LogicalAnd": return "operator &&";
                case "op_LogicalOr": return "operator ||";
                case "op_Modulus": return "operator %";
                case "op_Multiply": return "operator *";
                case "op_RightShift": return "operator >>";
                case "op_Subtraction": return "operator -";
                default: return name;
            }
        }

        public static MemberModifier GetTypeModifiers(Type type)
        {
            var modifiers = MemberModifier.None;

            if (type.IsPublic || type.IsNestedPublic)
                modifiers = MemberModifier.Public;
            else if (type.IsNestedFamily)
                modifiers = MemberModifier.Protected;
            else if (type.IsNestedFamORAssem)
                modifiers = MemberModifier.Protected | MemberModifier.Internal;
            else if (type.IsNestedAssembly)
                modifiers = MemberModifier.Internal;
            else if (type.IsNestedPrivate)
                modifiers = MemberModifier.Private;
            // non nested internal class - no own flag?
            else if (type.IsNotPublic)
                modifiers = MemberModifier.Internal;

            if (type.IsAbstract)
                modifiers |= MemberModifier.Abstract;
            else if (type.IsSealed)
                modifiers |= MemberModifier.Sealed;

            return modifiers;
        }

        public static bool IsOverriddenMember(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");

            var methodInfo = memberInfo as MethodInfo;
            if (methodInfo != null)
                return IsOverriddenMethod(methodInfo);

            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return IsOverriddenMethod(propertyInfo.GetGetMethod(true))
                       || IsOverriddenMethod(propertyInfo.GetSetMethod(true));
            }

            var eventInfo = memberInfo as EventInfo;
            if (eventInfo != null)
            {
                return IsOverriddenMethod(eventInfo.GetAddMethod(true))
                       || IsOverriddenMethod(eventInfo.GetRaiseMethod(true))
                       || IsOverriddenMethod(eventInfo.GetRemoveMethod(true));
            }

            return false;
        }

        public static MemberModifier GetMemberModifiers(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Method:
                case MemberTypes.Constructor:
                    return GetMethodModifiers(memberInfo, memberInfo);
                case MemberTypes.Field:
                    return GetFieldModifiers((FieldInfo)memberInfo);

                case MemberTypes.Property:
                    var propertyInfo = (PropertyInfo)memberInfo;
                    return GetMethodModifiers(propertyInfo.GetGetMethod(true) ?? propertyInfo.GetSetMethod(true), memberInfo);

                case MemberTypes.Event:
                    var eventInfo = (EventInfo)memberInfo;
                    return GetMethodModifiers(eventInfo.GetAddMethod(true), memberInfo);

                case MemberTypes.NestedType:
                    return GetTypeModifiers((Type)memberInfo);

                case MemberTypes.Custom:
                case MemberTypes.TypeInfo:
                    throw new NotSupportedException("TODO special MemberTypes");

                default:
                    throw new Exception("unknown member type");
            }
        }

        private static MemberModifier GetMethodModifiers(MemberInfo methodFieldOrConstructor, MemberInfo memberInfoForOverride)
        {
            var methodInfo = (MethodBase)methodFieldOrConstructor;
            var modifiers = MemberModifier.None;

            if (methodInfo.IsPublic)
                modifiers = MemberModifier.Public;
            else if (methodInfo.IsFamily)
                modifiers = MemberModifier.Protected;
            else if (methodInfo.IsFamilyOrAssembly)
                modifiers = MemberModifier.Protected | MemberModifier.Internal;
            else if (methodInfo.IsAssembly)
                modifiers = MemberModifier.Internal;
            else if (methodInfo.IsPrivate)
                modifiers = MemberModifier.Private;

            if (methodFieldOrConstructor is MethodInfo)
            {
                var isOverriddenMember = IsOverriddenMember(memberInfoForOverride);

                if (methodInfo.IsAbstract)
                    modifiers |= MemberModifier.Abstract;
                else if (methodInfo.IsFinal && (!methodInfo.IsVirtual || isOverriddenMember))
                    modifiers |= MemberModifier.Sealed;
                if (isOverriddenMember)
                    modifiers |= MemberModifier.Override;
                if (!isOverriddenMember
                    && !methodInfo.IsAbstract
                    && !methodInfo.IsFinal
                    && methodInfo.IsVirtual)
                    modifiers |= MemberModifier.Virtual;

                // explicit interface implementation
                if (methodInfo.IsHideBySig
                    && methodInfo.IsPrivate
                    && methodInfo.IsFinal
                    && methodInfo.IsVirtual)
                    return MemberModifier.None;
            }

            if (methodInfo.IsStatic)
                modifiers |= MemberModifier.Static;

            return modifiers;
        }

        public static string FormatModifiers(MemberModifier value)
        {
            var modifiers = "";

            if (value.HasFlag(MemberModifier.Public))
                modifiers = "public";
            else if (value.HasFlag(MemberModifier.Protected) && value.HasFlag(MemberModifier.Internal))
                modifiers = "protected internal";
            else if (value.HasFlag(MemberModifier.Protected))
                modifiers = "protected";
            else if (value.HasFlag(MemberModifier.Internal))
                modifiers = "internal";
            else if (value.HasFlag(MemberModifier.Private))
                modifiers = "private";

            if (value.HasFlag(MemberModifier.Abstract))
                modifiers += " abstract";
            else if (value.HasFlag(MemberModifier.Sealed))
                modifiers += " sealed";
            if (value.HasFlag(MemberModifier.Override))
                modifiers += " override";
            if (value.HasFlag(MemberModifier.Virtual))
                modifiers += " virtual";
            if (value.HasFlag(MemberModifier.Static))
                modifiers += " static";
            if (value.HasFlag(MemberModifier.ReadOnly))
                modifiers += " readonly";

            return modifiers;
        }

        private static MemberModifier GetFieldModifiers(FieldInfo methodInfo)
        {
            var modifiers = MemberModifier.None;

            if (methodInfo.IsPublic)
                modifiers = MemberModifier.Public;
            else if (methodInfo.IsFamily)
                modifiers = MemberModifier.Protected;
            else if (methodInfo.IsFamilyOrAssembly)
                modifiers = MemberModifier.Protected | MemberModifier.Internal;
            else if (methodInfo.IsAssembly)
                modifiers = MemberModifier.Internal;
            else if (methodInfo.IsPrivate)
                modifiers = MemberModifier.Private;

            if (methodInfo.IsStatic)
                modifiers |= MemberModifier.Static;

            if (methodInfo.IsInitOnly)
                modifiers |= MemberModifier.ReadOnly;

            return modifiers;
        }

        private static bool IsOverriddenMethod(MethodInfo methodInfo)
        {
            return (methodInfo != null) && (methodInfo != methodInfo.GetBaseDefinition());
        }

        public static string GetFormattedNestedTypeName(Type type)
        {
            if (type.FullName == null)
                return type.Name;

            var nestedTypeName = new StringBuilder();

            var nestedIndex = type.FullName.IndexOf('+');
            var nestingType = type.FullName.Substring(0, nestedIndex);
            var nestedType = type.FullName.Substring(nestedIndex);

            nestedTypeName.Append(nestingType.Substring(nestingType.LastIndexOf('.') + 1));
            nestedTypeName.Append(nestedType.Replace("+", "."));

            return nestedTypeName.ToString();
        }

        private static string GetAbreviatedTypeName(Type type)
        {
            string name = type.Name;

            return name.Split('`')[0];
        }

        public static string GetFormattedGenericTypeName(Type type)
        {
            var typeName = "";
            var nestedTypeName = "";

            // is not really a generic type name
            if (!type.Name.Contains("`") && type.FullName == null)
                return type.Name;

            if (type.IsNested)
            {
                typeName = (type.FullName.Substring(0, type.FullName.IndexOf('`')));
                typeName = typeName.Substring(typeName.LastIndexOf('.') + 1);

                var index = type.FullName.IndexOf('+');
                if (index > 0)
                {
                    nestedTypeName = (type.FullName.Substring(index, type.FullName.Length - index));
                    nestedTypeName = "." + nestedTypeName.Substring(1, nestedTypeName.IndexOf('[') - 1);
                }
            }
            else
                typeName = type.Name.Substring(0, type.Name.IndexOf('`'));

            var result = new StringBuilder(typeName);
            result.Append("<");
            var genericArguments = type.GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i != 0)
                    result.Append(", ");

                result.Append(GetShortFormattedTypeName(genericArguments[i]));
            }
            result.Append(">");
            result.Append(nestedTypeName);

            return result.ToString();
        }

        public static string GetFormattedTypeName(Type type)
        {
            return GetFormattedTypeName(type, true);
        }

        public static string GetFormattedTypeName(Type type, bool includeNamespace)
        {
            string name;

            if (TryGetKeywordType(type, out name))
                return name;

            name = GetShortFormattedTypeName(type);

            if (
                includeNamespace &&
                !String.IsNullOrEmpty(type.Namespace) &&
                type.FullName != null
            )
                name = type.Namespace + "." + name;

            return name;
        }

        private static bool TryGetKeywordType(Type type, out string name)
        {
            string postfix = "";

            name = null;

            if (type.IsArray)
            {
                postfix = "[" + new String(',', type.GetArrayRank() - 1) + "]";

                type = type.GetElementType();
            }
            else if (
                type.IsGenericType &&
                type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))
            )
            {
                postfix = "?";

                var converter = new NullableConverter(type);

                type = converter.UnderlyingType;
            }

            if (typeof(bool) == type)
                name = "bool";
            else if (typeof(int) == type)
                name = "int";
            else if (typeof(uint) == type)
                name = "uint";
            else if (typeof(short) == type)
                name = "short";
            else if (typeof(ushort) == type)
                name = "ushort";
            else if (typeof(byte) == type)
                name = "byte";
            else if (typeof(sbyte) == type)
                name = "sbyte";
            else if (typeof(long) == type)
                name = "long";
            else if (typeof(ulong) == type)
                name = "ulong";
            else if (typeof(char) == type)
                name = "char";
            else if (typeof(float) == type)
                name = "float";
            else if (typeof(double) == type)
                name = "double";
            else if (typeof(decimal) == type)
                name = "decimal";
            else if (type.FullName == "System.Void")
                name = "void";
            else if (typeof(object) == type)
                name = "object";
            else if (typeof(string) == type)
                name = "string";
            else
                return false;

            name += postfix;

            return true;
        }

        public static string GetShortFormattedTypeName(Type type)
        {
            return GetShortFormattedTypeName(type, false);
        }

        public static string GetShortFormattedTypeName(Type type, bool isOut)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string name;

            if (TryGetKeywordType(type, out name))
                return name;

            string postfix = "";

            if (
               type.IsGenericType &&
               type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))
            )
            {
                postfix = "?";

                var converter = new NullableConverter(type);

                type = converter.UnderlyingType;
            }

            if (type.IsGenericType)
                return GetFormattedGenericTypeName(type);
            else if (type.IsNested)
                return GetFormattedNestedTypeName(type);

            name = type.Name;

            if (name.EndsWith("&"))
            {
                if (isOut)
                    name = "out " + name.Substring(0, name.Length - 1);
                else
                    name = "ref " + name.Substring(0, name.Length - 1);
            }

            var match = Regex.Match(name, "^(.*?)`(\\d+)$");

            if (match.Success)
            {
                name = match.Groups[1].Value + "<" + new String(',', int.Parse(match.Groups[2].Value) - 1) + ">";
            }

            return name + postfix;
        }

        public static void AddParameterMarkup(ParameterInfo[] parameterInfos, StringBuilder sb, bool isExtensionMethod)
        {
            if (parameterInfos == null)
                throw new ArgumentNullException("parameterInfos");

            sb.Append("(");

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (i != 0)
                    sb.Append(", ");

                if (isExtensionMethod && i == 0)
                    sb.Append("this ");

                bool isOut = false;
                bool isOptional = false;

                foreach (var attr in parameterInfos[i].GetCustomAttributes(true))
                {
                    if (attr is ParamArrayAttribute)
                        sb.Append("params ");
                    else if (attr is OutAttribute)
                        isOut = true;
                    else if (attr is OptionalAttribute)
                        isOptional = true;
                    else
                        Console.WriteLine("Cannot parse parameter attribute '{0}'", attr.GetType());
                }

                sb.Append(CreateTypeOrKeywordElement(parameterInfos[i].ParameterType, isOut) + " ");
                sb.Append(parameterInfos[i].Name);

                if (isOptional)
                    sb.Append(" = ...");
            }

            sb.Append(")");
        }

        public static string CreateConstructorMarkup(string name, ParameterInfo[] parameterInfos)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (parameterInfos == null)
                throw new ArgumentNullException("parameterInfos");

            return CreateMemberMarkup(null, null, name, parameterInfos);
        }

        public static string CreateDelegateMarkup(Type delegateType)
        {
            if (delegateType == null)
                throw new ArgumentNullException("delegateType");

            var method = delegateType.GetMethod("Invoke");

            return "delegate " + CreateMethodMarkup(delegateType.Name, method.ReturnType, method.GetParameters());
        }

        public static string CreateMethodMarkup(string methodName, Type returnType, ParameterInfo[] parameterInfos)
        {
            return CreateMethodMarkup(methodName, returnType, parameterInfos, false);
        }

        public static string CreateMethodMarkup(string methodName, Type returnType, ParameterInfo[] parameterInfos, bool isExtensionMethod)
        {
            if (methodName == null)
                throw new ArgumentNullException("methodName");
            if (returnType == null)
                throw new ArgumentNullException("returnType");
            if (parameterInfos == null)
                throw new ArgumentNullException("parameterInfos");

            return CreateMemberMarkup(null, returnType, methodName, parameterInfos, isExtensionMethod);
        }

        public static string CreateEventMarkup(string eventName, Type handlerType)
        {
            if (eventName == null)
                throw new ArgumentNullException("eventName");
            if (handlerType == null)
                throw new ArgumentNullException("handlerType");

            return CreateMemberMarkup("event", handlerType, eventName, null);
        }

        public static string CreateFieldMarkup(string fieldName, Type fieldType)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");
            if (fieldType == null)
                throw new ArgumentNullException("fieldType");

            return CreateMemberMarkup(null, fieldType, fieldName, null);
        }

        public static string CreatePropertyMarkup(string propertyName, Type propertyType, PropertyInfo propertyInfo)
        {
            if (propertyName == null)
                throw new ArgumentNullException("propertyName");
            if (propertyType == null)
                throw new ArgumentNullException("propertyType");
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            string markup = CreateMemberMarkup(null, propertyType, propertyName, null);

            var propertyModifier = GetMemberModifiers(propertyInfo);

            markup += " {";

            foreach (var accessor in propertyInfo.GetAccessors())
            {
                var accessorModifier = GetMemberModifiers(accessor);

                if ((accessorModifier & (MemberModifier.Public | MemberModifier.Protected)) != 0)
                {
                    markup += " ";

                    if (
                        propertyModifier == MemberModifier.Public &&
                        accessorModifier.HasFlag(MemberModifier.Protected)
                    )
                        markup += "protected ";

                    markup += accessor.Name.Split('_')[0] + ";";
                }
            }

            markup += " }";

            return markup;
        }

        public static string CreateNestedTypeMarkup(Type nestedType)
        {
            if (nestedType.IsEnum)
                return CreateMemberMarkup("enum", null, nestedType.Name, null);

            string prefix;
            if (nestedType.IsClass)
                prefix = "class";
            else if (nestedType.IsInterface)
                prefix = "interface";
            else if (nestedType.IsValueType)
                prefix = "struct";
            else
                throw new NotSupportedException("Unknown type");

            var nestedTypeMarkup = CreateMemberMarkup(prefix, null, GetShortFormattedTypeName(nestedType), null);

            var inheritance = new List<Type>();

            if (nestedType.BaseType != null && nestedType.BaseType != typeof(object) && nestedType.BaseType != typeof(ValueType))
                inheritance.Add(nestedType.BaseType);

            inheritance.AddRange(GetInterfaces(nestedType));

            for (int i = 0; i < inheritance.Count; i++)
            {
                if (i == 0)
                    nestedTypeMarkup += " : ";
                else
                    nestedTypeMarkup += ", ";

                nestedTypeMarkup += GetShortFormattedTypeName(inheritance[i]);
            }

            return nestedTypeMarkup;
        }

        private static IEnumerable<Type> GetInterfaces(Type type)
        {
            var interfaces = new HashSet<Type>(type.GetInterfaces());

            // Remove all interfaces that are implemented by the base type.

            if (type.BaseType != null)
            {
                foreach (var item in type.BaseType.GetInterfaces())
                {
                    interfaces.Remove(item);
                }
            }

            // Remove all interfaces that are implemented by one of the interfaces
            // in the list.

            var toRemove = new HashSet<Type>();

            foreach (var item in interfaces)
            {
                toRemove.UnionWith(item.GetInterfaces());
            }

            foreach (var item in toRemove)
            {
                interfaces.Remove(item);
            }

            return interfaces.OrderBy(p => p.Name);
        }

        private static string CreateMemberMarkup(string prefix, Type type, string memberName, ParameterInfo[] parameterInfos)
        {
            return CreateMemberMarkup(prefix, type, memberName, parameterInfos, false);
        }

        private static string CreateMemberMarkup(string prefix, Type type, string memberName, ParameterInfo[] parameterInfos, bool isExtensionMethod)
        {
            var sb = new StringBuilder();

            if (prefix != null)
            {
                sb.Append(prefix + " ");
            }

            sb.Append(CreateTypeOrKeywordElement(type) + " ");

            sb.Append(memberName);

            if (parameterInfos != null)
                AddParameterMarkup(parameterInfos, sb, isExtensionMethod);

            return sb.ToString();
        }

        private static string CreateTypeOrKeywordElement(Type type)
        {
            return CreateTypeOrKeywordElement(type, false);
        }

        private static string CreateTypeOrKeywordElement(Type type, bool isOut)
        {
            if (type == null)
                return null;

            return GetShortFormattedTypeName(type, isOut);
        }

        private static string CreateElement(string elementName, string content)
        {
            return content;
        }
    }
}