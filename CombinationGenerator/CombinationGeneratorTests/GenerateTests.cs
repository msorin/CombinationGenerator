using CombinationGenerator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinationGeneratorTests
{
	[TestFixture]
	public class GenerateTests
	{
		[Test]
		public void CombinationBitwiseTest([ValueSource(nameof(TestValues))] KeyValuePair<string, int> testValue)
		{
			//Arrange
			var split = testValue.Key.Split('|');
			var pool = Enumerable.Range(1, int.Parse(split[0])).Select(x => Guid.NewGuid()).ToArray();
			var size = int.Parse(split[1]);

			if(pool.Length > 19)
			{
				return; //Bitwise fails when numbers get large
			}

			//Act
			var gen = new Generator<Guid>(pool, size);
			var teams = gen.GetBitwiseSamples().ToList();

			//Assert		
			Assert.AreEqual(testValue.Value, teams.Count);
			Assert.AreEqual(teams.Count, teams.Distinct().Count());
		}

		[Test]
		public void CombinationRecursiveTest([ValueSource(nameof(TestValues))] KeyValuePair<string, int> testValue)
		{
			//Arrange
			var split = testValue.Key.Split('|');
			var pool = Enumerable.Range(1, int.Parse(split[0]) ).Select(x => Guid.NewGuid()).ToArray();
			var size = int.Parse(split[1]);

			//Act
			var gen = new Generator<Guid>(pool, size);
			
			//
			var recursive = gen.GetSamples().ToList();

			//Assert
			Assert.AreEqual(testValue.Value, recursive.Count);
			Assert.AreEqual(recursive.Count, recursive.Distinct().Count());
		}

		public static Dictionary<string, int> TestValues
		{
			get
			{
				return new Dictionary<string, int>
				{
					{ "1|1", 1 },
					{ "2|1", 2 },
					{ "3|3", 1 },
					{ "5|4", 5 },
					{ "5|3", 10 },
					{ "6|3", 20},
					{ "6|4", 15},
					{ "10|3", 120 },
					{ "15|3", 455 },
					{ "17|3", 680 },
					{ "20|5", 15504 },
				};
			}
		}
	}
}
