using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.QueueProcessor
{
    public abstract class VersionedProcessorBase
    {
        protected string ItemType;
        protected string ItemTypeAssembly;
        protected string ItemTypeMajorVersion;

        protected VersionedProcessorBase(string assemblyQualifiedName)
        {
            var parts = GetParts(assemblyQualifiedName);
            ItemType = parts[0];
            ItemTypeAssembly = parts[1];
            ItemTypeMajorVersion = parts[2];
        }


        protected string[] GetParts(string assemblyQualifiedName)
        {
            var parts = assemblyQualifiedName.Split(',');
            return new[]{
                parts[0],
                parts[1],
                parts[2].Split('.')[0].Replace("Version=", "")
            };
        }

        public bool CanProcessItem(string itemType)
        {
            //rather than check the entire assembly qualified name we only want to check the type name, assembly name and the Major version of the assembly
            //we want to handle updates to the assembly and in theory as long as the major version doesnt change we should be able to parse it
            var parts = GetParts(itemType);
            return parts[0] == ItemType
                    && parts[1] == ItemTypeAssembly
                    && parts[2] == ItemTypeMajorVersion;
        }

    }
}
