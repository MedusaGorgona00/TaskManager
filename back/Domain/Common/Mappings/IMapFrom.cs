using AutoMapper;

namespace Domain.Common.Mappings
{
    /// <summary>
    /// IMapFrom base
    /// </summary>
    public interface IMapFrom
    {

    }

    /// <summary>
    /// IMapFrom with generic type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapFrom<T> : IMapFrom
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
