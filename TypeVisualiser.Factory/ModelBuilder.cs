using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using StructureMap;
using StructureMap.Pipeline;
using TypeVisualiser.Abstractions;
using TypeVisualiser.Factory;
using TypeVisualiser.Messaging;
using TypeVisualiser.Models.Abstractions;
//using TypeVisualiser.Startup;

namespace TypeVisualiser.Model
{
    public class ModelBuilder : IModelBuilder
    {
        private static readonly ConcurrentDictionary<Type, IVisualisableType> TypeCache = new ConcurrentDictionary<Type, IVisualisableType>();

        private ITypeBuilder typeBuilder => container.GetInstance<ITypeBuilder>();

        // Do not make readonly - need to set in testing
        // ReSharper disable FieldCanBeMadeReadOnly.Local

        //// ReSharper restore FieldCanBeMadeReadOnly.Local
        private IContainer container;

        public ModelBuilder(IContainer container)
        {
            this.container = container;

        }

       
        /// <summary>
        /// This overload is only used when Navigating to an existing type on a diagram.
        /// </summary>
        /// <param name="type">
        /// The existing type from the diagram
        /// </param>
        /// <param name="depth">
        /// A depth counter to prevent infinitely recursively loading types
        /// </param>
        /// <returns>
        /// The <see cref="IVisualisableTypeWithAssociations"/>.
        /// </returns>
        public IVisualisableTypeWithAssociations BuildSubject(IVisualisableType type, int depth)
        {
            if (type == null)
            {
                throw new ArgumentNullResourceException("type", "Resources.General_Given_Parameter_Cannot_Be_Null");
                //throw new ArgumentNullResourceException("type", Resources.General_Given_Parameter_Cannot_Be_Null);
            }

            Type netType = typeBuilder.BuildType(type.AssemblyFileName, type.AssemblyQualifiedName);
            IVisualisableTypeWithAssociations subject = this.BuildSubject(netType, depth);
            subject.InitialiseForReuseFromCache(depth);
            return subject;
        }

        public IVisualisableTypeWithAssociations BuildSubject(Type type, int depth)
        {
            if (type == null)
            {
                //throw new ArgumentNullResourceException("type", Resources.General_Given_Parameter_Cannot_Be_Null);
                throw new ArgumentNullResourceException("type", "Resources.General_Given_Parameter_Cannot_Be_Null");
            }

            if(TryGettingTypeFromCache(type) is IVisualisableTypeWithAssociations visualisableTypeSubject) 
            {
                return visualisableTypeSubject;
            }

            TypeCache.TryAdd(type, visualisableTypeSubject = BuildType(type, depth));
            visualisableTypeSubject.InitialiseForReuseFromCache(depth);
            return visualisableTypeSubject;

         
        }

        public IVisualisableTypeWithAssociations BuildSubject(string assemblyFile, string fullTypeName, int depth)
        {
            return this.BuildSubject(typeBuilder.BuildType(assemblyFile, fullTypeName), depth);
        }

      

        public IVisualisableType BuildVisualisableType(Type? type, int depth)
        {
            if (type == null)
            {
                throw new Exception(" 4 43222 f");
            }

            IVisualisableType? cachedType = TryGettingTypeFromCache(type);
            if (cachedType != null)
            {
                if (type.AssemblyQualifiedName != cachedType.AssemblyQualifiedName)
                {
                    Logger.Instance.WriteEntry("Cached type retrieved does not matched expected fully qualified name.\n{0}\n{1}", type.AssemblyQualifiedName, cachedType.AssemblyQualifiedName);
                    Debug.Assert(
                        type.AssemblyQualifiedName == cachedType.AssemblyQualifiedName,
                        "Cached type does not match the type required - matching by GUID may not be working as intended. " + type.AssemblyQualifiedName + " != " + cachedType.AssemblyQualifiedName);
                }

                return cachedType;
            }

            TypeCache.TryAdd(type, cachedType = BuildType(type, depth));
            return cachedType;

    
        }


        private IVisualisableTypeWithAssociations BuildType(Type type, int depth)
        {
            var args = new Dictionary<string, object> { { "type", type }, { "depth", depth }, { "factory", this.container } };
            return this.container.GetInstance<IVisualisableTypeWithAssociations>(new ExplicitArguments(args));
        }

        private static IVisualisableType? TryGettingTypeFromCache(Type type)
        {
            if (TypeCache.TryGetValue(type, out var cachedType))
            {
                var subject = cachedType as IVisualisableTypeWithAssociations;
                if (subject != null)
                {
                    return subject;
                }
            }

            return null;
        }
    }
}