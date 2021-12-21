using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Caravela.Open.DependencyEmbedder.Weaver
{
    public class ResourceNameFinder
    {
        private readonly AssemblyLoaderInfo _info;
        private readonly IEnumerable<string> _resourceNames;

        public ResourceNameFinder(AssemblyLoaderInfo info, IEnumerable<string> resourceNames)
        {
            _info = info;
            _resourceNames = resourceNames;
        }

        public void FillInStaticConstructor(bool createTemporaryAssemblies, ImmutableArray<string> preloadOrder,
            string resourcesHash,
            Checksums checksums)
        {
            var statements = new List<StatementSyntax>();

            var orderedResources = preloadOrder
                .Join(_resourceNames, p => p.ToLowerInvariant(),
                    r =>
                    {
                        var parts = r.Split('.');
                        GetNameAndExt(parts, out var name, out _);
                        return name;
                    }, (s, r) => r)
                .Union(_resourceNames.OrderBy(r => r));

            foreach (var resource in orderedResources)
            {
                var parts = resource.Split('.');

                GetNameAndExt(parts, out var name, out var ext);

                if (string.Equals(parts[0], "DependencyEmbedder", StringComparison.OrdinalIgnoreCase))
                {
                    if (createTemporaryAssemblies)
                    {
                        AddToList(statements, _info.PreloadListField, resource);
                    }
                    else
                    {
                        if (string.Equals(ext, "pdb", StringComparison.OrdinalIgnoreCase))
                            AddToDictionary(statements, _info.SymbolNamesField, name, resource);
                        else
                            AddToDictionary(statements, _info.AssemblyNamesField, name, resource);
                    }
                }
                else if (string.Equals(parts[0], "DependencyEmbedder32", StringComparison.OrdinalIgnoreCase))
                {
                    AddToList(statements, _info.Preload32ListField, resource);
                }
                else if (string.Equals(parts[0], "DependencyEmbedder64", StringComparison.OrdinalIgnoreCase))
                {
                    AddToList(statements, _info.Preload64ListField, resource);
                }
            }

            if (_info.ChecksumsField != null)
                foreach (var checksum in checksums.AllChecksums)
                    AddToDictionary(statements, _info.ChecksumsField, checksum.Key, checksum.Value);

            if (_info.Md5HashField != null)
                statements.Add(ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(_info.Md5HashField),
                    LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(resourcesHash)))));

            var staticConstructor = ConstructorDeclaration(_info.SourceTypeName)
                .AddModifiers(Token(SyntaxKind.StaticKeyword)).WithBody(Block(statements));
            _info.SourceType = _info.SourceType.InsertNodesAfter(
                _info.SourceType.DescendantNodes().OfType<ClassDeclarationSyntax>().Single().Members.Last(),
                new[] { staticConstructor });
        }

        private static void GetNameAndExt(string[] parts, out string name, out string ext)
        {
            var isCompressed = string.Equals(parts[parts.Length - 1], "compressed", StringComparison.OrdinalIgnoreCase);

            ext = parts[parts.Length - (isCompressed ? 2 : 1)];

            name = string.Join(".", parts.Skip(1).Take(parts.Length - (isCompressed ? 3 : 2)));
        }

        private void AddToDictionary(List<StatementSyntax> statements, string field, string key, string name)
        {
            statements.Add(ExpressionStatement(
                InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(field), IdentifierName("Add")))
                    .AddArgumentListArguments(
                        Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(key))),
                        Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(name))))));
        }

        private void AddToList(List<StatementSyntax> statements, string field, string name)
        {
            statements.Add(ExpressionStatement(
                InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(field), IdentifierName("Add")))
                    .AddArgumentListArguments(
                        Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(name))))));
        }
    }
}