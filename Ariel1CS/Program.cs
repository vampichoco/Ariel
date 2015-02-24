
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
static class Module1
{

	public static void Main()
	{
		Console.WriteLine("Write a text to generate");
		OptimalResult = Console.ReadLine().ToUpper();

		Poblation MyGroup = GenerateGroup(300, OptimalResult.Length);
		bool HasResult = false;
		Individual Winner = null;

		System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();
		StopWatch.Start();


		while (HasResult == false) {
			MyGroup.RemoveBadCandidats(OptimalResult);
			MyGroup.GenerateChanges(OptimalResult);


			foreach (Individual item in MyGroup) {
				Console.WriteLine("{0}: Level:{1}, Genomma:{2}", item.Name, item.Evaluate(OptimalResult), item.GenommaString);
				if (item.Evaluate(OptimalResult) == OptimalResult.Length | MyGroup.Count <= 1) {
					HasResult = true;

					Winner = item;
					StopWatch.Stop();
					break; // TODO: might not be correct. Was : Exit For
				}
			}
		}
		Console.WriteLine("Hello, my name is {0} and i got this result: {1}", Winner.Name, Winner.GenommaString);
		Console.WriteLine("My brothers: ");
		dynamic query = from item in MyGroup where item.GenommaString == Winner.GenommaStringitem;

		foreach (Individual item in query) {
			Console.WriteLine("My name is {0} and my genomma is {1}", item.Name, item.GenommaString);
		}
		Console.WriteLine("Elapsed {0}", StopWatch.ElapsedMilliseconds);
		Console.ReadLine();
	}


	private static string OptimalResult;
	public static char RandChar()
	{
		Random RandSeed = new Random();
		return Convert.ToChar(Convert.ToInt32(Math.Floor(26 * RandSeed.NextDouble() + 65)));
	}

