using System.Linq;
using System.Xml;
using Scaffold.Io;

namespace Scaffold.VisualStudio
{
    public class ProjectFileManager : IProjectFileManager
    {
        private readonly IFileSystem fileSystem;

        public ProjectFileManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void AddContentFileToProject(string file, string projectName)
        {
            string projectFilePath = GetProjectFilePath(projectName);
            XmlDocument xml = LoadXml(projectFilePath);

            var anyChanges = AddContentFileToXml(xml, file);

            if (anyChanges)
            {
                SaveXml(xml, projectFilePath);
            }
        }

        public void AddCompileFileToProject(string file, string projectName)
        {
            string projectFilePath = GetProjectFilePath(projectName);
            XmlDocument xml = LoadXml(projectFilePath);

            var anyChanges = AddCompileFileToXml(xml, file);

            if (anyChanges)
            {
                SaveXml(xml, projectFilePath);
            }
        }

        private string GetProjectFilePath(string projectName)
        {
            return projectName + "\\" + projectName + ".csproj";
        }


        private XmlDocument LoadXml(string projectFilePath)
        {
            string xml = ReadXml(projectFilePath);
            var originalXml = new XmlDocument { PreserveWhitespace = true };
            originalXml.LoadXml(xml);
            return originalXml;
        }

        private void SaveXml(XmlDocument originalXml, string projectFilePath)
        {
            fileSystem.FileWriteText(projectFilePath, originalXml.OuterXml);
        }

        private string ReadXml(string projectFilePath)
        {
            return fileSystem.FileReadText(projectFilePath);
        }

        private bool AddContentFileToXml(XmlDocument xml, string file)
        {
            var root = FindRoot(xml);
            var itemGroup = FindContentItemGroup(root);

            if (!DoesContentIncludeExists(root, file))
            {
                XmlNode newCompileChild = xml.CreateNode(XmlNodeType.Element, "Content", "http://schemas.microsoft.com/developer/msbuild/2003");
                XmlAttribute includeAttribute = xml.CreateAttribute("Include");
                includeAttribute.Value = file;

                newCompileChild.Attributes.Append(includeAttribute);

                itemGroup.AppendChild(newCompileChild);

                return true;
            }
            return false;
        }

        private bool AddCompileFileToXml(XmlDocument xml, string file)
        {
            var root = FindRoot(xml);
            var itemGroup = FindCompileItemGroup(root);

            if(!DoesCompileIncludeExists(itemGroup, file))
            {
                XmlNode newCompileChild = xml.CreateNode(XmlNodeType.Element, "Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                XmlAttribute includeAttribute = xml.CreateAttribute("Include");
                includeAttribute.Value = file;

                newCompileChild.Attributes.Append(includeAttribute);

                itemGroup.AppendChild(newCompileChild);

                return true;
            }
            return false;
        }

        private bool DoesCompileIncludeExists(XmlNode itemGroup, string file)
        {
            foreach (XmlNode childNode in itemGroup.ChildNodes)
            {
                if(childNode.Name == "Compile")
                foreach (XmlAttribute xmlAttribute in childNode.Attributes)
                {
                    if(xmlAttribute.Name == "Include" && xmlAttribute.Value == file)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DoesContentIncludeExists(XmlNode root, string file)
        {
            foreach (XmlNode childNode in root.ChildNodes)
            {
                if (childNode.Name == "ItemGroup")
                {
                    foreach (XmlNode childItemGroup in childNode.ChildNodes)
                    {
                        if (childItemGroup.Name == "Content")
                            foreach (XmlAttribute xmlAttribute in childItemGroup.Attributes)
                            {
                                if (xmlAttribute.Name == "Include" && xmlAttribute.Value == file)
                                {
                                    return true;
                                }
                            }
                    }

                }
            }
            return false;
        }

        private XmlNode FindContentItemGroup(XmlNode root)
        {
            foreach (XmlNode childNode in root.ChildNodes)
            {
                if (childNode.Name == "ItemGroup")
                {
                    if (childNode.ChildNodes.Cast<XmlNode>().Any(node => node.Name == "Content"))
                    {
                        return childNode;
                    }
                }
            }
            return null;
        }


        private XmlNode FindCompileItemGroup(XmlNode root)
        {
            foreach (XmlNode childNode in root.ChildNodes)
            {
                if(childNode.Name == "ItemGroup")
                {
                    if (childNode.ChildNodes.Cast<XmlNode>().Any(node => node.Name == "Compile"))
                    {
                        return childNode;
                    }
                }
            }
            return null;
        }

        private XmlNode FindRoot(XmlDocument originalXml)
        {
            foreach (XmlNode childNode in originalXml.ChildNodes)
            {
                if(childNode.Name == "Project")
                {
                    return childNode;
                }
            }
            return null;
        }
    }
}