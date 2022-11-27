namespace Synergy.API;
public sealed record Response<TData>
(
    int StatusCode,
    string StatusDescription,
    TData? Data
);