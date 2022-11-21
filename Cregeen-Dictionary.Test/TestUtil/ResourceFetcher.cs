using System;
using System.IO;

namespace Cregeen_Dictionary.Test.TestUtil;

public static class ResourceFetcher
{
    public static string GetResource(string file)
    {
        using var stream = typeof(Tests).Assembly.GetManifestResourceStream(file);

        if (stream == null)
        {
            var names = typeof(Tests).Assembly.GetManifestResourceNames();
            throw new Exception($"Couldn't find {file}. Potential names: {string.Join(", ", names)}");
        }

        using TextReader tr = new StreamReader(stream);
        return tr.ReadToEnd();

    }
}