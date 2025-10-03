using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Distributions;

namespace _5S_QA_System.Controls;

public static class mMath
{
	public static double CalStandardDeviation(List<double> values)
	{
		double result = 0.0;
		if (values.Count() > 1 && values.Any())
		{
			double avg = values.Average();
			double num = values.Sum((double d) => Math.Pow(d - avg, 2.0));
			result = Math.Sqrt(num / (double)(values.Count() - 1));
		}
		return result;
	}

	public static int CalFrequency(List<double> datas, List<double> bins)
	{
		int num = 0;
		if (bins.Count < 2)
		{
			return datas.Count;
		}
		return datas.Count((double x) => x > bins[0] && x <= bins[1]);
	}

	public static double CalVarianceP(List<double> source)
	{
		double avg = source.Average();
		double num = source.Aggregate(0.0, (double total, double next) => total += Math.Pow(next - avg, 2.0));
		return num / (double)source.Count();
	}

	public static double CalNormdist(double value)
	{
		Normal normal = new Normal();
		return normal.CumulativeDistribution(value);
	}

	public static double CalNormdist(double x, double mean, double std, bool cumulative)
	{
		if (std.Equals(0.0))
		{
			return 0.0;
		}
		if (cumulative)
		{
			return Phi(x, mean, std);
		}
		double num = 1.0 / (Math.Sqrt(Math.PI * 2.0) * std);
		return num * Math.Exp(-0.5 * Math.Pow((x - mean) / std, 2.0));
	}

	private static double Phi(double z)
	{
		return 0.5 * (1.0 + erf(z / Math.Sqrt(2.0)));
	}

	private static double Phi(double z, double mu, double sigma)
	{
		return Phi((z - mu) / sigma);
	}

	private static double erf(double z)
	{
		double num = 1.0 / (1.0 + 0.5 * Math.Abs(z));
		double num2 = 1.0 - num * Math.Exp((0.0 - z) * z - 1.26551223 + num * (1.00002368 + num * (0.37409196 + num * (0.09678418 + num * (-0.18628806 + num * (0.27886807 + num * (-1.13520398 + num * (1.48851587 + num * (-0.82215223 + num * 0.17087277)))))))));
		if (z >= 0.0)
		{
			return num2;
		}
		return 0.0 - num2;
	}
}
