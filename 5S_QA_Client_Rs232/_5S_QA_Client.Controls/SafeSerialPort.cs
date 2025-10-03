using System;
using System.IO;
using System.IO.Ports;

namespace _5S_QA_Client.Controls;

public class SafeSerialPort : SerialPort
{
	private Stream theBaseStream;

	public string MachineName { get; set; }

	public string MachineType { get; set; }

	public SafeSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		: base(portName, baudRate, parity, dataBits, stopBits)
	{
	}

	public SafeSerialPort(string portName, int baudRate)
		: base(portName, baudRate)
	{
	}

	public SafeSerialPort()
	{
	}

	public new bool Open()
	{
		bool result = true;
		try
		{
			base.Open();
			theBaseStream = base.BaseStream;
			GC.SuppressFinalize(base.BaseStream);
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public new void Dispose()
	{
		Dispose(disposing: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && base.Container != null)
		{
			base.Container.Dispose();
		}
		try
		{
			if (theBaseStream != null && theBaseStream.CanRead)
			{
				theBaseStream.Close();
				GC.ReRegisterForFinalize(theBaseStream);
			}
		}
		catch
		{
		}
		base.Dispose(disposing);
	}
}
