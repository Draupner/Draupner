using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.PrettyPrinter;
using ICSharpCode.NRefactory.Visitors;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Entities
{
    public class EntityReader : IEntityReader
    {
        private readonly IFileSystem fileSystem;

        public EntityReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public Entity ReadEntity(string sourcePath)
        {
            if (!fileSystem.FileExists(sourcePath))
            {
                Console.WriteLine("Entity file does not exist");
                return null;
            }

            AstDetails astDetails = CreateAst(sourcePath);

            var entity = new Entity();

            foreach (var type in astDetails.Types)
            {
                entity.Name = type.OriginalObject.Name;
            }
            foreach (var property in astDetails.Properties)
            {
                var entityProperty = new EntityProperty
                                         {
                                             Name = property.Text,
                                             Type = property.OriginalObject.TypeReference.Type
                                         };
                entity.Properties.Add(entityProperty);
            }

            return entity;
        }

        private AstDetails CreateAst(string sourceCode)
        {
            sourceCode = (fileSystem.FileExists(sourceCode)) ? fileSystem.FileReadText(sourceCode) : sourceCode;
            IParser parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, new StringReader(sourceCode));
            parser.Parse();
            if(parser.Errors.Count > 0)
            {
                throw new EntityParsingException(parser.Errors.ErrorOutput);
            }

            CompilationUnit compilationUnit = parser.CompilationUnit;

            var astDetails = new AstDetails();
            var extraSpecials = new List<ISpecial>();
            var specials = parser.Lexer.SpecialTracker.RetrieveSpecials();
            specials.AddRange(extraSpecials);
            astDetails.MapSpecials(specials);

            compilationUnit.AcceptVisitor(astDetails, null);
            return astDetails;
        }

    }
    public class AstDetails : AbstractAstVisitor
    {
        public List<AstValue<UsingDeclaration>> UsingDeclarations = new List<AstValue<UsingDeclaration>>();
        public List<AstValue<TypeDeclaration>> Types = new List<AstValue<TypeDeclaration>>();
        public List<AstValue<MethodDeclaration>> Methods = new List<AstValue<MethodDeclaration>>();
        public List<AstValue<FieldDeclaration>> Fields = new List<AstValue<FieldDeclaration>>();
        public List<AstValue<PropertyDeclaration>> Properties = new List<AstValue<PropertyDeclaration>>();
        public List<AstValue<ISpecial>> Comments = new List<AstValue<ISpecial>>();
        public string CSharpCode { get; set; }

        public override object VisitUsingDeclaration(UsingDeclaration usingDeclaration, object data)
        {
            foreach (var declaration in usingDeclaration.Usings)
                UsingDeclarations.Add(new AstValue<UsingDeclaration>(declaration.Name, usingDeclaration, usingDeclaration.StartLocation, usingDeclaration.EndLocation));
            return null;
        }


        public override object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
        {
            base.VisitTypeDeclaration(typeDeclaration, data); // visit types                  
            Types.Add(new AstValue<TypeDeclaration>(typeDeclaration.Name, typeDeclaration, typeDeclaration.StartLocation, typeDeclaration.EndLocation));
            return null;
        }

        public override object VisitMethodDeclaration(MethodDeclaration methodDeclaration, object data)
        {
            base.VisitMethodDeclaration(methodDeclaration, data); // visit methods
            Methods.Add(new AstValue<MethodDeclaration>(methodDeclaration.Name, methodDeclaration, methodDeclaration.StartLocation, methodDeclaration.EndLocation));
            return null;
        }

        public override object VisitFieldDeclaration(FieldDeclaration fieldDeclaration, object data)
        {
            base.VisitFieldDeclaration(fieldDeclaration, data); // visit fields
            foreach (var field in fieldDeclaration.Fields)
                Fields.Add(new AstValue<FieldDeclaration>(field.Name, fieldDeclaration, fieldDeclaration.StartLocation, fieldDeclaration.EndLocation));
            return null;
        }
        public override object VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, object data)
        {
            base.VisitPropertyDeclaration(propertyDeclaration, data); // visit properties
            Properties.Add(new AstValue<PropertyDeclaration>(propertyDeclaration.Name, propertyDeclaration, propertyDeclaration.BodyStart, propertyDeclaration.BodyEnd));
            return null;
        }

        public void MapSpecials(IList<ISpecial> specials)
        {
            foreach (var special in specials)
            {
                if (special is Comment)
                {
                    Comments.Add(new AstValue<ISpecial>(
                                     ((Comment)special).CommentText,
                                     special,
                                     special.StartPosition,
                                     special.EndPosition));
                }
            }
        }

        public void RewriteCodeCSharp(CompilationUnit unit, IList<ISpecial> specials)
        {
            var outputVisitor = new CSharpOutputVisitor();
            using (SpecialNodesInserter.Install(specials, outputVisitor))
            {
                unit.AcceptVisitor(outputVisitor, null);
            }
            CSharpCode = outputVisitor.Text;
        }

        public void RewriteCodeVbNet(CompilationUnit unit, IList<ISpecial> specials)
        {
            var outputVisitor = new VBNetOutputVisitor();
            using (SpecialNodesInserter.Install(specials, outputVisitor))
            {
                unit.AcceptVisitor(outputVisitor, null);
            }
        }
    }

    public class AstValue<T>
    {
        public string Text { get; set; }
        public T OriginalObject { get; set; }
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }

        public AstValue(string text, T originalObject, Location startLocation, Location endLocation)
        {
            Text = text;
            OriginalObject = originalObject;
            StartLocation = startLocation;
            EndLocation = endLocation;
        }
    }
}
