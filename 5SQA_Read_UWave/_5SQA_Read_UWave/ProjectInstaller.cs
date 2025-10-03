using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace _5SQA_Read_UWave;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
	private IContainer components = null;

	private ServiceProcessInstaller serviceProcessInstallerMain;

	private ServiceInstaller serviceInstallerMain;

	public ProjectInstaller()
	{
		InitializeComponent();
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
		serviceProcessInstallerMain = new ServiceProcessInstaller();
		serviceInstallerMain = new ServiceInstaller();
		serviceProcessInstallerMain.Account = ServiceAccount.LocalSystem;
		serviceProcessInstallerMain.Password = null;
		serviceProcessInstallerMain.Username = null;
		serviceInstallerMain.Description = "5SQA Read UWave Service";
		serviceInstallerMain.DisplayName = "5SQA Read UWave Service";
		serviceInstallerMain.ServiceName = "5SQAReadUWaveService";
		serviceInstallerMain.StartType = ServiceStartMode.Automatic;
		base.Installers.AddRange(new Installer[2] { serviceProcessInstallerMain, serviceInstallerMain });
	}
}
