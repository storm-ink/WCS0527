using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Newtonsoft.Json;
using NLog;
using Spiral;

public class AppInit
{
    private static Logger _logger = LogManager.GetCurrentClassLogger();

    public static void AppInitialize()
    {
        _logger.Info("正在初始化 Wms");

        HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

        Spiral.NHibernateHelper.Init();

        Client.App.PeriodicWorkerManager.Item<Client.App.RefreshWarningsPeriodicWorker>().Start();

        Client.App.PeriodicWorkerManager.Item<Client.App.WcsLogDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.RailGuidedVehicleDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.SingleForkCraneDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorAlarmDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorAppearanceInspectionDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorLocationTaskDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorHoldSignalDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorOccupyDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorTaskDiagnosisDataPeriodicWorker>().Start();
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorLocationDiagnosisDataPeriodicWorker>().Start();

        Client.App.PeriodicWorkerManager.Item<Client.App.RailGuidedVehicleRefreshStatusPeriodicWorker>().Interval = 5 * 60 * 1000;
        Client.App.PeriodicWorkerManager.Item<Client.App.RailGuidedVehicleRefreshStatusPeriodicWorker>().Start();

        Client.App.PeriodicWorkerManager.Item<Client.App.SingleForkCraneRefreshStatusPeriodicWorker>().Interval = 5 * 60 * 1000;
        Client.App.PeriodicWorkerManager.Item<Client.App.SingleForkCraneRefreshStatusPeriodicWorker>().Start();

        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorRefreshStatusPeriodicWorker>().Interval = 5 * 60 * 1000;
        Client.App.PeriodicWorkerManager.Item<Client.App.ConveyorRefreshStatusPeriodicWorker>().Start();

    }
}