	public static string RandString(int Size)
	{
		System.Text.StringBuilder strBuilder = new System.Text.StringBuilder();
		Random RandSeed = new Random();

		char ch = '\0';
		int i = 1;
		for (i = 1; i <= Size; i++)
		{
			ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * RandSeed.NextDouble() + 65)));
			strBuilder.Append(ch);
		}

		return strBuilder.ToString();
	}

	public static Poblation GenerateGroup(int Size, int IndividualSize)
	{
		Poblation Group = new Poblation();

		int i = 0;
		int Max = Size;

		for (i = 0; i <= Max; i++)
		{
			Individual newIndividual = new Individual(string.Format("Individual_{0}", i));
			newIndividual.Genomma = RandString(IndividualSize);

			Group.Add(newIndividual);

		}

		return Group;

	}


	public class Individual
	{
		private char[] _Genomma;
		private string _Name;
		private Individual _Father;

		private Individual _Mather;
		public Individual Father
		{
			get { return _Father; }
			set { _Father = value; }
		}

		public Individual Mather
		{
			get { return _Mather; }
			set { _Mather = value; }
		}


		public string Name
		{
			get { return _Name; }
		}

		public char[] Genomma
		{
			get { return _Genomma; }
			set { _Genomma = value; }
		}

		public string GenommaString
		{
			get { return this.Genomma; }
		}

		public int Evaluate(string CompareWith)
		{
			int i = 0;
			int Max = CompareWith.Length - 1;

			int Calification = 0;

			for (i = 0; i <= Max; i++)
			{
				char GennomaCromosome = this.Genomma[i];
				char OptimalCromosome = CompareWith[i];

				if (GennomaCromosome == OptimalCromosome)
				{
					Calification += 1;
				}

			}

			return Calification;

		}

		public Individual()
		{
			_Name = Guid.NewGuid().ToString();
		}

		public Individual(string Name)
		{
			_Name = Name;
		}

	}

	public class Poblation : List<Individual>
	{

		public void RemoveBadCandidats(string OptimalResult)
		{
			//Dim RegularAvergate = Me.Average
			List<Individual> ToRemove = new List<Individual>();

			int i = 0;
			int Max = this.Count - 1;

			for (i = 0; i <= Max; i++)
			{
				try
				{
					dynamic AttackingIndivdual = this[i];

					Random RandSeed = new Random();


					dynamic AttackedIndividual = this[RandSeed.Next(0, Max)];

					int EvaluationOfAttackingIndividual = AttackingIndivdual.Evaluate(OptimalResult);

					int EvaluationOfAttackedIndividual = AttackedIndividual.Evaluate(OptimalResult);

					if (AttackingIndivdual.Evaluate(OptimalResult) > AttackedIndividual.Evaluate(OptimalResult))
					{
						Console.WriteLine("My name is {0} and i will die, look at my genomma:{1}", AttackedIndividual.Name, AttackedIndividual.GenommaString);
						//Console.ReadLine()
						this.Remove(AttackedIndividual);
					}
					else
					{
						if (AttackedIndividual.Evaluate(OptimalResult) > AttackingIndivdual.Evaluate(OptimalResult))
						{
							Console.WriteLine("My name is {0} and i will die, look at my genomma:{1}", AttackedIndividual.Name, AttackedIndividual.GenommaString);
							//Console.ReadLine()
							this.Remove(AttackingIndivdual);
						}

					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("==" + ex.Message);
				}
			}

			//For Each Item As Individual In ToRemove
			//    Console.WriteLine(, Item.Name, Item.GenommaString)
			//    Console.ReadLine()
			//    Me.Remove(Item)
			//Next
		}

		public void GenerateChanges(string CompareWith)
		{

			foreach (Individual Item in this)
			{
				int I = 0;
				int Max = CompareWith.Length - 1;

				for (I = 0; I <= Max; I++)
				{
					char GennomaCromosome = Item.Genomma[I];
					char OptimalCromosome = CompareWith[I];

					if (!(GennomaCromosome == OptimalCromosome))
					{
						Item.Genomma[I] = Module1.RandChar();
					}
				}

			}
		}

		public void CreateSon(Individual Father, Individual Mather, string OptimalResult)
		{
			if (Father.Evaluate(OptimalResult) == Mather.Evaluate(OptimalResult))
			{
				if (!(Father.GenommaString == Mather.GenommaString))
				{
					Individual son = new Individual();
					son.Father = Father;
					son.Mather = Mather;

					son.Genomma = Module1.RandString(OptimalResult.Length);

					Random RandSeed = new Random();

					int randPos = RandSeed.Next(0, OptimalResult.Length - 1);
					//    Dim SideX(randPos) As Char
					//    Dim SideY(OptimalResult.Length - randPos)

					int ri = 0;
					for (ri = 0; ri <= randPos; ri++)
					{
						son.Genomma[ri] = Father.Genomma[ri];
					}

					ri = 0;

					for (ri = randPos; ri <= (OptimalResult.Length - 1); ri++)
					{
						son.Genomma[ri] = Mather.Genomma[ri];
					}

					bool MutedGenomma = false;
					while (MutedGenomma == false)
					{
						int Index = RandSeed.Next(0, OptimalResult.Length);
						char NewCromosome = Module1.RandChar();

						if (!(son.Genomma[Index] == OptimalResult[Index]))
						{
							son.Genomma[Index] = NewCromosome;
							this.Remove(Father);
							this.Remove(Mather);
							MutedGenomma = true;
							//Console.ReadLine()
						}

					}

					Console.WriteLine("Hello i'm new at this poblation, my name is: {0} And my gennoma is {1}", son.Name, son.GenommaString);
					this.Add(son);

				}
			}
		}

		public void CreateNewGeneration(string OptimalResult)
		{
			int I = 0;
			int Max = this.Count - 1;

			Random RandSeed = new Random();
			try
			{
				for (I = 0; I <= Max; I++)
				{
					Individual Father = this[I];
					Individual Mather = this[RandSeed.Next(0, this.Count - 1)];

					CreateSon(Father, Mather, OptimalResult);

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("==ups");
			}

		}

		public int Average
		{
			get
			{
				int Count = 0;
				foreach (Individual Item in this)
				{
					Count += Item.Evaluate(Module1.OptimalResult);
				}

				return Count / this.Count;
			}
		}

	}

}
