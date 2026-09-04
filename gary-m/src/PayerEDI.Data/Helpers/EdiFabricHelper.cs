using EdiFabric;

namespace PayerEDI.Data.Helpers;

public static class EdiFabricHelper
{
    private static string BuildTokenCacheTemp() =>
        Path.Combine(Path.GetTempPath(), "Edi_Fabric_TokenCache.txt");

    private static string ConfigureEdiFabricCore(string ediFabricKey, string? tokenCacheFileIn)
    {
        var tokenCacheFile = string.IsNullOrEmpty(tokenCacheFileIn)
            ? BuildTokenCacheTemp()
            : tokenCacheFileIn;

        var tokenLoadedVia = "Token not loaded";

        if (File.Exists(tokenCacheFile))
        {
            try
            {
                var token = File.ReadAllText(tokenCacheFile);
                SerialKey.SetToken(token);
                tokenLoadedVia = "Token loaded from cache";
            }
            catch (Exception)
            {
                File.Delete(tokenCacheFile);
                SerialKey.Set(ediFabricKey);
                File.WriteAllText(tokenCacheFile, SerialKey.Token);
                tokenLoadedVia = "Invalid token cache discarded; token loaded from key";
            }
        }

        if (SerialKey.Token is null)
        {
            SerialKey.Set(ediFabricKey);
            File.WriteAllText(tokenCacheFile, SerialKey.Token);
            tokenLoadedVia = "Token loaded from key";
        }

        if (SerialKey.DaysToExpiration < 3)
        {
            SerialKey.Set(ediFabricKey);
            File.WriteAllText(tokenCacheFile, SerialKey.Token);
            tokenLoadedVia = "Token refreshed from key";
        }

        return tokenLoadedVia;
    }

    public static string ConfigureEdiFabric(string ediFabricKey) =>
        ConfigureEdiFabricCore(ediFabricKey, null);

    public static string ConfigureEdiFabric(string ediFabricKey, string? tokenCacheFileIn) =>
        ConfigureEdiFabricCore(ediFabricKey, tokenCacheFileIn);
}
