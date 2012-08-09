using System;
using System.Collections.Generic;
using Scaffold.Configuration;
using Scaffold.Io;

namespace Scaffold.Generator.Helpers
{
    public class AutoMapperHelper : IAutoMapperHelper
    {
        private readonly IConfiguration configuration;
        private readonly FileMerger fileMerger;

        public AutoMapperHelper(IFileSystem fileSystem, IConfiguration configuration)
        {
            this.configuration = configuration;
            fileMerger = new FileMerger(fileSystem);
        }

        public void AddAutoMapperConfiguration(string entityName, string viewModelName)
        {
            Console.WriteLine("Adding automapper configuration");
            string filePath = AutoMapperConfigurationFile();

            var mapEntityToViewModelSourceCode = "            Mapper.CreateMap<" + entityName + ", " + viewModelName + ">();";
            var mapViewModelToEntitySourceCode = "            Mapper.CreateMap<" + viewModelName + ", " + entityName + ">();";
            var mapPaginationSourceCode = "            Mapper.CreateMap<Page<" + entityName + ">, IPagination<" + viewModelName +
                                          ">>().ConvertUsing(x => new CustomPagination<" + viewModelName +
                                          ">(Mapper.Map<IEnumerable<" + entityName + ">, IEnumerable<" + viewModelName +
                                          ">>(x.Items), x.PageNumber, x.PageSize, x.TotalItemCount));";

            fileMerger.InsertLineIntoMethod(filePath, mapEntityToViewModelSourceCode, "Configure", mapEntityToViewModelSourceCode);
            fileMerger.InsertLineIntoMethod(filePath, mapViewModelToEntitySourceCode, "Configure", mapViewModelToEntitySourceCode);
            fileMerger.InsertLineIntoMethod(filePath, mapPaginationSourceCode, "Configure", mapPaginationSourceCode);

            AddNamespace(filePath, new[] { configuration.CoreNameSpace + ".Domain.Model", configuration.WebNameSpace + ".Models" });
        }

        private void AddNamespace(string filePath, IEnumerable<string> namespaces)
        {
            foreach (var ns in namespaces)
            {
                fileMerger.InsertFirstLine(filePath, "using " + ns + ";", "using " + ns + ";");
            }
        }

        private string AutoMapperConfigurationFile()
        {
            return configuration.WebNameSpace + "\\Common\\AutoMapper\\AutoMapperConfiguration.cs";
        }
    }
}