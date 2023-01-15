using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeVisualiser.Factory
{
    public class AssemblyResolver
    {
        public static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Logger.Instance.WriteEntry("OnAssemblyResolve");
            Logger.Instance.WriteEntry("   Resolving " + args.Name);

            string folderPath = Path.GetDirectoryName(args.RequestingAssembly.Location);
            if (string.IsNullOrEmpty(folderPath))
            {
                throw new ArgumentException("Resources.ModelBuilder_OnAssemblyResolve_ResolveEventArgs_RequestingAssembly_Location_is_null_or_empty", "args");
                //throw new ArgumentException(Resources.ModelBuilder_OnAssemblyResolve_ResolveEventArgs_RequestingAssembly_Location_is_null_or_empty, "args");
            }

            string assemblyNameObject = new AssemblyName(args.Name).Name + ".dll";
            string assemblyPath = Path.Combine(folderPath, assemblyNameObject);

            // Check in the same path as the user selected assembly.
            if (File.Exists(assemblyPath))
            {
                return Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }

            // Check if there is a subfolder called "Dependencies" - this is used in testing to better organise the number of dependencies.
            assemblyPath = Path.Combine(Path.Combine(folderPath, "Dependencies"), assemblyNameObject);
            if (File.Exists(assemblyPath))
            {
                return Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            }

            // Try dot net default.
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

    }
}
