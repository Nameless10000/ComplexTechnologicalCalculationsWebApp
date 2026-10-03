using Core.Contexts;
using Core.Models.Calculations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Data.Services;

public sealed class CalculationPresetService(AuthDBContext db)
{
    public IQueryable<CalculationPreset> Own(int userId) => db.CalculationPresets.Where(x => x.UserId == userId);
    public async Task<CalculationPreset> Find(int userId, Guid id, CancellationToken token = default) =>
        await Own(userId).FirstOrDefaultAsync(x => x.Id == id, token) ?? throw new KeyNotFoundException();

    public async Task<CalculationPreset> Save(int userId, Guid? id, string module, string name, string? description, string payload, CancellationToken token = default)
    {
        if (module is not ("aglom-mode" or "slag-mode" or "gas-dynamic" or "furnace")) throw new ArgumentException("Неизвестный модуль.");
        name = name.Trim();
        if (name.Length is < 1 or > 200 || (description?.Length ?? 0) > 2000) throw new ArgumentException("Проверьте название (1–200 символов) и описание (до 2000 символов).");
        var normalized = name.ToUpperInvariant();
        if (await Own(userId).AnyAsync(x => x.Module == module && x.NormalizedName == normalized && x.Id != id, token))
            throw new Data.Infrastructure.ConflictException("Шаблон с таким названием уже существует.");
        JObject json;
        try { json = JObject.Parse(payload); } catch { throw new ArgumentException("Входные данные шаблона должны быть JSON-объектом."); }
        if (payload.Length > 100_000) throw new ArgumentException("Шаблон слишком большой.");
        json.Properties().Where(x => x.Name.Equals("User", StringComparison.OrdinalIgnoreCase)).ToList().ForEach(x => x.Remove());
        var preset = id.HasValue ? await Find(userId, id.Value, token) : new CalculationPreset { Id = Guid.NewGuid(), UserId = userId, CreatedAt = DateTime.UtcNow };
        preset.Module = module; preset.Name = name; preset.NormalizedName = normalized; preset.Description = description?.Trim() ?? "";
        preset.Payload = json.ToString(Newtonsoft.Json.Formatting.None); preset.UpdatedAt = DateTime.UtcNow;
        if (!id.HasValue) db.CalculationPresets.Add(preset);
        await db.SaveChangesAsync(token);
        return preset;
    }

    public async Task Delete(int userId, Guid id, CancellationToken token = default)
    { db.CalculationPresets.Remove(await Find(userId, id, token)); await db.SaveChangesAsync(token); }
}
