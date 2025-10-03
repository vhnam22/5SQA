using System;
using System.Collections.Generic;
using System.Linq;
using _5S_QA_Entities.Models;

namespace _5S_QA_System.Controls;

public class LinearTrend
{
	public readonly List<ValueItem> DataItems;

	public readonly List<ValueItem> TrendItems;

	public int Count { get; private set; }

	public double? Slope { get; private set; }

	public double? Intercept { get; private set; }

	public double? Correl { get; private set; }

	public double? R2 { get; private set; }

	public ValueItem StartPoint => TrendItems.OrderBy((ValueItem item) => item.X).FirstOrDefault();

	public ValueItem EndPoint => TrendItems.OrderByDescending((ValueItem item) => item.X).FirstOrDefault();

	public LinearTrend(List<double> xValues, List<double> yValues)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		DataItems = new List<ValueItem>();
		Count = ((xValues.Count > yValues.Count) ? yValues.Count : xValues.Count);
		for (int i = 0; i < Count; i++)
		{
			DataItems.Add(new ValueItem
			{
				X = xValues[i],
				Y = yValues[i]
			});
		}
		TrendItems = new List<ValueItem>();
		Calculate();
	}

	private void Calculate()
	{
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Expected O, but got Unknown
		if (DataItems.Count == 0)
		{
			return;
		}
		double averageX = DataItems.Average((ValueItem val) => val.X);
		double averageY = DataItems.Average((ValueItem val) => val.Y);
		double num = DataItems.Sum((ValueItem val) => (val.X - averageX) * (val.Y - averageY));
		double num2 = DataItems.Sum((ValueItem val) => Math.Pow(val.X - averageX, 2.0));
		Slope = num / num2;
		Intercept = averageY - Slope * averageX;
		double num3 = Math.Sqrt(DataItems.Sum((ValueItem val) => Math.Pow(val.X - averageX, 2.0)) * DataItems.Sum((ValueItem val) => Math.Pow(val.Y - averageY, 2.0)));
		Correl = num / num3;
		foreach (ValueItem item in DataItems.OrderBy((ValueItem dataItem) => dataItem.X))
		{
			if (TrendItems.Where((ValueItem existingItem) => existingItem.X == item.X).FirstOrDefault() == null)
			{
				ValueItem item2 = new ValueItem
				{
					X = item.X,
					Y = Slope.Value * item.X + Intercept.Value
				};
				TrendItems.Add(item2);
			}
		}
		double num4 = DataItems.Sum((ValueItem dataItem) => Math.Pow(dataItem.Y - TrendItems.Where((ValueItem calcItem) => calcItem.X == dataItem.X).First().Y, 2.0));
		double num5 = DataItems.Sum((ValueItem dataItem) => Math.Pow(dataItem.Y, 2.0)) - Math.Pow(DataItems.Sum((ValueItem dataItem) => dataItem.Y), 2.0) / (double)DataItems.Count;
		R2 = 1.0 - num4 / num5;
	}
}
