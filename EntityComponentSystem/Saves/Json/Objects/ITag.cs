using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityComponentSystem.Saves.Json.Objects
{
    public interface ITag
    {
        int Resolve(TagMaster json, BinaryWriter writer);
    }
}
