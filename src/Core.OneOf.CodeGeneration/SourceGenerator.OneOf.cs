// -----------------------------------------------------------------------
// <copyright file="SourceGenerator.OneOf.cs" company="Altemiq">
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
/// Methods for OneOf class.
/// </content>
public partial class SourceGenerator
{
    private static (string Name, ClassDeclarationSyntax ClassDeclaration) GenerateOneOf(int count)
    {
        var classDeclaraion = ClassDeclaration(OneOf)
            .WithAttributeLists(
            List(
                GetAttributes()))
            .WithModifiers(
            TokenList(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword)))
            .WithMembers(List(GetMemberDeclarations(count)
            .Append(GetNoneStructDeclaration(count))))
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
                                            Space,
                                            Space,
                                            TriviaList()))),
                                XmlNullKeywordElement()
                                .WithAttributes(
                                    SingletonList<XmlAttributeSyntax>(
                                        XmlCrefAttribute(
                                            NameMemberCref(
                                                IdentifierName($"I{OneOf}"))))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(),
                                            " methods.",
                                            " methods.",
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

        return (OneOf, classDeclaraion);

        static IEnumerable<AttributeListSyntax> GetAttributes()
        {
            yield return AttributeList(
                SingletonSeparatedList(
                    GetGeneratedCodeAttribute()));
        }

        static IEnumerable<MemberDeclarationSyntax> GetMemberDeclarations(int count)
        {
            static IEnumerable<TypeSyntax> GetTypes(int count)
            {
                return GetTypeParameterNames(count).Select(IdentifierName);
            }

            for (var i = 0; i < count; i++)
            {
                foreach (var declaration in GetSubMemberDeclarations(i + 1))
                {
                    yield return declaration;
                }

                static IEnumerable<MemberDeclarationSyntax> GetSubMemberDeclarations(int count)
                {
                    for (var i = 0; i < count; i++)
                    {
                        const string InputVariableName = "input";

                        yield return MethodDeclaration(
                            GenericName(
                                Identifier(OneOf))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList<TypeSyntax>(Join(GetTypes(count))))),
                            Identifier("From"))
                            .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PublicKeyword),
                                Token(SyntaxKind.StaticKeyword)))
                            .WithTypeParameterList(
                            TypeParameterList(
                                SeparatedList<TypeParameterSyntax>(
                                    Join(GetTypeParameterNames(count).Select(TypeParameter)))))
                            .WithParameterList(
                            ParameterList(
                                SingletonSeparatedList(
                                    Parameter(
                                        Identifier(InputVariableName))
                                    .WithType(
                                        IdentifierName(GetTypeParameterName(i))))))
                            .WithExpressionBody(
                            ArrowExpressionClause(
                                IdentifierName(InputVariableName)))
                            .WithSemicolonToken(
                            Token(SyntaxKind.SemicolonToken))
                            .WithLeadingTrivia(
                            Trivia(
                                DocumentationCommentTrivia(
                                    SyntaxKind.SingleLineDocumentationCommentTrivia,
                                    List(GetDocumentation(i, count)))));

                        static IEnumerable<XmlNodeSyntax> GetDocumentation(int index, int count)
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
                                            " Converts a ",
                                            " Converts a ",
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
                                            IdentifierName(GetTypeParameterName(index)),
                                            Token(SyntaxKind.DoubleQuoteToken)))),
                                XmlText()
                                .WithTextTokens(
                                    TokenList(
                                        XmlTextLiteral(
                                            TriviaList(),
                                            " to a ",
                                            " to a ",
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
                                                        SeparatedList<TypeSyntax>(
                                                            Join(GetTypes(count))))))))),
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
                                        Identifier(Keywords.Summary))));
                            yield return XmlText()
                                .WithTextTokens(
                                TokenList(
                                    XmlTextNewLine(
                                        TriviaList(),
                                        NewLine,
                                        NewLine,
                                        TriviaList())));

                            for (var i = 0; i < count; i++)
                            {
                                var example = count == 1
                                    ? "The type parameter."
                                    : $"The {(i + 1).ToOrdinalWords()} type parameter.";

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
                                                    example,
                                                    example,
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
                                                IdentifierName(GetTypeParameterName(i)),
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
                                            IdentifierName(InputVariableName),
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
                                            "The ",
                                            "The ",
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
                                                        SeparatedList<TypeSyntax>(
                                                            Join(GetTypes(count))))))))),
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
                                        TriviaList())));
                        }
                    }
                }
            }
        }

        static StructDeclarationSyntax GetNoneStructDeclaration(int count)
        {
            const string None = nameof(None);

            return StructDeclaration(None)
                .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.ReadOnlyKeyword)))
                .WithMembers(
                List(GetMemberDeclarations(count)))
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
                                                    " Value of none.",
                                                    " Value of none.",
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
                                            TriviaList()))),
                            }))));

            static IEnumerable<MemberDeclarationSyntax> GetMemberDeclarations(int count)
            {
                for (var i = 1; i < count; i++)
                {
                    var typeParameterNames = GetTypeParameterNames(i);
                    var fullTypeParameterNames = typeParameterNames.Append(None);

                    yield return MethodDeclaration(
                        GenericName(
                            Identifier(OneOf))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SeparatedList<TypeSyntax>(
                                    Join(fullTypeParameterNames.Select(IdentifierName))))),
                        Identifier("Of"))
                        .WithModifiers(
                        TokenList(
                            Token(SyntaxKind.PublicKeyword),
                            Token(SyntaxKind.StaticKeyword)))
                        .WithTypeParameterList(
                        TypeParameterList(
                            SeparatedList<TypeParameterSyntax>(
                                Join(typeParameterNames.Select(TypeParameter)))))
                        .WithExpressionBody(
                        ArrowExpressionClause(
                            DefaultExpression(
                                IdentifierName(None))))
                        .WithSemicolonToken(
                        Token(SyntaxKind.SemicolonToken))
                        .WithLeadingTrivia(
                        Trivia(
                            DocumentationCommentTrivia(
                                SyntaxKind.SingleLineDocumentationCommentTrivia,
                                List(GetDocumentation(i)))));

                    static IEnumerable<XmlNodeSyntax> GetDocumentation(int count)
                    {
                        var typeParameterNames = GetTypeParameterNames(count);
                        var fullTypeParameterNames = typeParameterNames.Append(None);

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
                                        " Creates a new instance of ",
                                        " Creates a new instance of ",
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
                                                    SeparatedList<TypeSyntax>(
                                                        Join(fullTypeParameterNames.Select(IdentifierName))))))))),
                            XmlText()
                            .WithTextTokens(
                                TokenList(
                                    XmlTextLiteral(
                                        TriviaList(),
                                        " set to ",
                                        " set to ",
                                        TriviaList()))),
                            XmlNullKeywordElement()
                            .WithAttributes(
                                SingletonList<XmlAttributeSyntax>(
                                    XmlCrefAttribute(
                                        NameMemberCref(
                                            IdentifierName(None))))),
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
                                    Identifier(Keywords.Summary))));
                        yield return XmlText()
                            .WithTextTokens(
                            TokenList(
                                XmlTextNewLine(
                                    TriviaList(),
                                    NewLine,
                                    NewLine,
                                    TriviaList())));
                        for (var i = 0; i < count; i++)
                        {
                            var example = count == 1
                                ? "The type in the "
                                : $"The {(i + 1).ToOrdinalWords()} type in the ";

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
                                            example,
                                            example,
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
                                                        SeparatedList<TypeSyntax>(
                                                            Join(fullTypeParameterNames.Select(IdentifierName))))))))),
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
                                        Identifier(Keywords.TypeParam)))
                                .WithAttributes(
                                    SingletonList<XmlAttributeSyntax>(
                                        XmlNameAttribute(
                                            XmlName(
                                                Identifier(Keywords.Name)),
                                            Token(SyntaxKind.DoubleQuoteToken),
                                            IdentifierName(GetTypeParameterName(i)),
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
                                        "The created instance ",
                                        "The created instance ",
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
                                                    SeparatedList<TypeSyntax>(
                                                        Join(fullTypeParameterNames.Select(IdentifierName))))))))),
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
                                    TriviaList())));
                    }
                }
            }
        }
    }
}