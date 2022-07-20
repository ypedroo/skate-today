using LanguageExt.Common;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Services.Interfaces;

public interface IGoogleMapService
{
    Task<Result<SkateParks>> GetSkateParks(string spot);
}