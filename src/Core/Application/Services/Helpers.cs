using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Application.Repositories.Interfaces;

namespace Application.Services
{
    public class Helpers : IHelpers
    {
        private static readonly HashSet<string> _stopWords = new HashSet<string>
        {
            "ve", "ile", "ya", "da", "de", "ama", "fakat", "bir", "şu", "bu", "için",
            "gibi", "çok", "az", "daha", "en", "olan", "olanlar", "ki", "ise", "veya"
        };

        public string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim().ToLowerInvariant();

            var turkishChars = new Dictionary<char, string>
            {
                ['ç'] = "c",
                ['ğ'] = "g",
                ['ı'] = "i",
                ['ö'] = "o",
                ['ş'] = "s",
                ['ü'] = "u",
                ['â'] = "a",
                ['î'] = "i",
                ['û'] = "u"
            };

            var builder = new StringBuilder();
            foreach (var ch in text)
            {
                if (turkishChars.TryGetValue(ch, out var replacement))
                {
                    builder.Append(replacement);
                }
                else if (char.IsLetterOrDigit(ch) || ch == ' ' || ch == '-')
                {
                    builder.Append(ch);
                }
                // Diğer tüm karakterleri (ör. soru işareti, nokta vb.) direkt atlıyoruz
            }

            text = builder.ToString();

            // Stop word'leri filtrele
            var words = text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                            .Where(word => !_stopWords.Contains(word))
                            .ToList();

            if (!words.Any())
                return string.Empty;

            text = string.Join("-", words);

            // Ardışık tireleri temizle
            text = Regex.Replace(text, @"-+", "-").Trim('-');

            // Maksimum uzunluk kontrolü
            return text.Length > 100 ? text.Substring(0, 100) : text;
        }
      


    }
}
