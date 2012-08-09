using System;
using System.IO;
using System.Reflection;
using RazorEngine;
using RazorEngine.Templating;
using Scaffold.Exceptions;
using Scaffold.Io;

namespace Scaffold.Template
{
    public class TemplateEngine : ITemplateEngine
    {
        private readonly IFileSystem fileSystem;

        public TemplateEngine(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            // Ensure Binder is loaded, else RazorEngine fails
            bool loaded = typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly != null;
        }

        public void GenerateFromTemplate<T>(string templateName, string outputFile, T templateModel)
        {
            var template = ReadTemplate(templateName);
            WriteTemplate(template, templateModel, outputFile);
        }

        public void GenerateFromTemplate<T>(TextReader templateStream, string outputFile, T templateModel)
        {
            var template = templateStream.ReadToEnd();
            WriteTemplate(template, templateModel, outputFile);
        }

        private void WriteTemplate<T>(string template, T templateModel, string outputFile)
        {
            try
            {
                string result = Razor.Parse(template, templateModel);
                Console.WriteLine("Writing: " + Directory.GetCurrentDirectory() + "\\" + outputFile);
                fileSystem.FileWriteText(outputFile, result);
            }
            catch (TemplateCompilationException exception)
            {
                Console.WriteLine("Error compiling template (" + outputFile + ") : ");
                foreach (var compilerError in exception.Errors)
                {
                    Console.WriteLine(compilerError);
                }
            }
        }

        public string ReadTemplate(string templateName)
        {
            Assembly assembly = GetType().Assembly;
            var templateStream = assembly.GetManifestResourceStream(templateName);
            if(templateStream == null)
            {
                throw new TemplateNotFoundException("Template not found: " + templateName);
            }
            using (var reader = new StreamReader(templateStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
