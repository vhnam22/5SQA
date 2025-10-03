using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using _5S_QA_Entities.Models;

namespace _5S_QA_Client.Controls;

internal class MachineFilter
{
	public static List<Result> FilterResult_CLP_35(string fileName, int sample = 1, int cavity = 1)
	{
		List<List<string>> list = new List<List<string>>();
		List<string> list2 = new List<string>();
		string[] array = File.ReadAllLines(fileName);
		bool flag = false;
		string[] array2 = array;
		foreach (string text in array2)
		{
			string text2 = text.Trim();
			if (flag)
			{
				if (!text2.StartsWith("**** END"))
				{
					if (text2.Contains("*** GEAR") || text2.StartsWith("1002."))
					{
						list2.Add(text2);
					}
				}
				else if (list2.Count > 0)
				{
					list.Add(new List<string>(list2));
					list2.Clear();
				}
			}
			else if (text2.StartsWith("**** END"))
			{
				flag = true;
			}
		}
		List<Result> list3 = new List<Result>();
		foreach (List<string> item in list)
		{
			List<Result> list4 = AnalysisValue_CLP_35(item, sample, cavity);
			if (list4.Count > 0)
			{
				list3.AddRange(list4);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list3
			group x by x.Name;
		foreach (IGrouping<string, Result> item2 in enumerable)
		{
			if (item2.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item3 in item2)
			{
				num++;
				item3.Name += $"_{num}";
			}
		}
		return list3;
	}

	public static List<Result> FilterResult_ADCOLE911(string fileName, int sample = 1, int cavity = 1)
	{
		List<List<string>> list = new List<List<string>>();
		List<string> list2 = new List<string>();
		string[] array = File.ReadAllLines(fileName);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.Trim().StartsWith("Gaussian Filtering:") || text.Trim().StartsWith("End of sequence"))
			{
				if (list2.Count > 0)
				{
					list.Add(new List<string>(list2));
				}
				list2.Clear();
			}
			else
			{
				list2.Add(text);
			}
		}
		List<Result> list3 = new List<Result>();
		foreach (List<string> item in list)
		{
			List<Result> list4 = AnalysisValue_ADCOLE911(item, sample, cavity);
			if (list4.Count > 0)
			{
				list3.AddRange(list4);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list3
			group x by x.Name;
		foreach (IGrouping<string, Result> item2 in enumerable)
		{
			if (item2.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item3 in item2)
			{
				num++;
				item3.Name += $"_{num}";
			}
		}
		return list3;
	}

	public static List<Result> FilterResult_ROUNDNESS_ACCTee(List<string> fileNames, int sample = 1, int cavity = 1)
	{
		List<Result> list = new List<Result>();
		foreach (string fileName in fileNames)
		{
			Result val = AnalysisValue_ROUNDNESS_ACCTee(fileName, sample, cavity);
			if (val != null)
			{
				list.Add(val);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list
			group x by x.Name;
		foreach (IGrouping<string, Result> item in enumerable)
		{
			if (item.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item2 in item)
			{
				num++;
				item2.Name += $"_{num}";
			}
		}
		return list;
	}

	public static List<Result> FilterResult_CONTOUR_ACCTee(List<string> fileNames, int sample = 1, int cavity = 1, bool isConvert = true)
	{
		List<Result> list = new List<Result>();
		foreach (string fileName in fileNames)
		{
			List<Result> list2 = AnalysisValue_CONTOUR_ACCTee(fileName, sample, cavity, isConvert);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list
			group x by x.Name;
		foreach (IGrouping<string, Result> item in enumerable)
		{
			if (item.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item2 in item)
			{
				num++;
				item2.Name += $"_{num}";
			}
		}
		return list;
	}

	public static List<Result> FilterResult_SUFRCOM_ACCTee(List<string> fileNames, int sample = 1, int cavity = 1)
	{
		List<Result> list = new List<Result>();
		foreach (string fileName in fileNames)
		{
			List<Result> list2 = AnalysisValue_SUFRCOM_ACCTee(fileName, sample, cavity);
			if (list2 != null)
			{
				list.AddRange(list2);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list
			group x by x.Name;
		foreach (IGrouping<string, Result> item in enumerable)
		{
			if (item.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item2 in item)
			{
				num++;
				item2.Name += $"_{num}";
			}
		}
		return list;
	}

	public static List<Result> FilterResult_CRYSTA(string fileName, int sample = 1, int cavity = 1, bool isConvert = true)
	{
		List<string> list = new List<string>();
		string[] array = File.ReadAllLines(fileName);
		bool flag = false;
		string[] array2 = array;
		foreach (string text in array2)
		{
			string text2 = text.Trim();
			if (flag)
			{
				if (!string.IsNullOrEmpty(Regex.Match(text2, "^\\d*(?=\\s)").Value))
				{
					list.Add(text);
				}
			}
			else if (text2.StartsWith("Tolerance"))
			{
				flag = true;
			}
		}
		List<Result> list2 = new List<Result>();
		foreach (string item in list)
		{
			Result val = AnalysisValue_CRYSTA(item, sample, cavity, isConvert);
			if (val != null)
			{
				list2.Add(val);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list2
			group x by x.Name;
		foreach (IGrouping<string, Result> item2 in enumerable)
		{
			if (item2.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item3 in item2)
			{
				num++;
				item3.Name += $"_{num}";
			}
		}
		return list2;
	}

	public static List<Result> FilterResult_SE3500(string fileName, int sample = 1, int cavity = 1)
	{
		List<string> list = new List<string>();
		string[] array = File.ReadAllLines(fileName);
		string[] array2 = array;
		foreach (string text in array2)
		{
			string input = text.Trim();
			if (!string.IsNullOrEmpty(Regex.Match(input, "^\\d*(?=\\s)").Value))
			{
				list.Add(text);
			}
		}
		List<Result> list2 = new List<Result>();
		foreach (string item in list)
		{
			Result val = AnalysisValue_SE3500(item, sample, cavity);
			if (val != null)
			{
				list2.Add(val);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list2
			group x by x.Name;
		foreach (IGrouping<string, Result> item2 in enumerable)
		{
			if (item2.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item3 in item2)
			{
				num++;
				item3.Name += $"_{num}";
			}
		}
		return list2;
	}

	public static List<Result> FilterResult_EF3000(string fileName, int sample = 1, int cavity = 1, bool isConvert = true)
	{
		List<Result> list = new List<Result>();
		List<string> list2 = new List<string>();
		string[] array = File.ReadAllLines(fileName);
		if (array.Length < 1)
		{
			return list;
		}
		string title = array[0];
		string[] array2 = array;
		foreach (string text in array2)
		{
			string input = text.Trim();
			if (!string.IsNullOrEmpty(Regex.Match(input, "^\\d*(?=,)").Value))
			{
				list2.Add(text);
			}
		}
		foreach (string item in list2)
		{
			List<Result> list3 = AnalysisValue_EF3000(item, sample, cavity, title, isConvert);
			if (list3 != null)
			{
				list.AddRange(list3);
			}
		}
		IEnumerable<IGrouping<string, Result>> enumerable = from x in list
			group x by x.Name;
		foreach (IGrouping<string, Result> item2 in enumerable)
		{
			if (item2.Count() < 2)
			{
				continue;
			}
			int num = 0;
			foreach (Result item3 in item2)
			{
				num++;
				item3.Name += $"_{num}";
			}
		}
		return list;
	}

	private static List<Result> AnalysisValue_CLP_35(List<string> datas, int sample, int cavity)
	{
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		List<Result> list = new List<Result>();
		if (datas.Count != 2)
		{
			return list;
		}
		string value = Regex.Match(datas.First().Replace("*** GEAR NO.", "").Trim(), "\\D.*").Value;
		if (!value.Contains("LM") && !value.Contains("RM"))
		{
			return list;
		}
		string[] source = datas.Last().Split(':');
		string[] array = source.Last().Split(',');
		if (array.Length < 16)
		{
			return list;
		}
		if (value.Contains("Lead"))
		{
			if (double.TryParse(array[1], out var result))
			{
				list.Add(new Result
				{
					Name = value + "#FH",
					Actual = result,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[0], out result))
			{
				list.Add(new Result
				{
					Name = value + "#CRN",
					Actual = result,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[2], out result))
			{
				list.Add(new Result
				{
					Name = value + "#FQ",
					Actual = result,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[3], out result))
			{
				list.Add(new Result
				{
					Name = value + "#FQA",
					Actual = result,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[12], out result))
			{
				list.Add(new Result
				{
					Name = value + "#LCW",
					Actual = result,
					Cavity = cavity,
					Sample = sample
				});
			}
		}
		else
		{
			if (double.TryParse(array[1], out var result2))
			{
				list.Add(new Result
				{
					Name = value + "#FA",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[0], out result2))
			{
				list.Add(new Result
				{
					Name = value + "#FF",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[2], out result2))
			{
				list.Add(new Result
				{
					Name = value + "#FFA",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[3], out result2))
			{
				list.Add(new Result
				{
					Name = value + "#BS",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[3], out result2))
			{
				list.Add(new Result
				{
					Name = value + "#TC",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[12], out result2))
			{
				list.Add(new Result
				{
					Name = value + "#ICW",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
			if (double.TryParse(array[8], out result2))
			{
				list.Add(new Result
				{
					Name = value + "#FFT",
					Actual = result2,
					Cavity = cavity,
					Sample = sample
				});
			}
		}
		return list;
	}

	private static List<Result> AnalysisValue_ADCOLE911(List<string> datas, int sample, int cavity)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		List<Result> list = new List<Result>();
		if (datas.Count < 2 || datas.Any((string x) => !x.StartsWith(" ")))
		{
			return list;
		}
		for (int num = 1; num < datas.Count; num++)
		{
			string[] source = datas[num].Split('=');
			if (source.Count() >= 2 && double.TryParse(Regex.Match(datas[num], "(?<=\\=)\\s*([-+]?\\d*\\.\\d+|\\d+)").Value, out var result))
			{
				list.Add(new Result
				{
					Name = datas.First().Trim() + "_" + source.First().Trim(),
					Actual = result,
					Cavity = cavity,
					Sample = sample
				});
			}
		}
		return list;
	}

	private static Result AnalysisValue_ROUNDNESS_ACCTee(string fileName, int sample, int cavity)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		string[] array = fileName.Split('\\');
		string text = array[array.Length - 2];
		string[] array2 = File.ReadAllLines(fileName);
		if (array2.Length < 3)
		{
			return null;
		}
		string[] array3 = Path.GetFileNameWithoutExtension(fileName).Split('_');
		if (array3.Length < 4)
		{
			return null;
		}
		string text2 = array3[2];
		string value = Regex.Match(array2[2], "\\d+\\.\\d+").Value;
		if (!double.TryParse(value, out var result))
		{
			return null;
		}
		return new Result
		{
			Name = text + "#" + text2,
			Actual = result,
			Cavity = cavity,
			Sample = sample
		};
	}

	private static List<Result> AnalysisValue_CONTOUR_ACCTee(string fileName, int sample, int cavity, bool isConvert)
	{
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		string[] array = fileName.Split('\\');
		string text = array[array.Length - 2];
		string[] array2 = File.ReadAllLines(fileName);
		if (array2.Length < 1)
		{
			return null;
		}
		List<List<string>> list = new List<List<string>>();
		List<string> list2 = new List<string>();
		foreach (var item in array2.Select((string value2, int index) => new
		{
			index = index,
			value = value2
		}))
		{
			string text2 = item.value.Trim();
			if (string.IsNullOrEmpty(text2) || text2.Contains("Calculation type"))
			{
				continue;
			}
			if (text2.Contains("[") && text2.EndsWith("]"))
			{
				if (list2.Count > 0)
				{
					list.Add(new List<string>(list2));
				}
				list2.Clear();
			}
			list2.Add(text2);
			if (item.index.Equals(array2.Length - 1))
			{
				if (list2.Count > 0)
				{
					list.Add(new List<string>(list2));
				}
				list2.Clear();
			}
		}
		List<Result> list3 = new List<Result>();
		foreach (List<string> item2 in list)
		{
			if (item2.Count < 2)
			{
				continue;
			}
			string value = Regex.Match(item2.First(), "(?<=\\[).*(?=\\])").Value;
			if (string.IsNullOrEmpty(value))
			{
				continue;
			}
			for (int num = 1; num < item2.Count; num++)
			{
				string text3 = item2[num].Trim();
				string text4 = text3.Split('\t').Last().Replace("mm", "");
				int length = text3.IndexOf(text4);
				string text5 = text3.Substring(0, length).Trim();
				double result;
				Result val;
				if (text4.Contains("\ufffd"))
				{
					if (!isConvert)
					{
						val = new Result();
						val.Name = text + "#" + value + "#" + text5;
						val.Actual = text4;
						val.Sample = sample;
						val.Cavity = cavity;
						list3.Add(val);
						continue;
					}
					result = ConvertAngleToDoubleForACCTee(text4);
				}
				else if (!double.TryParse(text4, out result))
				{
					continue;
				}
				val = new Result();
				val.Name = text + "#" + value + "#" + text5;
				val.Actual = result;
				val.Sample = sample;
				val.Cavity = cavity;
				list3.Add(val);
			}
		}
		return list3;
	}

	private static List<Result> AnalysisValue_SUFRCOM_ACCTee(string fileName, int sample, int cavity)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		string[] array = fileName.Split('\\');
		string text = array[array.Length - 2];
		string[] array2 = File.ReadAllLines(fileName);
		if (array2.Length < 1)
		{
			return null;
		}
		List<Result> list = new List<Result>();
		string[] array3 = array2;
		foreach (string text2 in array3)
		{
			string text3 = text2.Trim();
			string value = Regex.Match(text3, "(?<=\\s)[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)(?=\\s)").Value;
			if (double.TryParse(value, out var result))
			{
				int length = text3.IndexOf(value);
				string text4 = text3.Substring(0, length).Trim();
				list.Add(new Result
				{
					Name = text + "#" + text4,
					Actual = result,
					Sample = sample,
					Cavity = cavity
				});
			}
		}
		return list;
	}

	private static Result AnalysisValue_CRYSTA(string data, int sample, int cavity, bool isConvert)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		if (data.Length < 74)
		{
			return null;
		}
		string name = Common.trimSpace(data.Substring(21, 25));
		string text = data.Substring(42);
		string[] array = text.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length < 2)
		{
			return null;
		}
		string text2 = array[1];
		if (array.Length > 2)
		{
			text2 = array[2];
		}
		double result;
		if (text2.Contains(":"))
		{
			if (!isConvert)
			{
				return new Result
				{
					Name = name,
					Actual = text2,
					Cavity = cavity,
					Sample = sample
				};
			}
			result = ConvertAngleToDouble(text2);
		}
		else if (!double.TryParse(text2, out result))
		{
			return null;
		}
		return new Result
		{
			Name = name,
			Actual = result,
			Cavity = cavity,
			Sample = sample
		};
	}

	private static Result AnalysisValue_SE3500(string data, int sample, int cavity)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		string[] array = data.Split('\t');
		if (array.Length < 5)
		{
			return null;
		}
		string name = array[2].Trim();
		if (!double.TryParse(array.Last(), out var result))
		{
			return null;
		}
		return new Result
		{
			Name = name,
			Actual = result,
			Cavity = cavity,
			Sample = sample
		};
	}

	private static List<Result> AnalysisValue_EF3000(string data, int sample, int cavity, string title, bool isConvert)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		string[] array = title.Split(',');
		string[] array2 = data.Split(',');
		if (array2.Length < 20 || array2.Length != array.Length)
		{
			return null;
		}
		List<Result> list = new List<Result>();
		string text = array2[1].Trim().Replace("\"", "");
		for (int i = 4; i < array.Length; i++)
		{
			string text2 = array[i].Trim().Replace("\"", "");
			if (!double.TryParse(array2[i], out var result))
			{
				if (!array2[i].Contains("'"))
				{
					continue;
				}
				if (!isConvert)
				{
					list.Add(new Result
					{
						Name = text + "#" + text2,
						Actual = array2[i],
						Sample = sample,
						Cavity = cavity
					});
					continue;
				}
				result = ConvertDegreeAngleToDouble(array2[i]);
			}
			list.Add(new Result
			{
				Name = text + "#" + text2,
				Actual = result,
				Sample = sample,
				Cavity = cavity
			});
		}
		return list;
	}

	private static double ConvertDegreeAngleToDouble(string valuestr)
	{
		int num = valuestr.IndexOf('.');
		string s = valuestr.Substring(0, num);
		double.TryParse(s, out var result);
		int num2 = valuestr.IndexOf('\'');
		s = valuestr.Substring(num + 1, num2 - num - 1);
		double.TryParse(s, out var result2);
		int num3 = valuestr.IndexOf('"');
		s = valuestr.Substring(num2 + 1, num3 - num2 - 1);
		double.TryParse(s, out var result3);
		return Math.Round(result + result2 / 60.0 + result3 / 3600.0, 4);
	}

	private static double ConvertAngleToDouble(string valuestr)
	{
		string[] array = valuestr.Split(':');
		if (array.Length < 3)
		{
			return 0.0;
		}
		double.TryParse(array.First(), out var result);
		double.TryParse(array[1], out var result2);
		double.TryParse(array.Last(), out var result3);
		return Math.Round(result + result2 / 60.0 + result3 / 3600.0, 4);
	}

	private static double ConvertAngleToDoubleForACCTee(string valuestr)
	{
		int num = valuestr.IndexOf('\ufffd');
		string s = valuestr.Substring(0, num);
		double.TryParse(s, out var result);
		int num2 = valuestr.IndexOf('\'');
		s = valuestr.Substring(num + 1, num2 - num - 1);
		double.TryParse(s, out var result2);
		int num3 = valuestr.IndexOf('"');
		s = valuestr.Substring(num2 + 1, num3 - num2 - 1);
		double.TryParse(s, out var result3);
		return Math.Round(result + result2 / 60.0 + result3 / 3600.0, 4);
	}
}
