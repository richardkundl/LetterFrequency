using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Web;

namespace LetterFrequency.Web.Helper
{
	public class Language
	{
		public static readonly List<string> EnglishCharacters = new List<string> { "q", "w", "e", "r", "t", "z", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "y", "x", "c", "v", "b", "n", "m" };

		public string Name { get; set; }

		public Dictionary<string, double> CharacterSet { get; set; }

		public Language(string name, Dictionary<string, double> characterSet)
		{
			Name = name;
			CharacterSet = characterSet;
			SpecificCharacters = characterSet.Keys.Except(EnglishCharacters).ToList();
		}

		public List<string> SpecificCharacters { get; set; }
	}

	public static class Letter
	{
		private static List<Language> LanguageSet { get; set; }

		static Letter()
		{
			LanguageSet = new List<Language>();

			AddHungarian();
			AddGerman();
			AddEnglish();
		}

		private static void AddEnglish()
		{
			var dictionary = new Dictionary<string, double>();
			dictionary.Add("e", 12.702);
			dictionary.Add("t", 9.056);
			dictionary.Add("a", 8.167);
			dictionary.Add("o", 7.507);
			dictionary.Add("i", 6.966);
			dictionary.Add("n", 6.749);
			dictionary.Add("s", 6.327);
			dictionary.Add("h", 6.094);
			dictionary.Add("r", 5.987);
			dictionary.Add("d", 4.253);
			dictionary.Add("l", 4.025);
			dictionary.Add("c", 2.782);
			dictionary.Add("u", 2.758);
			dictionary.Add("m", 2.406);
			dictionary.Add("w", 2.361);
			dictionary.Add("f", 2.228);
			dictionary.Add("g", 2.015);
			dictionary.Add("y", 1.974);
			dictionary.Add("p", 1.929);
			dictionary.Add("b", 1.492);
			dictionary.Add("v", 0.978);
			dictionary.Add("k", 0.772);
			dictionary.Add("j", 0.153);
			dictionary.Add("x", 0.150);
			dictionary.Add("q", 0.095);
			dictionary.Add("z", 0.074);
			LanguageSet.Add(new Language("ENGLISH", dictionary.OrderByDescending(d => d.Value).ToDictionary(x => x.Key, x => x.Value)));
		}

		private static void AddGerman()
		{
			var dictionary = new Dictionary<string, double>();
			dictionary.Add("e", 14.9958);
			dictionary.Add("n", 10.262);
			dictionary.Add("i", 8.26712);
			dictionary.Add("s", 8.14877);
			dictionary.Add("r", 7.04987);
			dictionary.Add("a", 6.44125);
			dictionary.Add("t", 4.86898);
			dictionary.Add("h", 4.68301);
			dictionary.Add("d", 4.6661);
			dictionary.Add("u", 3.65173);
			dictionary.Add("g", 3.60101);
			dictionary.Add("l", 3.39814);
			dictionary.Add("o", 2.55283);
			dictionary.Add("b", 2.55283);
			dictionary.Add("f", 1.9104);
			dictionary.Add("v", 1.6399);
			dictionary.Add("m", 1.62299);
			dictionary.Add("k", 1.62299);
			dictionary.Add("w", 1.55537);
			dictionary.Add("z", 0.811496);
			dictionary.Add("ü", 0.79459);
			dictionary.Add("p", 0.642434);
			dictionary.Add("ä", 0.507185);
			dictionary.Add("ö", 0.304311);
			dictionary.Add("j", 0.270499);
			dictionary.Add("ß", 0.0676247);
			dictionary.Add("q", 0.0169062);
			LanguageSet.Add(new Language("GERMAN", dictionary.OrderByDescending(d => d.Value).ToDictionary(x => x.Key, x => x.Value)));
		}

		private static void AddHungarian()
		{
			var dictionary = new Dictionary<string, double>();
			dictionary.Add("e", 12.255717255717256);
			dictionary.Add("a", 9.42827442827443);
			dictionary.Add("t", 7.380457380457381);
			dictionary.Add("n", 6.4449064449064455);
			dictionary.Add("l", 6.382536382536383);
			dictionary.Add("s", 5.3222453222453225);
			dictionary.Add("k", 4.521829521829522);
			dictionary.Add("é", 4.511434511434512);
			dictionary.Add("i", 4.1995841995842);
			dictionary.Add("m", 4.054054054054054);
			dictionary.Add("o", 3.866943866943867);
			dictionary.Add("á", 3.6486486486486487);
			dictionary.Add("g", 2.837837837837838);
			dictionary.Add("r", 2.806652806652807);
			dictionary.Add("z", 2.733887733887734);
			dictionary.Add("v", 2.4532224532224536);
			dictionary.Add("b", 2.0582120582120584);
			dictionary.Add("d", 2.0374220374220373);
			dictionary.Add("sz", 1.808731808731809);
			dictionary.Add("j", 1.5696465696465698);
			dictionary.Add("h", 1.340956340956341);
			dictionary.Add("gy", 1.185031185031185);
			dictionary.Add("ő", 0.8835758835758836);
			dictionary.Add("ö", 0.8212058212058213);
			dictionary.Add("ny", 0.7900207900207901);
			dictionary.Add("ly", 0.7380457380457381);
			dictionary.Add("ü", 0.654885654885655);
			dictionary.Add("ó", 0.6340956340956341);
			dictionary.Add("f", 0.5821205821205821);
			dictionary.Add("p", 0.5093555093555093);
			dictionary.Add("í", 0.498960498960499);
			dictionary.Add("u", 0.4158004158004158);
			dictionary.Add("cs", 0.2598752598752599);
			dictionary.Add("ű", 0.12474012474012475);
			dictionary.Add("c", 0.11434511434511435);
			dictionary.Add("ú", 0.10395010395010395);
			dictionary.Add("zs", 0.02079002079002079);
			LanguageSet.Add(new Language("HUNGARIAN", dictionary.OrderByDescending(d => d.Value).ToDictionary(x => x.Key, x => x.Value)));
		}

		public static string Process(string text)
		{
			// how to work multi character? eg: dzs in hungarian
			text = text.ToLower();
			text = Regex.Replace(text, @"\s+", string.Empty);
			text = Regex.Replace(text, @"[\d-]", string.Empty);

			var acceptedLetters = new List<string>();
			acceptedLetters.AddRange(Language.EnglishCharacters);
			foreach (var language in LanguageSet)
			{
				foreach (var spec in language.SpecificCharacters)
				{
					acceptedLetters.Add(spec);
				}
			}

			var letters = new Dictionary<string, int>();
			var percent = new Dictionary<string, double>();

			var processedChars = 0;

			foreach (var current in text)
			{
				if (!acceptedLetters.Contains(current.ToString()))
				{
					continue;
				}

				if (letters.ContainsKey(current.ToString()))
				{
					letters[current.ToString()] = letters[current.ToString()] + 1;
				}
				else
				{
					letters.Add(current.ToString(), 1);
				}

				processedChars = processedChars + 1;
			}

			foreach (var letter in letters)
			{
				percent.Add(letter.Key, ((double)letter.Value / processedChars));
			}

			percent = percent.OrderByDescending(d => d.Value).ToDictionary(x => x.Key, x => x.Value);

			// find similar lang

			return string.Empty;
		}
	}
}