using System;
using System.Collections.Generic;

namespace ShapezMono.Game.old
{
    /// <summary>
    /// 言語情報
    /// </summary>
    public class Language
    {
        public string Name { get; }
        public string Code { get; }
        public string Region { get; }

        public Language(string name, string code, string region = "")
        {
            Name = name;
            Code = code;
            Region = region;
        }
    }

    public static class Languages
    {
        public static readonly Dictionary<string, Language> All = new()
        {
            { "en", new Language("English", "en") },
            { "ja", new Language("日本語", "ja") },
        };

        public static Language GetLanguageByCode(string code)
        {
            if (All.TryGetValue(code, out var lang))
                return lang;
            throw new ArgumentException($"未知の言語コード: {code}");
        }
    }
}
