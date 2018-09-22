using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WooferGame.Meta.LevelEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    class InspectorAttribute : Attribute
    {
        public InspectorEditType Flags;

        public InspectorAttribute(InspectorEditType flags) => Flags = flags;
    }

    enum InspectorEditType
    {
        Default,
        Position,
        Id
    }
}
