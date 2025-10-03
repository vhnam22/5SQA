using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using _5SQA_Read_UWave.Funtions;
using _5SQA_Read_UWave.Models;

namespace _5SQA_Read_UWave;

public class ServiceMain : ServiceBase
{
	private APIClient client;

	private SafeSerialPort mSerialPort;

	private EventLog eventLogServices;

	private Config mConfig;

	private bool dataProcessing;

	private Timer mTimer;

	private IContainer components = null;

	public ServiceMain()
	{
		InitializeComponent();
		eventLogServices = new EventLog();
		if (!EventLog.SourceExists("5SQAReadUWaveSource"))
		{
			EventLog.CreateEventSource("5SQAReadUWaveSource", "5SQAReadUWaveLog");
		}
		eventLogServices.Source = "5SQAReadUWaveSource";
		eventLogServices.Log = "5SQAReadUWaveLog";
	}

	protected override void OnStart(string[] args)
	{
		eventLogServices.WriteEntry("Started");
		Common.WriteToFile("Log.txt", "Started");
		ReadConfig();
		string iME = Common.GetIME();
		if (iME.Equals(mConfig.IME))
		{
			InitAPI();
			OpenPort();
			CreatedTimer();
		}
	}

	protected override void OnStop()
	{
		eventLogServices.WriteEntry("Stopped");
		Common.WriteToFile("Log.txt", "Stopped");
		if (client != null)
		{
			client.CloseAPI();
		}
		ClosePort();
	}

	private void InitAPI()
	{
		client = new APIClient(mConfig.APIUrl);
	}

	private void CreatedTimer()
	{
		mTimer = new Timer();
		mTimer.Elapsed += OnElapsedTime;
		mTimer.Interval = 5000.0;
		mTimer.Enabled = true;
	}

	private void OnElapsedTime(object source, ElapsedEventArgs e)
	{
		if (mSerialPort == null || !mSerialPort.IsOpen)
		{
			RefreshPort();
		}
	}

	private SafeSerialPort GetPort()
	{
		SafeSerialPort result = null;
		foreach (COMPortInfo item in COMPortInfo.GetCOMPortsInfo())
		{
			if (!string.IsNullOrEmpty(mConfig.ComName) && item.Description.Contains(mConfig.ComName))
			{
				result = new SafeSerialPort
				{
					BaudRate = 57600,
					Handshake = Handshake.RequestToSend,
					PortName = item.Name
				};
				break;
			}
		}
		return result;
	}

	private void OpenPort()
	{
		mSerialPort = GetPort();
		if (mSerialPort != null)
		{
			if (!mSerialPort.IsOpen)
			{
				mSerialPort.Open();
			}
			mSerialPort.DataReceived += mSerialPort_DataReceived;
			Common.WriteToFile("Log.txt", mConfig.ComName + "(" + mSerialPort.PortName + ") is opened");
		}
	}

	private void ClosePort()
	{
		if (mSerialPort != null)
		{
			if (mSerialPort.IsOpen)
			{
				mSerialPort.Close();
			}
			Common.WriteToFile("Log.txt", mConfig.ComName + "(" + mSerialPort.PortName + ") is closed");
			mSerialPort = null;
		}
	}

	private void RefreshPort()
	{
		Common.WriteToFile("Log.txt", "Refresh port");
		mSerialPort = null;
		mSerialPort = GetPort();
		if (mSerialPort != null)
		{
			if (!mSerialPort.IsOpen)
			{
				mSerialPort.Open();
			}
			mSerialPort.DataReceived += mSerialPort_DataReceived;
			Common.WriteToFile("Log.txt", mSerialPort.IsOpen.ToString());
		}
	}

	private void mSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		if (!dataProcessing)
		{
			dataProcessing = true;
			string text = mSerialPort.ReadTo("\r");
			if (text.Length > 0)
			{
				processData(text);
			}
			dataProcessing = false;
		}
	}

	private void processData(string rxString)
	{
		UWave uwave = new UWave(rxString);
		string s = uwave.Digit + uwave.Data;
		Mapper mapper = mConfig.Mappers.Where((Mapper x) => x.ChanelId.Equals(uwave.ChanelId)).FirstOrDefault();
		if (mapper != null && uwave.Header.StartsWith("DT") && double.TryParse(s, out var result))
		{
			ToolResultDto body = new ToolResultDto
			{
				MachineId = mapper.Id,
				Result = result.ToString(),
				Unit = uwave.Unit
			};
			client.SaveAsync(body, "/api/tool/result");
		}
	}

	private void ReadConfig()
	{
		mConfig = Common.ReadFromFileConfig("Config.ini");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
		base.ServiceName = "Service1";
	}
}
