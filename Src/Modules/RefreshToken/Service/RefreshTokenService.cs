
namespace Src.Modules.RefreshToken.Service;
public class RefreshTokenService
{
    private readonly IRefreshTokenRepository _repo;

    public RefreshTokenService(IRefreshTokenRepository repo)
    {
        _repo = repo;
    }

    public async Task<RefreshTokenModel> Create(long userId)
    {
        var token = Guid.NewGuid().ToString();

        var refresh = new RefreshTokenModel
        {
            Token = token,
            IdUsuario = userId,
            CriadoEm = DateTime.UtcNow,
            ExpiraEm = DateTime.UtcNow.AddDays(7)
        };

        await _repo.CreateAsync(refresh);

        return refresh;
    }

    public async Task<RefreshTokenModel?> Validate(string token)
    {
        var refresh = await _repo.GetByToken(token);

        if (refresh == null)
            return null;

        if (refresh.RevogadoEm != null)
            return null;

        if (refresh.ExpiraEm < DateTime.UtcNow)
            return null;

        return refresh;
    }

    public async Task<RefreshTokenModel> Rotate(RefreshTokenModel oldToken)
    {
        var newToken = await Create(oldToken.IdUsuario);

        oldToken.RevogadoEm = DateTime.UtcNow;
        oldToken.SubstituidoPor = newToken.Token;

        await _repo.UpdateAsync(oldToken);

        return newToken;
    }
}