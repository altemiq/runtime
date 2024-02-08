// -----------------------------------------------------------------------
// <copyright file="SourceGenerator.OneOfGeneric.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.OneOf.CodeGeneration;

using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

/// <content>
/// Methods for OneOf structs.
/// </content>
public partial class SourceGenerator
{
    private static (string Name, StructDeclarationSyntax StructDeclaration) GenerateOneOfGeneric(int count)
    {
        var typeParameterNames = GetTypeParameterNames(count).ToArray();
        var name = FormattableString.Invariant($"{OneOf}`{count}");
        var typeParameters = GetTypeParameters(typeParameterNames);

        var structDeclaration = StructDeclaration(OneOf)
            .WithAttributeLists(
            List(
                GetAttributes()))
            .WithModifiers(
            TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ReadOnlyKeyword)))
            .WithTypeParameterList(typeParameters)
            .WithBaseList(GetBaseList(typeParameterNames))
            .WithMembers(List(
                GetFieldDeclarations(typeParameterNames)
                .Append<MemberDeclarationSyntax>(GetConstructor(typeParameterNames))
                .Append(GetValuePropertyDeclaration(count))
                .Append(GetIndexPropertyDeclaration())
                .Concat(GetIsPropertyDeclarations(typeParameterNames))
                .Concat(GetAsPropertyDeclarations(typeParameterNames))
                .Concat(GetOperatorDeclarations(typeParameterNames))
                .Append(GetSwitchMethodDeclaration(typeParameterNames))
                .Append(GetMatchMethodDeclaration(typeParameterNames))
                .Concat(GetMapMethodDeclarations(typeParameterNames))
                .Concat(GetTryPickMethodDeclarations(typeParameterNames))
                .Concat(GetEqualsMethodDeclarations(typeParameterNames))
                .Append(GetToStringMethodDeclaration())
                .Append(GetToStringWithProviderMethodDeclaration(typeParameterNames))
                .Append(GetGetHashCodeMethodDefinition(typeParameterNames))))
            .WithLeadingTrivia(
            Trivia(
                NullableDirectiveTrivia(
                    Token(SyntaxKind.EnableKeyword),
                    isActive: true)),
            Trivia(
                GetDocumentation(typeParameterNames)))
            .WithTrailingTrivia(
            Trivia(
                NullableDirectiveTrivia(
                    Token(SyntaxKind.RestoreKeyword),
                    isActive: true)));

        return (name, structDeclaration);

        static IEnumerable<AttributeListSyntax> GetAttributes()
        {
            yield return AttributeList(
                SingletonSeparatedList(
                    Attribute(
                        GetQualifiedName(typeof(System.Runtime.InteropServices.StructLayoutAttribute)))
                    .WithArgumentList(
                        AttributeArgumentList(
                            SingletonSeparatedList(
                                AttributeArgument(
                                    GetMemberAccessExpression(System.Runtime.InteropServices.LayoutKind.Auto)))))));

            yield return AttributeList(
                SingletonSeparatedList(
                    GetGeneratedCodeAttribute()));
        }

        static DocumentationCommentTriviaSyntax GetDocumentation(IList<string> typeParameterNames)
        {
            return DocumentationCommentTrivia(
                SyntaxKind.SingleLineDocumentationCommentTrivia,
                List(
                    GetXmlNodes(typeParameterNames)));

            static IEnumerable<XmlNodeSyntax> GetXmlNodes(IList<string> typeParameterNames)
            {
                var example = typeParameterNames.Count == 1
                    ? " Represents an option type with a single type."
                    : $" Represents an option type with {typeParameterNames.Count.ToWords()} types.";

                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextLiteral(
                            TriviaList(
                                DocumentationCommentExterior(TrippleSlash)),
                            Space,
                            Space,
                            TriviaList())));
                yield return XmlExampleElement(
                    SingletonList<XmlNodeSyntax>(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    example,
                                    example,
                                    TriviaList()),
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    Space,
                                    Space,
                                    TriviaList())))))
                    .WithStartTag(
                    XmlElementStartTag(
                        XmlName(
                            Identifier(Keywords.Summary))))
                    .WithEndTag(
                    XmlElementEndTag(
                        XmlName(
                            Identifier(Keywords.Summary))));
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextNewLine(
                            TriviaList(),
                            NewLine,
                            NewLine,
                            TriviaList())));
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    var typeParameterExample = typeParameterNames.Count == 1
                        ? "The option type."
                        : $"The {(i + 1).ToOrdinalWords()} option type.";

                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));

                    yield return XmlExampleElement(
                        SingletonList<XmlNodeSyntax>(
                            XmlText()
                            .WithTextTokens(
                                TokenList(
                                    XmlTextLiteral(
                                        TriviaList(),
                                        typeParameterExample,
                                        typeParameterExample,
                                        TriviaList())))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(Keywords.TypeParam)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(typeParameterNames[i]),
                                    Token(SyntaxKind.DoubleQuoteToken)))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(Keywords.TypeParam))));

                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList())));
                }
            }
        }

        static TypeParameterListSyntax GetTypeParameters(IList<string> typeParameterNames)
        {
            return TypeParameterList(SeparatedList<TypeParameterSyntax>(Join(typeParameterNames.Select(TypeParameter))));
        }

        static BaseListSyntax GetBaseList(IList<string> typeParameterNames)
        {
            return BaseList(
                SeparatedList<BaseTypeSyntax>(
                    new SyntaxNodeOrToken[]
                    {
                        SimpleBaseType(
                            IdentifierName($"I{OneOf}")),
                        Token(SyntaxKind.CommaToken),
                        SimpleBaseType(
                            GenericName(
                                Identifier(nameof(IEquatable<object>)))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        GenericName(
                                            Identifier(OneOf))
                                        .WithTypeArgumentList(
                                            GetTypeArgumentList(typeParameterNames)))))),
                    }));

            static TypeArgumentListSyntax GetTypeArgumentList(IList<string> typeParameterNames)
            {
                return TypeArgumentList(SeparatedList<TypeSyntax>(Join(typeParameterNames.Select(IdentifierName))));
            }
        }

        static IEnumerable<FieldDeclarationSyntax> GetFieldDeclarations(IList<string> typeParameterNames)
        {
            for (var i = 0; i < typeParameterNames.Count; i++)
            {
                yield return GetFieldDeclaration(typeParameterNames[i], i);
            }

            static FieldDeclarationSyntax GetFieldDeclaration(string typeParameterName, int number)
            {
                return FieldDeclaration(
                    VariableDeclaration(
                        NullableType(
                            IdentifierName(typeParameterName)))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                Identifier(GetValueName(number))))))
                    .WithModifiers(
                    TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword)));
            }
        }

        static ConstructorDeclarationSyntax GetConstructor(IList<string> typeParameterNames)
        {
            return ConstructorDeclaration(
                Identifier(OneOf))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PrivateKeyword)))
                .WithParameterList(
                ParameterList(
                    SeparatedList<ParameterSyntax>(Join(GetParameters(typeParameterNames)))))
                .WithBody(
                Block(GetAssignments(typeParameterNames.Count)));

            static IEnumerable<ParameterSyntax> GetParameters(IList<string> typeParameterNames)
            {
                yield return Parameter(Identifier(IndexVariableName)).WithType(PredefinedType(Token(SyntaxKind.IntKeyword)));

                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return Parameter(
                        Identifier(GetValueName(i)))
                        .WithType(NullableType(IdentifierName(typeParameterNames[i])))
                        .WithDefault(EqualsValueClause(
                            LiteralExpression(
                                SyntaxKind.DefaultLiteralExpression,
                                Token(SyntaxKind.DefaultKeyword))));
                }
            }

            static IEnumerable<ExpressionStatementSyntax> GetAssignments(int count)
            {
                yield return ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName(IndexPropertyName)),
                        IdentifierName(IndexVariableName)));

                for (var i = 0; i < count; i++)
                {
                    var name = GetValueIdentifierName(i);
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                name),
                            name));
                }
            }
        }

        static PropertyDeclarationSyntax GetValuePropertyDeclaration(int count)
        {
            return PropertyDeclaration(
                NullableType(
                    PredefinedType(
                        Token(SyntaxKind.ObjectKeyword))),
                Identifier(ValuePropertyName))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
                .WithExpressionBody(
                    ArrowExpressionClause(GetSwitchExpression(count)))
                .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlEmptyElement(Keywords.InheritDoc),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));

            static SwitchExpressionSyntax GetSwitchExpression(int count)
            {
                return SwitchExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        ThisExpression(),
                        IdentifierName(IndexPropertyName)))
                    .WithArms(
                    SeparatedList<SwitchExpressionArmSyntax>(GetSwitchExpressionArms(count)));

                static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(int count)
                {
                    for (var i = 0; i < count; i++)
                    {
                        yield return SwitchExpressionArm(
                            ConstantPattern(
                                LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    Literal(i))),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName(GetValueName(i))));

                        yield return Token(SyntaxKind.CommaToken);
                    }

                    yield return SwitchExpressionArm(
                        DiscardPattern(),
                        ThrowExpression(
                            ObjectCreationExpression(
                                IdentifierName(nameof(InvalidOperationException)))
                            .WithArgumentList(
                                ArgumentList())));
                    yield return Token(SyntaxKind.CommaToken);
                }
            }
        }

        static PropertyDeclarationSyntax GetIndexPropertyDeclaration()
        {
            return PropertyDeclaration(
                PredefinedType(
                    Token(SyntaxKind.IntKeyword)),
                Identifier(IndexPropertyName))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                AccessorList(
                    SingletonList(
                        AccessorDeclaration(
                            SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(
                            Token(SyntaxKind.SemicolonToken)))))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlEmptyElement(Keywords.InheritDoc),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));
        }

        static IEnumerable<PropertyDeclarationSyntax> GetIsPropertyDeclarations(IList<string> typeParameterNames)
        {
            for (var i = 0; i < typeParameterNames.Count; i++)
            {
                yield return PropertyDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.BoolKeyword)),
                    Identifier($"Is{typeParameterNames[i]}"))
                    .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                    .WithExpressionBody(
                    ArrowExpressionClause(
                        BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName(IndexPropertyName)),
                            LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                Literal(i)))))
                    .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken))
                    .WithLeadingTrivia(
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            List(
                                new XmlNodeSyntax[]
                                {
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    " Gets a value indicating whether this instance contains a ",
                                                    " Gets a value indicating whether this instance contains a ",
                                                    TriviaList()))),
                                        XmlNullKeywordElement()
                                        .WithName(
                                            XmlName(
                                                Identifier(Keywords.TypeParamRef)))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(typeParameterNames[i]),
                                                    Token(SyntaxKind.DoubleQuoteToken)))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    " value.",
                                                    " value.",
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    Space,
                                                    Space,
                                                    TriviaList()))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(Keywords.Summary))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(Keywords.Summary)))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()))),
                                }))));
            }
        }

        static IEnumerable<PropertyDeclarationSyntax> GetAsPropertyDeclarations(IList<string> typeParameterNames)
        {
            for (var i = 0; i < typeParameterNames.Count; i++)
            {
                yield return PropertyDeclaration(
                    NullableType(
                        IdentifierName(typeParameterNames[i])),
                    Identifier($"As{typeParameterNames[i]}"))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword)))
                    .WithExpressionBody(
                        ArrowExpressionClause(
                            ConditionalExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName($"Is{typeParameterNames[i]}")),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    GetValueIdentifierName(i)),
                                ThrowExpression(
                                    ObjectCreationExpression(
                                        IdentifierName(nameof(InvalidOperationException)))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList(
                                                Argument(
                                                    InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            PredefinedType(
                                                                Token(SyntaxKind.StringKeyword)),
                                                            IdentifierName(nameof(string.Format))))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SeparatedList<ArgumentSyntax>(
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    Argument(
                                                                        MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                IdentifierName(nameof(Properties)),
                                                                                IdentifierName(nameof(Properties.Resources))),
                                                                            IdentifierName(nameof(Properties.Resources.Culture)))),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    Argument(
                                                                        MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                IdentifierName(nameof(Properties)),
                                                                                IdentifierName(nameof(Properties.Resources))),
                                                                            IdentifierName(nameof(Properties.Resources.CannotReturnAsType)))),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    Argument(
                                                                        InvocationExpression(
                                                                            IdentifierName(
                                                                                Identifier(
                                                                                    TriviaList(),
                                                                                    SyntaxKind.NameOfKeyword,
                                                                                    Keywords.NameOf,
                                                                                    Keywords.NameOf,
                                                                                    TriviaList())))
                                                                        .WithArgumentList(
                                                                            ArgumentList(
                                                                                SingletonSeparatedList(
                                                                                    Argument(
                                                                                        IdentifierName(typeParameterNames[i])))))),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    Argument(
                                                                        MemberAccessExpression(
                                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                                            ThisExpression(),
                                                                            IdentifierName(IndexPropertyName))),
                                                                })))))))))))
                    .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken))
                    .WithLeadingTrivia(
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            List(
                                new XmlNodeSyntax[]
                                {
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    " Gets the value as a ",
                                                    " Gets the value as a ",
                                                    TriviaList()))),
                                        XmlNullKeywordElement()
                                        .WithName(
                                            XmlName(
                                                Identifier(Keywords.TypeParamRef)))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(typeParameterNames[i]),
                                                    Token(SyntaxKind.DoubleQuoteToken)))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    " instance.",
                                                    " instance.",
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    Space,
                                                    Space,
                                                    TriviaList()))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(Keywords.Summary))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(Keywords.Summary)))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()))),
                                }))));
            }
        }

        static IEnumerable<BaseMethodDeclarationSyntax> GetOperatorDeclarations(IList<string> typeParameterNames)
        {
            var genericName = GenericName(
                Identifier(OneOf))
                .WithTypeArgumentList(
                TypeArgumentList(
                    SeparatedList<TypeSyntax>(Join(typeParameterNames.Select(IdentifierName)))));

            for (var i = 0; i < typeParameterNames.Count; i++)
            {
                yield return ConversionOperatorDeclaration(
                    Token(SyntaxKind.ImplicitKeyword),
                    genericName)
                    .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.StaticKeyword)))
                    .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier(TVariableName))
                            .WithType(
                                NullableType(
                                    IdentifierName(typeParameterNames[i]))))))
                    .WithExpressionBody(
                    ArrowExpressionClause(
                        ImplicitObjectCreationExpression()
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                Literal(i))),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName(TVariableName))
                                        .WithNameColon(
                                            NameColon(
                                                IdentifierName(GetValueName(i)))),
                                    })))))
                    .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken))
                    .WithLeadingTrivia(
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            List(
                                new XmlNodeSyntax[]
                                {
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    " Converts an instance ",
                                                    " Converts an instance ",
                                                    TriviaList()))),
                                        XmlNullKeywordElement()
                                        .WithName(
                                            XmlName(
                                                Identifier(Keywords.TypeParamRef)))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(typeParameterNames[i]),
                                                    Token(SyntaxKind.DoubleQuoteToken)))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    " to an instance of ",
                                                    " to an instance of ",
                                                    TriviaList()))),
                                        XmlNullKeywordElement()
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlCrefAttribute(
                                                    NameMemberCref(
                                                        GenericName(
                                                            Identifier(OneOf))
                                                        .WithTypeArgumentList(
                                                            TypeArgumentList(
                                                                SeparatedList<TypeSyntax>(Join(typeParameterNames.Select(IdentifierName))))))))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    Period,
                                                    Period,
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    Space,
                                                    Space,
                                                    TriviaList()))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(Keywords.Summary))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(Keywords.Summary)))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        SingletonList<XmlNodeSyntax>(
                                            XmlText()
                                            .WithTextTokens(
                                                TokenList(
                                                    XmlTextLiteral(
                                                        TriviaList(),
                                                        "The value.",
                                                        "The value.",
                                                        TriviaList())))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(
                                                    TriviaList(),
                                                    SyntaxKind.ParamKeyword,
                                                    Keywords.Param,
                                                    Keywords.Param,
                                                    TriviaList())))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(TVariableName),
                                                    Token(SyntaxKind.DoubleQuoteToken)))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(
                                                    TriviaList(),
                                                    SyntaxKind.ParamKeyword,
                                                    Keywords.Param,
                                                    Keywords.Param,
                                                    TriviaList())))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()))),
                                }))));
            }

            // the equals
            yield return OperatorDeclaration(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword)),
                Token(SyntaxKind.EqualsEqualsToken))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.StaticKeyword)))
                .WithParameterList(
                ParameterList(
                    SeparatedList<ParameterSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            Parameter(
                                Identifier(LeftVariableName))
                            .WithType(
                                GenericName(
                                    Identifier(OneOf))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>(
                                            Join(typeParameterNames.Select(IdentifierName)))))),
                            Token(SyntaxKind.CommaToken),
                            Parameter(
                                Identifier(RightVariableName))
                            .WithType(
                                GenericName(
                                    Identifier(OneOf))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>(
                                            Join(typeParameterNames.Select(IdentifierName)))))),
                        })))
                .WithExpressionBody(
                ArrowExpressionClause(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(LeftVariableName),
                            IdentifierName(nameof(object.Equals))))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    IdentifierName(RightVariableName)))))))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    " Implements the equality operator.",
                                                    " Implements the equality operator.",
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    Space,
                                                    Space,
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(Keywords.Summary))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(Keywords.Summary)))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The left operand.",
                                                    "The left operand.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))
                                    .WithAttributes(
                                        SingletonList<XmlAttributeSyntax>(
                                            XmlNameAttribute(
                                                XmlName(
                                                    Identifier(Keywords.Name)),
                                                Token(SyntaxKind.DoubleQuoteToken),
                                                IdentifierName(LeftVariableName),
                                                Token(SyntaxKind.DoubleQuoteToken)))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The right operand.",
                                                    "The right operand.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))
                                    .WithAttributes(
                                        SingletonList<XmlAttributeSyntax>(
                                            XmlNameAttribute(
                                                XmlName(
                                                    Identifier(Keywords.Name)),
                                                Token(SyntaxKind.DoubleQuoteToken),
                                                IdentifierName(RightVariableName),
                                                Token(SyntaxKind.DoubleQuoteToken)))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The result of the operator.",
                                                    "The result of the operator.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(Keywords.Returns))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(Keywords.Returns)))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));

            // the not-equals
            yield return OperatorDeclaration(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword)),
                Token(SyntaxKind.ExclamationEqualsToken))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.StaticKeyword)))
                .WithParameterList(
                ParameterList(
                    SeparatedList<ParameterSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            Parameter(
                                Identifier(LeftVariableName))
                            .WithType(
                                GenericName(
                                    Identifier(OneOf))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>(
                                            Join(typeParameterNames.Select(IdentifierName)))))),
                            Token(SyntaxKind.CommaToken),
                            Parameter(
                                Identifier(RightVariableName))
                            .WithType(
                                GenericName(
                                    Identifier(OneOf))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>(
                                            Join(typeParameterNames.Select(IdentifierName)))))),
                        })))
                .WithExpressionBody(
                ArrowExpressionClause(
                    PrefixUnaryExpression(
                        SyntaxKind.LogicalNotExpression,
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(LeftVariableName),
                                IdentifierName(nameof(Equals))))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(
                                        IdentifierName(RightVariableName))))))))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    " Implements the inequality operator.",
                                                    " Implements the inequality operator.",
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    Space,
                                                    Space,
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(Keywords.Summary))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(Keywords.Summary)))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The left operand.",
                                                    "The left operand.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))
                                    .WithAttributes(
                                        SingletonList<XmlAttributeSyntax>(
                                            XmlNameAttribute(
                                                XmlName(
                                                    Identifier(Keywords.Name)),
                                                Token(SyntaxKind.DoubleQuoteToken),
                                                IdentifierName(LeftVariableName),
                                                Token(SyntaxKind.DoubleQuoteToken)))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The right operand.",
                                                    "The right operand.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))
                                    .WithAttributes(
                                        SingletonList<XmlAttributeSyntax>(
                                            XmlNameAttribute(
                                                XmlName(
                                                    Identifier(Keywords.Name)),
                                                Token(SyntaxKind.DoubleQuoteToken),
                                                IdentifierName(RightVariableName),
                                                Token(SyntaxKind.DoubleQuoteToken)))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The result of the operator.",
                                                    "The result of the operator.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(Keywords.Returns))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(Keywords.Returns)))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));
        }

        static MethodDeclarationSyntax GetSwitchMethodDeclaration(IList<string> typeParameterNames)
        {
            return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.VoidKeyword)),
                Identifier("Switch"))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                ParameterList(
                    SeparatedList<ParameterSyntax>(Join(GetParameters(typeParameterNames)))))
                .WithBody(
                Block(GetBody(typeParameterNames)))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(GetDocumentation(typeParameterNames)))));

            static IEnumerable<XmlNodeSyntax> GetDocumentation(IList<string> typeParameterNames)
            {
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextLiteral(
                            TriviaList(
                                DocumentationCommentExterior(TrippleSlash)),
                            Space,
                            Space,
                            TriviaList())));
                yield return XmlExampleElement(
                    SingletonList<XmlNodeSyntax>(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    " Performs an action on this instance.",
                                    " Performs an action on this instance.",
                                    TriviaList()),
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    Space,
                                    Space,
                                    TriviaList())))))
                    .WithStartTag(
                    XmlElementStartTag(
                        XmlName(
                            Identifier(Keywords.Summary))))
                    .WithEndTag(
                    XmlElementEndTag(
                        XmlName(
                            Identifier(Keywords.Summary))));
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextNewLine(
                            TriviaList(),
                            NewLine,
                            NewLine,
                            TriviaList())));
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    "The ",
                                    "The ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlCrefAttribute(
                                    NameMemberCref(
                                        GenericName(
                                            Identifier(nameof(Action)))
                                        .WithTypeArgumentList(
                                            TypeArgumentList(
                                                SingletonSeparatedList<TypeSyntax>(
                                                    IdentifierName(TTypeParameter)))))))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    " action for ",
                                    " action for ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithName(
                            XmlName(
                                Identifier(Keywords.TypeParamRef)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(typeParameterNames[i]),
                                    Token(SyntaxKind.DoubleQuoteToken)))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    Period,
                                    Period,
                                    TriviaList()))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(
                                    TriviaList(),
                                    SyntaxKind.ParamKeyword,
                                    Keywords.Param,
                                    Keywords.Param,
                                    TriviaList())))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(GetFunctionName(i)),
                                    Token(SyntaxKind.DoubleQuoteToken)))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(
                                    TriviaList(),
                                    SyntaxKind.ParamKeyword,
                                    Keywords.Param,
                                    Keywords.Param,
                                    TriviaList()))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList())));
                }
            }

            static IEnumerable<ParameterSyntax> GetParameters(IList<string> typeParameterNames)
            {
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return Parameter(
                        Identifier(GetFunctionName(i)))
                        .WithType(
                        GenericName(
                            Identifier(nameof(Action)))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList<TypeSyntax>(
                                    NullableType(
                                        IdentifierName(typeParameterNames[i]))))));
                }
            }

            static IEnumerable<StatementSyntax> GetBody(IList<string> typeParameterNames)
            {
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    var functionName = IdentifierName(GetFunctionName(i));
                    yield return IfStatement(
                        BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName($"Is{typeParameterNames[i]}")),
                            IsPatternExpression(
                                functionName,
                                UnaryPattern(
                                    ConstantPattern(
                                        LiteralExpression(
                                            SyntaxKind.NullLiteralExpression))))),
                        Block(
                            ExpressionStatement(
                                InvocationExpression(
                                    functionName)
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    ThisExpression(),
                                                    IdentifierName(GetValueName(i)))))))),
                            ReturnStatement()));
                }

                yield return ThrowStatement(
                    ObjectCreationExpression(
                        IdentifierName(nameof(InvalidOperationException)))
                    .WithArgumentList(
                        ArgumentList()));
            }
        }

        static MethodDeclarationSyntax GetMatchMethodDeclaration(IList<string> typeParameterNames)
        {
            return MethodDeclaration(
                IdentifierName(ResultsTypeParameter),
                Identifier("Match"))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
                .WithTypeParameterList(
                TypeParameterList(
                    SingletonSeparatedList(
                        TypeParameter(
                            Identifier(ResultsTypeParameter)))))
                .WithParameterList(
                ParameterList(
                    SeparatedList<ParameterSyntax>(
                        Join(GetParameters(typeParameterNames)))))
                .WithExpressionBody(
                ArrowExpressionClause(GetSwitchExpression(typeParameterNames)))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(GetDocumentation(typeParameterNames)))));

            static IEnumerable<XmlNodeSyntax> GetDocumentation(IList<string> typeParameterNames)
            {
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextLiteral(
                            TriviaList(
                                DocumentationCommentExterior(TrippleSlash)),
                            Space,
                            Space,
                            TriviaList())));
                yield return XmlExampleElement(
                    SingletonList<XmlNodeSyntax>(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    " Matches this instance.",
                                    " Matches this instance.",
                                    TriviaList()),
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    Space,
                                    Space,
                                    TriviaList())))))
                    .WithStartTag(
                    XmlElementStartTag(
                        XmlName(
                            Identifier(Keywords.Summary))))
                    .WithEndTag(
                    XmlElementEndTag(
                        XmlName(
                            Identifier(Keywords.Summary))));
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextNewLine(
                            TriviaList(),
                            NewLine,
                            NewLine,
                            TriviaList()),
                        XmlTextLiteral(
                            TriviaList(
                                DocumentationCommentExterior(TrippleSlash)),
                            Space,
                            Space,
                            TriviaList())));
                yield return XmlExampleElement(
                    SingletonList<XmlNodeSyntax>(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    "The result type.",
                                    "The result type.",
                                    TriviaList())))))
                    .WithStartTag(
                    XmlElementStartTag(
                        XmlName(
                            Identifier(Keywords.TypeParam)))
                    .WithAttributes(
                        SingletonList<XmlAttributeSyntax>(
                            XmlNameAttribute(
                                XmlName(
                                    Identifier(Keywords.Name)),
                                Token(SyntaxKind.DoubleQuoteToken),
                                IdentifierName(ResultsTypeParameter),
                                Token(SyntaxKind.DoubleQuoteToken)))))
                    .WithEndTag(
                    XmlElementEndTag(
                        XmlName(
                            Identifier(Keywords.TypeParam))));
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextNewLine(
                            TriviaList(),
                            NewLine,
                            NewLine,
                            TriviaList())));
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    "The ",
                                    "The ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlCrefAttribute(
                                    NameMemberCref(
                                        GenericName(
                                            Identifier(nameof(Func<object>)))
                                        .WithTypeArgumentList(
                                            TypeArgumentList(
                                                SeparatedList<TypeSyntax>(
                                                    new SyntaxNodeOrToken[]
                                                    {
                                                        IdentifierName(TTypeParameter),
                                                        Token(SyntaxKind.CommaToken),
                                                        IdentifierName(ResultsTypeParameter),
                                                    }))))))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    " for ",
                                    " for ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithName(
                            XmlName(
                                Identifier(Keywords.TypeParamRef)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(typeParameterNames[i]),
                                    Token(SyntaxKind.DoubleQuoteToken)))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    Period,
                                    Period,
                                    TriviaList()))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(
                                    TriviaList(),
                                    SyntaxKind.ParamKeyword,
                                    Keywords.Param,
                                    Keywords.Param,
                                    TriviaList())))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(GetFunctionName(i)),
                                    Token(SyntaxKind.DoubleQuoteToken)))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(
                                    TriviaList(),
                                    SyntaxKind.ParamKeyword,
                                    Keywords.Param,
                                    Keywords.Param,
                                    TriviaList()))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList())));
                }

                yield return XmlText()
                    .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                yield return XmlExampleElement(
                    SingletonList<XmlNodeSyntax>(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    "The matched result.",
                                    "The matched result.",
                                    TriviaList())))))
                    .WithStartTag(
                    XmlElementStartTag(
                        XmlName(
                            Identifier(Keywords.Returns))))
                    .WithEndTag(
                    XmlElementEndTag(
                        XmlName(
                            Identifier(Keywords.Returns))));
                yield return XmlText()
                    .WithTextTokens(
                    TokenList(
                        XmlTextNewLine(
                            TriviaList(),
                            NewLine,
                            NewLine,
                            TriviaList())));
            }

            static IEnumerable<ParameterSyntax> GetParameters(IList<string> typeParameterNames)
            {
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return Parameter(
                        Identifier(GetFunctionName(i)))
                        .WithType(
                        GenericName(
                            Identifier(nameof(Func<object>)))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SeparatedList<TypeSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        NullableType(
                                            IdentifierName(typeParameterNames[i])),
                                        Token(SyntaxKind.CommaToken),
                                        IdentifierName(ResultsTypeParameter),
                                    }))));
                }
            }

            static SwitchExpressionSyntax GetSwitchExpression(IList<string> typeParameterNames)
            {
                return SwitchExpression(
                    TupleExpression(
                        SeparatedList<ArgumentSyntax>(
                            Join(GetArguments(typeParameterNames)))))
                    .WithArms(
                    SeparatedList<SwitchExpressionArmSyntax>(GetSwitchExpressionArms(typeParameterNames)));

                static IEnumerable<ArgumentSyntax> GetArguments(IList<string> typeParameterNames)
                {
                    yield return Argument(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName(IndexPropertyName)));

                    for (var i = 0; i < typeParameterNames.Count; i++)
                    {
                        yield return Argument(
                            IdentifierName(GetFunctionName(i)));
                    }
                }

                static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(IList<string> typeParameterNames)
                {
                    for (var i = 0; i < typeParameterNames.Count; i++)
                    {
                        yield return SwitchExpressionArm(
                            RecursivePattern()
                            .WithPositionalPatternClause(
                                PositionalPatternClause(
                                    SeparatedList<SubpatternSyntax>(Join(GetSubpatterns(i, typeParameterNames.Count))))),
                            InvocationExpression(
                                IdentifierName(GetFunctionName(i)))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList(
                                        Argument(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                ThisExpression(),
                                                IdentifierName(GetValueName(i))))))));

                        yield return Token(SyntaxKind.CommaToken);

                        static IEnumerable<SubpatternSyntax> GetSubpatterns(int index, int count)
                        {
                            yield return Subpattern(
                                ConstantPattern(
                                    LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        Literal(index))));

                            for (var i = 0; i < count; i++)
                            {
                                yield return i == index
                                    ? Subpattern(
                                        UnaryPattern(
                                            ConstantPattern(
                                                LiteralExpression(
                                                    SyntaxKind.NullLiteralExpression))))
                                    : Subpattern(DiscardPattern());
                            }
                        }
                    }

                    yield return SwitchExpressionArm(
                        DiscardPattern(),
                        ThrowExpression(
                            ObjectCreationExpression(
                                IdentifierName(nameof(InvalidOperationException)))
                            .WithArgumentList(
                                ArgumentList())));
                    yield return Token(SyntaxKind.CommaToken);
                }
            }
        }

        static IEnumerable<MethodDeclarationSyntax> GetMapMethodDeclarations(IList<string> typeParameterNames)
        {
            const string MapFunctionName = "mapFunc";

            for (var i = 0; i < typeParameterNames.Count; i++)
            {
                var types = GetTypeArguments(i, typeParameterNames);

                yield return MethodDeclaration(
                    GenericName(
                        Identifier(OneOf))
                    .WithTypeArgumentList(
                        TypeArgumentList(
                            SeparatedList<TypeSyntax>(Join(types)))),
                    Identifier($"Map{typeParameterNames[i]}"))
                    .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                    .WithTypeParameterList(
                    TypeParameterList(
                        SingletonSeparatedList(
                            TypeParameter(
                                Identifier(ResultsTypeParameter)))))
                    .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                Identifier(MapFunctionName))
                            .WithType(
                                GenericName(
                                    Identifier(nameof(Func<object>)))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>(
                                            new SyntaxNodeOrToken[]
                                            {
                                                NullableType(
                                                    IdentifierName(typeParameterNames[i])),
                                                Token(SyntaxKind.CommaToken),
                                                IdentifierName(ResultsTypeParameter),
                                            })))))))
                    .WithBody(
                    Block(
                        GetStatements(i, typeParameterNames)))
                    .WithLeadingTrivia(
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            List(
                                GetDocumentation(typeParameterNames[i], types)))));

                static IEnumerable<TypeSyntax> GetTypeArguments(int index, IList<string> typeParameterNames)
                {
                    for (var i = 0; i < typeParameterNames.Count; i++)
                    {
                        yield return i == index
                            ? IdentifierName(ResultsTypeParameter)
                            : (TypeSyntax)IdentifierName(typeParameterNames[i]);
                    }
                }

                static IEnumerable<XmlNodeSyntax> GetDocumentation(string typeParameterName, IEnumerable<TypeSyntax> typeParameters)
                {
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    " Maps the ",
                                    " Maps the ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithName(
                            XmlName(
                                Identifier(Keywords.TypeParamRef)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(typeParameterName),
                                    Token(SyntaxKind.DoubleQuoteToken)))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    " instance through the function.",
                                    " instance through the function.",
                                    TriviaList()),
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList()),
                                XmlTextLiteral(
                                    TriviaList(
                                        DocumentationCommentExterior(TrippleSlash)),
                                    Space,
                                    Space,
                                    TriviaList()))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(Keywords.Summary))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(Keywords.Summary))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList())));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        SingletonList<XmlNodeSyntax>(
                            XmlText()
                            .WithTextTokens(
                                TokenList(
                                    XmlTextLiteral(
                                        TriviaList(),
                                        "The type of result.",
                                        "The type of result.",
                                        TriviaList())))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(Keywords.TypeParam)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(ResultsTypeParameter),
                                    Token(SyntaxKind.DoubleQuoteToken)))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(Keywords.TypeParam))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList()),
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        SingletonList<XmlNodeSyntax>(
                            XmlText()
                            .WithTextTokens(
                                TokenList(
                                    XmlTextLiteral(
                                        TriviaList(),
                                        "The map function.",
                                        "The map function.",
                                        TriviaList())))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(
                                    TriviaList(),
                                    SyntaxKind.ParamKeyword,
                                    Keywords.Param,
                                    Keywords.Param,
                                    TriviaList())))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(MapFunctionName),
                                    Token(SyntaxKind.DoubleQuoteToken)))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(
                                    TriviaList(),
                                    SyntaxKind.ParamKeyword,
                                    Keywords.Param,
                                    Keywords.Param,
                                    TriviaList()))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList())));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    "A new instance of ",
                                    "A new instance of ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlCrefAttribute(
                                    NameMemberCref(
                                        GenericName(
                                            Identifier(OneOf))
                                        .WithTypeArgumentList(
                                            TypeArgumentList(
                                                SeparatedList<TypeSyntax>(Join(typeParameters)))))))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    Period,
                                    Period,
                                    TriviaList()))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(Keywords.Returns))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(Keywords.Returns))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList()),
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        XmlNullKeywordElement()
                        .WithName(
                            XmlName(
                                Identifier(Keywords.ParamRef)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(MapFunctionName),
                                    Token(SyntaxKind.DoubleQuoteToken)))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    " is ",
                                    " is ",
                                    TriviaList()))),
                        XmlNullKeywordElement(),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    Period,
                                    Period,
                                    TriviaList()))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(ExceptionVariableName)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlCrefAttribute(
                                    NameMemberCref(
                                        IdentifierName(nameof(ArgumentNullException)))))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(ExceptionVariableName))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList()),
                            XmlTextLiteral(
                                TriviaList(
                                    DocumentationCommentExterior(TrippleSlash)),
                                Space,
                                Space,
                                TriviaList())));
                    yield return XmlExampleElement(
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    "This instance does not represent a ",
                                    "This instance does not represent a ",
                                    TriviaList()))),
                        XmlNullKeywordElement()
                        .WithName(
                            XmlName(
                                Identifier(Keywords.TypeParamRef)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlNameAttribute(
                                    XmlName(
                                        Identifier(Keywords.Name)),
                                    Token(SyntaxKind.DoubleQuoteToken),
                                    IdentifierName(typeParameterName),
                                    Token(SyntaxKind.DoubleQuoteToken)))),
                        XmlText()
                        .WithTextTokens(
                            TokenList(
                                XmlTextLiteral(
                                    TriviaList(),
                                    " instance.",
                                    " instance.",
                                    TriviaList()))))
                        .WithStartTag(
                        XmlElementStartTag(
                            XmlName(
                                Identifier(ExceptionVariableName)))
                        .WithAttributes(
                            SingletonList<XmlAttributeSyntax>(
                                XmlCrefAttribute(
                                    NameMemberCref(
                                        IdentifierName(nameof(InvalidOperationException)))))))
                        .WithEndTag(
                        XmlElementEndTag(
                            XmlName(
                                Identifier(ExceptionVariableName))));
                    yield return XmlText()
                        .WithTextTokens(
                        TokenList(
                            XmlTextNewLine(
                                TriviaList(),
                                NewLine,
                                NewLine,
                                TriviaList())));
                }

                static IEnumerable<StatementSyntax> GetStatements(int index, IList<string> typeParameterNames)
                {
                    yield return IfStatement(
                        IsPatternExpression(
                            IdentifierName(MapFunctionName),
                            ConstantPattern(
                                LiteralExpression(
                                    SyntaxKind.NullLiteralExpression))),
                        Block(
                            SingletonList<StatementSyntax>(
                                ThrowStatement(
                                    ObjectCreationExpression(
                                        IdentifierName(nameof(ArgumentNullException)))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList(
                                                Argument(
                                                    InvocationExpression(
                                                        IdentifierName(
                                                            Identifier(
                                                                TriviaList(),
                                                                SyntaxKind.NameOfKeyword,
                                                                Keywords.NameOf,
                                                                Keywords.NameOf,
                                                                TriviaList())))
                                                    .WithArgumentList(
                                                        ArgumentList(
                                                            SingletonSeparatedList(
                                                                Argument(
                                                                    IdentifierName(MapFunctionName)))))))))))));

                    yield return ReturnStatement(GetSwitchExpression(index, typeParameterNames));

                    static SwitchExpressionSyntax GetSwitchExpression(int index, IList<string> typeParameterNames)
                    {
                        return SwitchExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName(IndexPropertyName)))
                            .WithArms(
                            SeparatedList<SwitchExpressionArmSyntax>(GetSwitchExpressionArms(index, typeParameterNames)));

                        static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(int index, IList<string> typeParameterNames)
                        {
                            for (var i = 0; i < typeParameterNames.Count; i++)
                            {
                                yield return i == index
                                    ? (SyntaxNodeOrToken)SwitchExpressionArm(
                                        ConstantPattern(
                                            LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                Literal(i))),
                                        InvocationExpression(
                                            IdentifierName(MapFunctionName))
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList(
                                                    Argument(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            ThisExpression(),
                                                            IdentifierName($"As{typeParameterNames[i]}")))))))
                                    : (SyntaxNodeOrToken)SwitchExpressionArm(
                                        ConstantPattern(
                                            LiteralExpression(
                                                SyntaxKind.NumericLiteralExpression,
                                                Literal(i))),
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName($"As{typeParameterNames[i]}")));

                                yield return Token(SyntaxKind.CommaToken);
                            }

                            yield return SwitchExpressionArm(
                                DiscardPattern(),
                                ThrowExpression(
                                    ObjectCreationExpression(
                                        IdentifierName(nameof(InvalidOperationException)))
                                    .WithArgumentList(
                                        ArgumentList())));
                            yield return Token(SyntaxKind.CommaToken);
                        }
                    }
                }
            }
        }

        static IEnumerable<MethodDeclarationSyntax> GetTryPickMethodDeclarations(IList<string> typeParameterNames)
        {
            if (typeParameterNames.Count < 2)
            {
                yield break;
            }

            for (var i = 0; i < typeParameterNames.Count; i++)
            {
                yield return MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.BoolKeyword)),
                    Identifier($"TryPick{typeParameterNames[i]}"))
                    .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                    .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(Join(GetParameters(i, typeParameterNames)))))
                    .WithBody(
                    Block(
                        GetStatements(i, typeParameterNames)))
                    .WithLeadingTrivia(
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            List(
                                new XmlNodeSyntax[]
                                {
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    " Tries to pick the value as a ",
                                                    " Tries to pick the value as a ",
                                                    TriviaList()))),
                                        XmlNullKeywordElement()
                                        .WithName(
                                            XmlName(
                                                Identifier(Keywords.TypeParamRef)))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(typeParameterNames[i]),
                                                    Token(SyntaxKind.DoubleQuoteToken)))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    Period,
                                                    Period,
                                                    TriviaList()),
                                                XmlTextNewLine(
                                                    TriviaList(),
                                                    NewLine,
                                                    NewLine,
                                                    TriviaList()),
                                                XmlTextLiteral(
                                                    TriviaList(
                                                        DocumentationCommentExterior(TrippleSlash)),
                                                    Space,
                                                    Space,
                                                    TriviaList()))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(Keywords.Summary))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(Keywords.Summary)))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        SingletonList<XmlNodeSyntax>(
                                            XmlText()
                                            .WithTextTokens(
                                                TokenList(
                                                    XmlTextLiteral(
                                                        TriviaList(),
                                                        "The value.",
                                                        "The value.",
                                                        TriviaList())))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(
                                                    TriviaList(),
                                                    SyntaxKind.ParamKeyword,
                                                    Keywords.Param,
                                                    Keywords.Param,
                                                    TriviaList())))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(ValueVariableName),
                                                    Token(SyntaxKind.DoubleQuoteToken)))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(
                                                    TriviaList(),
                                                    SyntaxKind.ParamKeyword,
                                                    Keywords.Param,
                                                    Keywords.Param,
                                                    TriviaList())))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        SingletonList<XmlNodeSyntax>(
                                            XmlText()
                                            .WithTextTokens(
                                                TokenList(
                                                    XmlTextLiteral(
                                                        TriviaList(),
                                                        "The remainder.",
                                                        "The remainder.",
                                                        TriviaList())))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(
                                                    TriviaList(),
                                                    SyntaxKind.ParamKeyword,
                                                    Keywords.Param,
                                                    Keywords.Param,
                                                    TriviaList())))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlNameAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Name)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    IdentifierName(RemainderVariableName),
                                                    Token(SyntaxKind.DoubleQuoteToken)))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(
                                                    TriviaList(),
                                                    SyntaxKind.ParamKeyword,
                                                    Keywords.Param,
                                                    Keywords.Param,
                                                    TriviaList())))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        XmlNullKeywordElement()
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlTextAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Langword)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    Token(SyntaxKind.DoubleQuoteToken))
                                                .WithTextTokens(
                                                    TokenList(
                                                        XmlTextLiteral(
                                                            TriviaList(),
                                                            Keywords.True,
                                                            Keywords.True,
                                                            TriviaList()))))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    " upon success; otherwise ",
                                                    " upon success; otherwise ",
                                                    TriviaList()))),
                                        XmlNullKeywordElement()
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlTextAttribute(
                                                    XmlName(
                                                        Identifier(Keywords.Langword)),
                                                    Token(SyntaxKind.DoubleQuoteToken),
                                                    Token(SyntaxKind.DoubleQuoteToken))
                                                .WithTextTokens(
                                                    TokenList(
                                                        XmlTextLiteral(
                                                            TriviaList(),
                                                            Keywords.False,
                                                            Keywords.False,
                                                            TriviaList()))))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    Period,
                                                    Period,
                                                    TriviaList()))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(Keywords.Returns))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(Keywords.Returns)))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()),
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlExampleElement(
                                        XmlNullKeywordElement()
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlCrefAttribute(
                                                    NameMemberCref(
                                                        IdentifierName(IndexPropertyName))))),
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    " is out of range.",
                                                    " is out of range.",
                                                    TriviaList()))))
                                    .WithStartTag(
                                        XmlElementStartTag(
                                            XmlName(
                                                Identifier(ExceptionVariableName)))
                                        .WithAttributes(
                                            SingletonList<XmlAttributeSyntax>(
                                                XmlCrefAttribute(
                                                    NameMemberCref(
                                                        IdentifierName(nameof(InvalidOperationException)))))))
                                    .WithEndTag(
                                        XmlElementEndTag(
                                            XmlName(
                                                Identifier(ExceptionVariableName)))),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()))),
                                }))));

                static IEnumerable<ParameterSyntax> GetParameters(int index, IList<string> typeParameterNames)
                {
                    yield return Parameter(
                        Identifier(ValueVariableName))
                        .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.OutKeyword)))
                        .WithType(
                        NullableType(
                            IdentifierName(typeParameterNames[index])));
                    yield return Parameter(
                        Identifier(RemainderVariableName))
                        .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.OutKeyword)))
                        .WithType(
                        GenericName(
                            Identifier(OneOf))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SeparatedList<TypeSyntax>(
                                    Join(typeParameterNames.Where((_, i) => i != index).Select(IdentifierName))))));
                }

                static IEnumerable<StatementSyntax> GetStatements(int index, IList<string> typeParameterNames)
                {
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(ValueVariableName),
                            ConditionalExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName($"Is{typeParameterNames[index]}")),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName($"As{typeParameterNames[index]}")),
                                LiteralExpression(
                                    SyntaxKind.DefaultLiteralExpression,
                                    Token(SyntaxKind.DefaultKeyword)))));
                    yield return ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName(RemainderVariableName),
                            SwitchExpression(
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    ThisExpression(),
                                    IdentifierName(IndexPropertyName)))
                            .WithArms(
                                SeparatedList<SwitchExpressionArmSyntax>(GetSwitchExpressionArms(index, typeParameterNames)))));
                    yield return ReturnStatement(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName($"Is{typeParameterNames[index]}")));

                    static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(int index, IList<string> typeParameterNames)
                    {
                        for (var i = 0; i < typeParameterNames.Count; i++)
                        {
                            yield return i == index
                                ? (SyntaxNodeOrToken)SwitchExpressionArm(
                                    ConstantPattern(
                                        LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            Literal(i))),
                                    LiteralExpression(
                                        SyntaxKind.DefaultLiteralExpression,
                                        Token(SyntaxKind.DefaultKeyword)))
                                : (SyntaxNodeOrToken)SwitchExpressionArm(
                                    ConstantPattern(
                                        LiteralExpression(
                                            SyntaxKind.NumericLiteralExpression,
                                            Literal(i))),
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        ThisExpression(),
                                        IdentifierName($"As{typeParameterNames[i]}")));

                            yield return Token(SyntaxKind.CommaToken);
                        }

                        yield return SwitchExpressionArm(
                            DiscardPattern(),
                            ThrowExpression(
                                ObjectCreationExpression(
                                    IdentifierName(nameof(InvalidOperationException)))
                                .WithArgumentList(
                                    ArgumentList())));
                        yield return Token(SyntaxKind.CommaToken);
                    }
                }
            }
        }

        static IEnumerable<MethodDeclarationSyntax> GetEqualsMethodDeclarations(IList<string> typeParameterNames)
        {
            yield return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword)),
                Identifier(nameof(object.Equals)))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(
                            Identifier(OtherVariableName))
                        .WithType(
                            GenericName(
                                Identifier(OneOf))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(
                                        Join(typeParameterNames.Select(IdentifierName)))))))))
                .WithExpressionBody(
                ArrowExpressionClause(
                    BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        BinaryExpression(
                            SyntaxKind.EqualsExpression,
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName(IndexPropertyName)),
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName(OtherVariableName),
                                IdentifierName(IndexPropertyName))),
                        GetSwitchExpression(typeParameterNames))))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlEmptyElement(Keywords.InheritDoc),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));

            yield return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.BoolKeyword)),
                Identifier(nameof(Equals)))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.OverrideKeyword)))
                .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(
                            Identifier(ObjVariableName))
                        .WithType(
                            NullableType(
                                PredefinedType(
                                    Token(SyntaxKind.ObjectKeyword)))))))
                .WithExpressionBody(
                ArrowExpressionClause(
                    BinaryExpression(
                        SyntaxKind.LogicalAndExpression,
                        IsPatternExpression(
                            IdentifierName(ObjVariableName),
                            DeclarationPattern(
                                GenericName(
                                    Identifier(OneOf))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SeparatedList<TypeSyntax>(
                                            Join(typeParameterNames.Select(IdentifierName))))),
                                SingleVariableDesignation(
                                    Identifier(OVariableName)))),
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName(nameof(object.Equals))))
                        .WithArgumentList(
                            ArgumentList(
                                SingletonSeparatedList(
                                    Argument(
                                        IdentifierName(OVariableName))))))))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                    Trivia(
                        DocumentationCommentTrivia(
                            SyntaxKind.SingleLineDocumentationCommentTrivia,
                            List(
                                new XmlNodeSyntax[]
                                {
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextLiteral(
                                                TriviaList(
                                                    DocumentationCommentExterior(TrippleSlash)),
                                                Space,
                                                Space,
                                                TriviaList()))),
                                    XmlEmptyElement(Keywords.InheritDoc),
                                    XmlText()
                                    .WithTextTokens(
                                        TokenList(
                                            XmlTextNewLine(
                                                TriviaList(),
                                                NewLine,
                                                NewLine,
                                                TriviaList()))),
                                }))));

            static SwitchExpressionSyntax GetSwitchExpression(IList<string> typeParameterNames)
            {
                return SwitchExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        ThisExpression(),
                        IdentifierName(IndexPropertyName)))
                    .WithArms(
                    SeparatedList<SwitchExpressionArmSyntax>(
                        GetSwitchExpressionArms(typeParameterNames)));

                static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(IList<string> typeParameterNames)
                {
                    for (var i = 0; i < typeParameterNames.Count; i++)
                    {
                        yield return SwitchExpressionArm(
                            ConstantPattern(
                                LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    Literal(i))),
                            InvocationExpression(
                                IdentifierName(nameof(object.Equals)))
                            .WithArgumentList(
                                ArgumentList(
                                    SeparatedList<ArgumentSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    ThisExpression(),
                                                    IdentifierName(GetValueName(i)))),
                                            Token(SyntaxKind.CommaToken),
                                            Argument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName(OtherVariableName),
                                                    IdentifierName(GetValueName(i)))),
                                        }))));
                        yield return Token(SyntaxKind.CommaToken);
                    }

                    yield return SwitchExpressionArm(
                        DiscardPattern(),
                        LiteralExpression(
                            SyntaxKind.FalseLiteralExpression));
                    yield return Token(SyntaxKind.CommaToken);
                }
            }
        }

        static MethodDeclarationSyntax GetToStringMethodDeclaration()
        {
            return MethodDeclaration(
                NullableType(
                    PredefinedType(
                        Token(SyntaxKind.StringKeyword))),
                Identifier(nameof(ToString)))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.OverrideKeyword)))
                .WithExpressionBody(
                ArrowExpressionClause(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName(nameof(ToString))))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList(
                                Argument(
                                    LiteralExpression(
                                        SyntaxKind.NullLiteralExpression))
                                .WithNameColon(
                                    NameColon(
                                        IdentifierName(ProviderParameterName))))))))
                .WithSemicolonToken(
                Token(SyntaxKind.SemicolonToken))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlEmptyElement(Keywords.InheritDoc),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));
        }

        static MethodDeclarationSyntax GetToStringWithProviderMethodDeclaration(IList<string> typeParameterNames)
        {
            const string FormatValue = nameof(FormatValue);

            return MethodDeclaration(
                NullableType(
                    PredefinedType(
                        Token(SyntaxKind.StringKeyword))),
                Identifier(nameof(ToString)))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(
                            Identifier(ProviderParameterName))
                        .WithType(
                            NullableType(
                                IdentifierName(nameof(IFormatProvider)))))))
                .WithBody(
                Block(
                    ReturnStatement(
                        SwitchExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                ThisExpression(),
                                IdentifierName(IndexPropertyName)))
                        .WithArms(
                            SeparatedList<SwitchExpressionArmSyntax>(
                                GetSwitchExpressionArms(typeParameterNames)))),
                    LocalFunctionStatement(
                        PredefinedType(
                            Token(SyntaxKind.StringKeyword)),
                        Identifier(FormatValue))
                    .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.StaticKeyword)))
                    .WithTypeParameterList(
                        TypeParameterList(
                            SingletonSeparatedList(
                                TypeParameter(
                                    Identifier(TTypeParameter)))))
                    .WithParameterList(
                        ParameterList(
                            SeparatedList<ParameterSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                    Parameter(
                                        Identifier(ValueVariableName))
                                    .WithType(
                                        IdentifierName(TTypeParameter)),
                                    Token(SyntaxKind.CommaToken),
                                    Parameter(
                                        Identifier(ProviderParameterName))
                                    .WithType(
                                        NullableType(
                                            IdentifierName(nameof(IFormatProvider)))),
                                })))
                    .WithBody(
                        Block(
                            SingletonList<StatementSyntax>(
                                ReturnStatement(
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            PredefinedType(
                                                Token(SyntaxKind.StringKeyword)),
                                            IdentifierName(nameof(string.Format))))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SeparatedList<ArgumentSyntax>(
                                                new SyntaxNodeOrToken[]
                                                {
                                                    Argument(
                                                        IdentifierName(ProviderParameterName)),
                                                    Token(SyntaxKind.CommaToken),
                                                    Argument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal("{0}: {1}"))),
                                                    Token(SyntaxKind.CommaToken),
                                                    Argument(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            TypeOfExpression(
                                                                IdentifierName(TTypeParameter)),
                                                            IdentifierName(nameof(Type.FullName)))),
                                                    Token(SyntaxKind.CommaToken),
                                                    Argument(
                                                        IdentifierName(ValueVariableName)),
                                                })))))))))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlNullKeywordElement()
                                .WithName(
                                    XmlName(
                                        Identifier(Keywords.InheritDoc)))
                                .WithAttributes(
                                    SingletonList<XmlAttributeSyntax>(
                                        XmlCrefAttribute(
                                            NameMemberCref(
                                                IdentifierName(nameof(ToString)))
                                            .WithParameters(
                                                CrefParameterList())))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()),
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlExampleElement(
                                    SingletonList<XmlNodeSyntax>(
                                        XmlText()
                                        .WithTextTokens(
                                            TokenList(
                                                XmlTextLiteral(
                                                    TriviaList(),
                                                    "The format provider.",
                                                    "The format provider.",
                                                    TriviaList())))))
                                .WithStartTag(
                                    XmlElementStartTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))
                                    .WithAttributes(
                                        SingletonList<XmlAttributeSyntax>(
                                            XmlNameAttribute(
                                                XmlName(
                                                    Identifier(Keywords.Name)),
                                                Token(SyntaxKind.DoubleQuoteToken),
                                                IdentifierName(ProviderParameterName),
                                                Token(SyntaxKind.DoubleQuoteToken)))))
                                .WithEndTag(
                                    XmlElementEndTag(
                                        XmlName(
                                            Identifier(
                                                TriviaList(),
                                                SyntaxKind.ParamKeyword,
                                                Keywords.Param,
                                                Keywords.Param,
                                                TriviaList())))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));

            static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(IList<string> typeParameterNames)
            {
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return SwitchExpressionArm(
                        ConstantPattern(
                            LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                Literal(i))),
                        InvocationExpression(
                            IdentifierName(FormatValue))
                        .WithArgumentList(
                            ArgumentList(
                                SeparatedList<ArgumentSyntax>(
                                    new SyntaxNodeOrToken[]
                                    {
                                        Argument(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                ThisExpression(),
                                                IdentifierName(GetValueName(i)))),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            IdentifierName(ProviderParameterName)),
                                    }))));
                    yield return Token(SyntaxKind.CommaToken);
                }

                yield return SwitchExpressionArm(
                    DiscardPattern(),
                    LiteralExpression(
                        SyntaxKind.DefaultLiteralExpression,
                        Token(SyntaxKind.DefaultKeyword)));
                yield return Token(SyntaxKind.CommaToken);
            }
        }

        static MethodDeclarationSyntax GetGetHashCodeMethodDefinition(IList<string> typeParameterNames)
        {
            const string Suffix = "Core";
            const string HashCodeVariableName = "hashCode";

            return MethodDeclaration(
                PredefinedType(
                    Token(SyntaxKind.IntKeyword)),
                Identifier(nameof(GetHashCode)))
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.OverrideKeyword)))
                .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        CheckedStatement(
                            SyntaxKind.UncheckedStatement,
                            Block(
                                ReturnStatement(
                                    BinaryExpression(
                                        SyntaxKind.ExclusiveOrExpression,
                                        ParenthesizedExpression(
                                            BinaryExpression(
                                                SyntaxKind.MultiplyExpression,
                                                InvocationExpression(
                                                    IdentifierName(nameof(GetHashCode) + Suffix))
                                                .WithArgumentList(
                                                    ArgumentList(
                                                        SeparatedList<ArgumentSyntax>(
                                                            Join(GetArguments(typeParameterNames))))),
                                                LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    Literal(397)))),
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            ThisExpression(),
                                            IdentifierName(IndexPropertyName)))),
                                LocalFunctionStatement(
                                    PredefinedType(
                                        Token(SyntaxKind.IntKeyword)),
                                    Identifier(nameof(GetHashCode) + Suffix))
                                .WithModifiers(
                                    TokenList(
                                        Token(SyntaxKind.StaticKeyword)))
                                .WithParameterList(
                                    ParameterList(
                                        SeparatedList<ParameterSyntax>(
                                            Join(GetParameters(typeParameterNames)))))
                                .WithBody(
                                    Block(
                                        LocalDeclarationStatement(
                                            VariableDeclaration(
                                                IdentifierName(
                                                    Identifier(
                                                        TriviaList(),
                                                        SyntaxKind.VarKeyword,
                                                        Keywords.Var,
                                                        Keywords.Var,
                                                        TriviaList())))
                                            .WithVariables(
                                                SingletonSeparatedList(
                                                    VariableDeclarator(
                                                        Identifier(HashCodeVariableName))
                                                    .WithInitializer(
                                                        EqualsValueClause(
                                                            SwitchExpression(
                                                                IdentifierName(IndexVariableName))
                                                            .WithArms(
                                                                SeparatedList<SwitchExpressionArmSyntax>(
                                                                    GetSwitchExpressionArms(typeParameterNames)))))))),
                                        ReturnStatement(
                                            BinaryExpression(
                                                SyntaxKind.CoalesceExpression,
                                                IdentifierName(HashCodeVariableName),
                                                LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    Literal(0)))))))))))
                .WithLeadingTrivia(
                Trivia(
                    DocumentationCommentTrivia(
                        SyntaxKind.SingleLineDocumentationCommentTrivia,
                        List(
                            new XmlNodeSyntax[]
                            {
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(
                                                DocumentationCommentExterior(TrippleSlash)),
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlEmptyElement(Keywords.InheritDoc),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextNewLine(
                                            TriviaList(),
                                            NewLine,
                                            NewLine,
                                            TriviaList()))),
                            }))));

            static IEnumerable<ParameterSyntax> GetParameters(IList<string> typeParameterNames)
            {
                yield return Parameter(
                    Identifier(IndexVariableName))
                    .WithType(
                    PredefinedType(
                        Token(SyntaxKind.IntKeyword)));

                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return Parameter(
                        Identifier(GetValueName(i)))
                        .WithType(
                        NullableType(
                            IdentifierName(typeParameterNames[i])));
                }
            }

            static IEnumerable<ArgumentSyntax> GetArguments(IList<string> typeParameterNames)
            {
                yield return Argument(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        ThisExpression(),
                        IdentifierName(IndexPropertyName)));

                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return Argument(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            ThisExpression(),
                            IdentifierName(GetValueName(i))));
                }
            }

            static IEnumerable<SyntaxNodeOrToken> GetSwitchExpressionArms(IList<string> typeParameterNames)
            {
                for (var i = 0; i < typeParameterNames.Count; i++)
                {
                    yield return SwitchExpressionArm(
                        ConstantPattern(
                            LiteralExpression(
                                SyntaxKind.NumericLiteralExpression,
                                Literal(i))),
                        ConditionalAccessExpression(
                            IdentifierName(GetValueName(i)),
                            InvocationExpression(
                                MemberBindingExpression(
                                    IdentifierName(nameof(GetHashCode))))));
                    yield return Token(SyntaxKind.CommaToken);
                }

                yield return SwitchExpressionArm(
                    DiscardPattern(),
                    LiteralExpression(
                        SyntaxKind.DefaultLiteralExpression,
                        Token(SyntaxKind.DefaultKeyword)));
                yield return Token(SyntaxKind.CommaToken);
            }
        }

        static IdentifierNameSyntax GetValueIdentifierName(int number)
        {
            return IdentifierName(GetValueName(number));
        }

        static string GetValueName(int number)
        {
            return GetName(ValueVariableName, number);
        }

        static string GetFunctionName(int number)
        {
            return GetName("f", number);
        }

        static MemberAccessExpressionSyntax GetMemberAccessExpression<T>(T value)
            where T : Enum
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                GetBaseMemberAccessExpression(GetNames(typeof(T)).ToList()),
                IdentifierName(Enum.GetName(typeof(T), value)));

            static MemberAccessExpressionSyntax GetBaseMemberAccessExpression(IList<IdentifierNameSyntax> names)
            {
                var memberAccessExpression = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    names[0],
                    names[1]);

                for (var i = 2; i < names.Count; i++)
                {
                    memberAccessExpression = MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        memberAccessExpression,
                        names[i]);
                }

                return memberAccessExpression;
            }
        }
    }
}