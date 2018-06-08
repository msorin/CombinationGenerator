using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinationGenerator
{
	public class Generator<T>
	{
		public Generator(T[] population, int size)
		{
			if (size > population.Length)
			{
				throw new InvalidOperationException($"Sample size ({size})cannot exceed population ({population.Length})");
			}

			Population = population;
			Size = size;
		}
		public int Size { get; internal set; }
		public T[] Population { get; internal set; }

		public IEnumerable<T[]> GetBitwiseSamples()
		{
			var bitwiseMax = Convert.ToInt64(Math.Pow(2, Population.Length));
			var current = 0L;

			while (++current < bitwiseMax)
			{
				if (GetSetBitCount(current) == Size)
				{
					var bits = GetSetBitSets(current);
					if (bits.Count == Size)
					{
						yield return bits.Select(x => Population[x]).ToArray();
					}
				}
			}

			List<int> GetSetBitSets(long val)
			{
				var bits = new List<int>();
				var ix = 0;
				while (val > 0)
				{
					if ((val & 1) == 1)
					{
						bits.Add(ix);
					}

					ix++;
					val = val >> 1;
				}

				return bits;
			}

			int GetSetBitCount(long val)
			{
				var count = 0;
				BitwiseFunc(val, (a) => { count++; });
				return count;
			}


			void BitwiseFunc(long val, Action<long> func)
			{
				while (val > 0)
				{
					if ((val & 1) == 1)
					{
						func(val);
					}

					val = val >> 1;
				}
			}

		}

		public IEnumerable<T[]> GetSamples()
		{			
			var indices = Enumerable.Range(0, Size).ToList();				//Starting position 0, 1, 2, 3, 4... n-1		
			var max = Population.Length - Size;								//Maximum of 0 index

			while (indices[0] <= max)										//Continue until max is reached
			{
				yield return CreateSample(indices);							//Makes a sample of based on current indecies

				indices[Size - 1]++;										//Increment last index

				for (var position = Size - 1; position > 0; position--)		//Loop until we have reached 0 position in indecies
				{
					if (indices[position] > max + position)					//If current position exceeds position-max update parent and subsequent positions
					{
						indices[position - 1]++;                            //Parent

						for (var update = position; update < Size; update++)//Subsequent
						{
							indices[update] = indices[update - 1] + 1;		//Set to one more than parent
						}
					}
				}
			}			
		}
 
		private T[] CreateSample(List<int> indecies)
		{
			return indecies.Select(x => Population[x]).ToArray();
		}
	}
}