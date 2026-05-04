using Src.Shared.Interfaces;

public interface IRefreshTokenRepository : IBaseRepository<RefreshTokenModel>
{
    Task<RefreshTokenModel?> GetByToken(string token);
}