﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using NUnit.Framework;

using NGettext.Loaders;

namespace NGettext.Tests.Loaders
{
	[TestFixture]
	public class MoFileParserTest
	{
		public string LocalesDir;

		[SetUp]
		public void Init()
		{
			this.LocalesDir = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("TestResources", "locales"));
		}

		[Test]
		public void TestParsing()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test.mo")))))
			{
				var parser = new MoFileParser();
				parser.Parse(stream);
				this._TestLoadedTranslation(parser.Translations);
			}
		}

		[Test]
		public void TestBigEndianParsing()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test_BigEndian.mo")))))
			{
				var parser = new MoFileParser();
				parser.Parse(stream);
				this._TestLoadedTranslation(parser.Translations);
			}
		}

		[Test]
		public void TestAutoEncoding()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test_KOI8-R.mo")))))
			{
				var parser = new MoFileParser();
				parser.Parse(stream);
				this._TestLoadedTranslation(parser.Translations);
			}
		}

		[Test]
		public void TestManualEncoding()
		{
			using (var stream = File.OpenRead(Path.Combine(LocalesDir, Path.Combine("ru_RU", Path.Combine("LC_MESSAGES", "Test_KOI8-R.mo")))))
			{
				var parser = new MoFileParser()
				{
					Encoding = Encoding.GetEncoding("KOI8-R"),
					DetectEncoding = false,
				};
				parser.Parse(stream);
				this._TestLoadedTranslation(parser.Translations);
			}
		}

		private void _TestLoadedTranslation(IDictionary<string, string[]> dict)
		{
			Assert.AreEqual(new[] { "тест" }, dict["test"]);
			Assert.AreEqual(new[] { "тест2" }, dict["test2"]);
			Assert.AreEqual(new[] { "{0} минута", "{0} минуты", "{0} минут" }, dict["{0} minute"]);
			Assert.AreEqual(new[] { "тест3контекст1" }, dict["context1" + BaseCatalog.CONTEXT_GLUE + "test3"]);
			Assert.AreEqual(new[] { "тест3контекст2" }, dict["context2" + BaseCatalog.CONTEXT_GLUE + "test3"]);
		}
	}
}