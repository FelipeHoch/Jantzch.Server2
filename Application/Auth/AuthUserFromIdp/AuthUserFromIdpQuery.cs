using MediatR;

namespace Jantzch.Server2.Application.Auth.AuthUserFromIdp;

public record AuthUserFromIdpQuery(string DataInBase64): IRequest<string>;