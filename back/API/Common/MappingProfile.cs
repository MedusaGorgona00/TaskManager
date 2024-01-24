using AutoMapper;
using Domain.Common.Mappings;

namespace API.Common
{
    public class MappingProfile : Profile, IMappingProfile
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public MappingProfile()
        {
            var self = this as IMappingProfile;
            self.ApplyMappingsFromAssembly(typeof(MappingProfile).Assembly);
        }
    }
}
