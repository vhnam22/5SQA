using System;
using System.IO;
using System.IO.Ports;

namespace _5SQA_Read_UWave.Funtions;

public class SafeSerialPort : SerialPort
{
	private Stream theBaseStream;

	public SafeSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		: base(portName, baudRate, parity, dataBits, stopBits)
	{
	}

	public SafeSerialPort()
	{
	}

	public new void Open()
	{
		try
		{
			base.Open();
			theBaseStream = base.BaseStream;
			GC.SuppressFinalize(base.BaseStream);
		}
		catch
		{
		}
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
			if (theBaseStream.CanRead)
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
