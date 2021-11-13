using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Mappings
{
    public abstract class MapConfig
    {
        public MapperConfiguration Config => CreateConfiguration();

        protected abstract MapperConfiguration CreateConfiguration();
    }
}
