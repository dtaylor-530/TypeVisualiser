using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeVisualiser.Abstractions;

namespace TypeVisualiser.Factory
{
    public class TypeBuilder : ITypeBuilder
    {
        private IUserPromptMessage userPrompt => container.GetInstance<IUserPromptMessage>();
        private readonly IContainer container;

        public TypeBuilder(IContainer container)
        {

            this.container = container;
        }

        public Type BuildType(string assemblyFile, string fullTypeName)
        {
            Type? type = null;
            try
            {
                type = Type.ReflectionOnlyGetType(fullTypeName, true, false);
            }
            catch (FileNotFoundException)
            {
                // Ignore this exception. This means the type you are attempting to load has not had its assembly loaded into the appdomain.
            }

            if (type != null)
            {
                return type;
            }

            if (string.IsNullOrWhiteSpace(assemblyFile))
            {
                throw new ArgumentNullResourceException("assemblyFile", "Resources.General_Given_Parameter_Cannot_Be_Null");
                //throw new ArgumentNullResourceException("assemblyFile", Resources.General_Given_Parameter_Cannot_Be_Null);
            }

            Assembly assembly = this.LoadAssembly(assemblyFile);
            if (assembly == null)
            {
                throw new FileLoadException("Load Assembly failed.", assemblyFile);
            }

            string shortTypeName = ParseShortTypeName(fullTypeName);
            type = assembly.GetType(shortTypeName);
            if (type == null)
            {
                throw new TypeLoadException("Load Type failed. " + fullTypeName);
            }

            return type;

            static string ParseShortTypeName(string typeName)
            {
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    throw new ArgumentNullException("typeName", "Resources.General_Given_Parameter_Cannot_Be_Null");
                    //throw new ArgumentNullException("typeName", Resources.General_Given_Parameter_Cannot_Be_Null);
                }

                string[] parts = typeName.Split(',');
                return parts[0];
            }
        }


        public Assembly LoadAssembly(string fileName)
        {
            Assembly? subjectLibrary = null;
            try
            {
                subjectLibrary = Assembly.ReflectionOnlyLoadFrom(fileName);
            }
            catch (FileLoadException ex)
            {
                Logger.Instance.WriteEntry("ShellController.GetAssemblyFromFile");
                Logger.Instance.WriteEntry(ex);
                this.userPrompt.Show(ex, "Unable to load assembly: {0} most likely due to missing dependent libraries.", fileName);
            }
            catch (FileNotFoundException ex)
            {
                Logger.Instance.WriteEntry("ShellController.GetAssemblyFromFile");
                Logger.Instance.WriteEntry(ex);
                this.userPrompt.Show(ex, "File not found ({0}). Or one of its dependencies is not found.", fileName);
            }

            return subjectLibrary;
        }


    }
}
