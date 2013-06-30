using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CSharpSyntax.Printer.Configuration;
using NUnit.Framework;

namespace CSharpSyntax.Test.SyntaxPrinterConfiguration
{
    [TestFixture]
    public class SyntaxPrinterConfigurationFixture
    {
        [Test]
        public void PrintConfiguration()
        {
            PrintObject(new Printer.Configuration.SyntaxPrinterConfiguration(), 0);
        }

        private void PrintObject(object obj, int indent)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(ModifiersCollection))
                {
                    Console.WriteLine(
                        "{0}{1} =",
                        new string(' ', indent * 2),
                        property.Name
                    );

                    foreach (var modifier in (ModifiersCollection)property.GetValue(obj))
                    {
                        Console.WriteLine(
                            "{0}Modifier = {1}",
                            new string(' ', (indent + 1) * 2),
                            modifier
                        );
                    }
                }
                else if (property.PropertyType.IsValueType)
                {
                    Console.WriteLine(
                        "{0}{1} = {2}",
                        new string(' ', indent * 2),
                        property.Name,
                        property.GetValue(obj)
                    );
                }
                else if (property.PropertyType.Namespace == obj.GetType().Namespace)
                {
                    Console.WriteLine(
                        "{0}{1} =",
                        new string(' ', indent * 2),
                        property.Name
                    );

                    PrintObject(property.GetValue(obj), indent + 1);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        [Test]
        public void EmptyConfiguration()
        {
            Assert.AreEqual(
@"<Configuration xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""https://github.com/pvginkel/CSharpSyntax/SyntaxPrinterConfiguration"">
  <BracesLayout />
  <BlankLines />
  <LineBreaksAndWrapping>
    <PlaceOnNewLine />
    <LineWrapping />
    <Other />
  </LineBreaksAndWrapping>
  <Spaces>
    <BeforeParentheses />
    <AroundOperators />
    <WithinParentheses />
    <TernaryOperator />
    <Other />
  </Spaces>
  <Other>
    <Indentation />
    <Modifiers />
    <AlignMultiLineConstructs />
    <Other />
  </Other>
</Configuration>",
                new Printer.Configuration.SyntaxPrinterConfiguration().ToString()
            );
        }

        [Test]
        public void AllConfiguration()
        {
            var config = new Printer.Configuration.SyntaxPrinterConfiguration();

            ChangeValues(config);

            var tmp = config.Other.Modifiers.ModifiersOrder[0];
            config.Other.Modifiers.ModifiersOrder[0] = config.Other.Modifiers.ModifiersOrder[1];
            config.Other.Modifiers.ModifiersOrder[1] = tmp;

            Assert.AreEqual(
@"<Configuration xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""https://github.com/pvginkel/CSharpSyntax/SyntaxPrinterConfiguration"">
  <Indentation>5</Indentation>
  <IndentationStyle>Tabs</IndentationStyle>
  <BracesLayout>
    <TypeAndNamespaceDeclaration>NextLineIndented2</TypeAndNamespaceDeclaration>
    <MethodDeclaration>NextLineIndented2</MethodDeclaration>
    <AnonymousMethodDeclaration>NextLineIndented2</AnonymousMethodDeclaration>
    <BlockUnderCaseLabel>NextLineIndented2</BlockUnderCaseLabel>
    <ArrayAndObjectInitializer>NextLineIndented2</ArrayAndObjectInitializer>
    <Other>NextLineIndented2</Other>
    <EmptyBraceFormatting>TogetherOnSameLine</EmptyBraceFormatting>
  </BracesLayout>
  <BlankLines>
    <AroundNamespace>2</AroundNamespace>
    <InsideNamespace>1</InsideNamespace>
    <AroundType>2</AroundType>
    <InsideType>1</InsideType>
    <AroundField>1</AroundField>
    <AroundMethod>2</AroundMethod>
    <BetweenDifferentUsingGroups>1</BetweenDifferentUsingGroups>
    <AfterUsingList>2</AfterUsingList>
    <AfterFileHeaderComment>2</AfterFileHeaderComment>
  </BlankLines>
  <LineBreaksAndWrapping>
    <PlaceOnNewLine>
      <PlaceElseOnNewLine>false</PlaceElseOnNewLine>
      <PlaceWhileOnNewLine>false</PlaceWhileOnNewLine>
      <PlaceCatchOnNewLine>false</PlaceCatchOnNewLine>
      <PlaceFinallyOnNewLine>false</PlaceFinallyOnNewLine>
    </PlaceOnNewLine>
    <LineWrapping>
      <WrapLongLines>true</WrapLongLines>
      <RightMargin>121</RightMargin>
      <WrapFormalParameters>ChopIfLong</WrapFormalParameters>
      <WrapInvocationArguments>ChopIfLong</WrapInvocationArguments>
      <PreferWrapAfterDotInMethodCalls>true</PreferWrapAfterDotInMethodCalls>
      <WrapChainedMethodCalls>ChopAlways</WrapChainedMethodCalls>
      <WrapExtendsImplementsList>ChopIfLong</WrapExtendsImplementsList>
      <WrapForStatementHeader>ChopAlways</WrapForStatementHeader>
      <WrapTernaryExpression>ChopAlways</WrapTernaryExpression>
      <WrapMultipleDeclarations>ChopAlways</WrapMultipleDeclarations>
      <PreferWrapBeforeOperatorInBinaryExpression>true</PreferWrapBeforeOperatorInBinaryExpression>
      <ForceChopCompoundConditionInIfStatement>true</ForceChopCompoundConditionInIfStatement>
      <ForceChopCompoundConditionInWhileStatement>true</ForceChopCompoundConditionInWhileStatement>
      <ForceChopCompoundConditionInDoStatement>true</ForceChopCompoundConditionInDoStatement>
      <WrapMultipleTypeParameterConstraints>ChopAlways</WrapMultipleTypeParameterConstraints>
      <WrapObjectAndCollectionInitializers>ChopAlways</WrapObjectAndCollectionInitializers>
    </LineWrapping>
    <Other>
      <PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine>false</PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine>
      <PlaceSimplePropertyIndexerEventDeclarationOnSingleLine>true</PlaceSimplePropertyIndexerEventDeclarationOnSingleLine>
      <PlaceSimpleAccessorOnSingleLine>false</PlaceSimpleAccessorOnSingleLine>
      <PlaceSimpleMethodOnSingleLine>true</PlaceSimpleMethodOnSingleLine>
      <PlaceSimpleAnonymousMethodOnSingleLine>true</PlaceSimpleAnonymousMethodOnSingleLine>
      <PlaceLinqExpressionOnSingleLine>true</PlaceLinqExpressionOnSingleLine>
      <PlaceTypeAttributeOnSingleLine>true</PlaceTypeAttributeOnSingleLine>
      <PlaceMethodAttributeOnSameLine>true</PlaceMethodAttributeOnSameLine>
      <PlacePropertyIndexerEventAttributeOnSameLine>true</PlacePropertyIndexerEventAttributeOnSameLine>
      <PlaceSingleLineAccessorAttributeOnSameLine>true</PlaceSingleLineAccessorAttributeOnSameLine>
      <PlaceMultiLineAccessorAttributeOnSameLine>true</PlaceMultiLineAccessorAttributeOnSameLine>
      <PlaceFieldAttributeOnSameLine>true</PlaceFieldAttributeOnSameLine>
      <PlaceConstructorInitializerOnSameLine>true</PlaceConstructorInitializerOnSameLine>
      <PlaceTypeConstraintsOnSameLine>true</PlaceTypeConstraintsOnSameLine>
    </Other>
  </LineBreaksAndWrapping>
  <Spaces>
    <BeforeParentheses>
      <MethodCallParentheses>true</MethodCallParentheses>
      <MethodCallEmptyParentheses>true</MethodCallEmptyParentheses>
      <ArrayAccessBrackets>true</ArrayAccessBrackets>
      <MethodDeclarationParentheses>true</MethodDeclarationParentheses>
      <MethodDeclarationEmptyParentheses>true</MethodDeclarationEmptyParentheses>
      <IfParentheses>false</IfParentheses>
      <WhileParentheses>false</WhileParentheses>
      <CatchParentheses>false</CatchParentheses>
      <SwitchParentheses>false</SwitchParentheses>
      <ForParentheses>false</ForParentheses>
      <ForEachParentheses>false</ForEachParentheses>
      <UsingParentheses>false</UsingParentheses>
      <LockParentheses>false</LockParentheses>
      <TypeOfParentheses>true</TypeOfParentheses>
      <SizeOfParentheses>true</SizeOfParentheses>
      <BeforeTypeParameterListAngle>true</BeforeTypeParameterListAngle>
      <BeforeTypeArgumentListAngle>true</BeforeTypeArgumentListAngle>
    </BeforeParentheses>
    <AroundOperators>
      <AssignmentOperators>false</AssignmentOperators>
      <LogicalOperators>false</LogicalOperators>
      <EqualityOperators>false</EqualityOperators>
      <RelationalOperators>false</RelationalOperators>
      <BitwiseOperators>false</BitwiseOperators>
      <AdditiveOperators>false</AdditiveOperators>
      <MultiplicativeOperators>false</MultiplicativeOperators>
      <ShiftOperators>false</ShiftOperators>
      <NullCoalescingOperator>false</NullCoalescingOperator>
    </AroundOperators>
    <WithinParentheses>
      <Parentheses>true</Parentheses>
      <MethodDeclarationParentheses>true</MethodDeclarationParentheses>
      <MethodDeclarationEmptyParentheses>true</MethodDeclarationEmptyParentheses>
      <MethodCallParentheses>true</MethodCallParentheses>
      <MethodCallEmptyParentheses>true</MethodCallEmptyParentheses>
      <ArrayAccessBrackets>true</ArrayAccessBrackets>
      <TypeCastParentheses>true</TypeCastParentheses>
      <IfParentheses>true</IfParentheses>
      <WhileParentheses>true</WhileParentheses>
      <CatchParentheses>true</CatchParentheses>
      <SwitchParentheses>true</SwitchParentheses>
      <ForParentheses>true</ForParentheses>
      <ForEachParentheses>true</ForEachParentheses>
      <UsingParentheses>true</UsingParentheses>
      <LockParentheses>true</LockParentheses>
      <TypeOfParentheses>true</TypeOfParentheses>
      <SizeOfParentheses>true</SizeOfParentheses>
      <TypeParameterAngles>true</TypeParameterAngles>
      <TypeArgumentAngles>true</TypeArgumentAngles>
    </WithinParentheses>
    <TernaryOperator>
      <BeforeQuestionMark>false</BeforeQuestionMark>
      <AfterQuestionMark>false</AfterQuestionMark>
      <BeforeColon>false</BeforeColon>
      <AfterColon>false</AfterColon>
    </TernaryOperator>
    <Other>
      <AfterTypeCastParentheses>true</AfterTypeCastParentheses>
      <BeforeComma>true</BeforeComma>
      <AfterComma>false</AfterComma>
      <BeforeForSemicolon>true</BeforeForSemicolon>
      <AfterForSemicolon>false</AfterForSemicolon>
      <BeforeColonInAttribute>true</BeforeColonInAttribute>
      <AfterColonInAttribute>false</AfterColonInAttribute>
      <BeforeBaseTypesListColon>false</BeforeBaseTypesListColon>
      <AfterBaseTypesListColon>false</AfterBaseTypesListColon>
      <AroundDot>true</AroundDot>
      <AroundLambdaArrow>false</AroundLambdaArrow>
      <SpacesBetweenEmptyBraces>true</SpacesBetweenEmptyBraces>
      <WithinAttributeBrackets>true</WithinAttributeBrackets>
      <BeforeArrayRankBrackets>true</BeforeArrayRankBrackets>
      <WithinArrayRankBrackets>true</WithinArrayRankBrackets>
      <WithinArrayRankEmptyBrackets>true</WithinArrayRankEmptyBrackets>
      <BeforeSemicolon>true</BeforeSemicolon>
      <BeforeColonInCaseStatement>true</BeforeColonInCaseStatement>
      <BeforeNullableMark>true</BeforeNullableMark>
      <BeforeTypeParameterConstraintColon>false</BeforeTypeParameterConstraintColon>
      <AfterTypeParameterConstraintColon>false</AfterTypeParameterConstraintColon>
      <AroundEqualsInNamespaceAliasDirective>false</AroundEqualsInNamespaceAliasDirective>
    </Other>
  </Spaces>
  <Other>
    <Indentation>
      <ContinuousLineIndentMultiplier>2</ContinuousLineIndentMultiplier>
    </Indentation>
    <Modifiers>
      <UseExplicitPrivateModifier>true</UseExplicitPrivateModifier>
      <UseExplicitInternalModifier>true</UseExplicitInternalModifier>
      <ModifiersOrder>
        <Modifier>Protected</Modifier>
        <Modifier>Public</Modifier>
        <Modifier>Internal</Modifier>
        <Modifier>Private</Modifier>
        <Modifier>New</Modifier>
        <Modifier>Abstract</Modifier>
        <Modifier>Virtual</Modifier>
        <Modifier>Override</Modifier>
        <Modifier>Sealed</Modifier>
        <Modifier>Static</Modifier>
        <Modifier>ReadOnly</Modifier>
        <Modifier>Extern</Modifier>
        <Modifier>Volatile</Modifier>
        <Modifier>Async</Modifier>
      </ModifiersOrder>
    </Modifiers>
    <AlignMultiLineConstructs>
      <MethodParameters>true</MethodParameters>
      <CallArguments>true</CallArguments>
      <ListOfBaseClassesAndInterfaces>true</ListOfBaseClassesAndInterfaces>
      <Expression>true</Expression>
      <ChainedBinaryExpressions>true</ChainedBinaryExpressions>
      <ArrayObjectCollectionInitializer>true</ArrayObjectCollectionInitializer>
      <ForStatementHeader>true</ForStatementHeader>
      <MultipleDeclarations>true</MultipleDeclarations>
      <TypeParameterConstraints>true</TypeParameterConstraints>
      <LinqQuery>true</LinqQuery>
    </AlignMultiLineConstructs>
    <Other>
      <SpecialElseIfTreatment>false</SpecialElseIfTreatment>
      <IndentCaseFromSwitch>false</IndentCaseFromSwitch>
      <IndentNestedUsingStatements>true</IndentNestedUsingStatements>
    </Other>
  </Other>
</Configuration>",
                config.ToString()
            );
        }

        private void ChangeValues(object obj)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null)
                {
                    object value = defaultValueAttribute.Value;
                    object newValue = null;

                    if (value is bool)
                    {
                        newValue = !(bool)value;
                    }
                    else if (value is int)
                    {
                        newValue = (int)value + 1;
                    }
                    else if (value is Enum)
                    {
                        newValue = Enum.ToObject(value.GetType(), (int)value + 1);
                        if (!Enum.IsDefined(value.GetType(), newValue))
                            newValue = Enum.ToObject(value.GetType(), 0);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    property.SetValue(obj, newValue);
                }
                else if (obj.GetType().Name.EndsWith("Configuration"))
                {
                    ChangeValues(property.GetValue(obj));
                }
            }
        }
    }
}
