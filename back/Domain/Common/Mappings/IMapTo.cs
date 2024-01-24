using AutoMapper;

namespace Domain.Common.Mappings
{
    public interface IMapTo
    {

    }

    /// <summary>
    /// IMapTo with generic type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapTo<T> : IMapTo
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T), MemberList.Source);
    }
}
