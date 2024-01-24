namespace Domain.Common.Interfaces;

public interface IIdHas<TKey>
{
    TKey Id { get; set; }
}
