const { env } = require('process');

// const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
//   env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:16721';

const target = (env.production)? 'https://client.umfa.co.za': 'https://localhost:7072';
//const target = 'https://umfaclientportal.azurewebsites.net';

const PROXY_CONFIG = [
  {
    context: [
      "/user",
      "/home",
      "/AMRScadaUser",
      "/Building",
      "/AMRMeter",
      "/AMRData",
      "/Dashboard",
      "/DXXRDV",
      "/MappedMeters",
      "/ScadaMeters",
      "/Swagger",
      "/Roles",
      "/NotificationTypes",
      "/UserNotifications",
      "/UserNotificationSchedules",
      "/ScadaRequestHeaders",
      "/ScadaRequestDetails",
      "/SupplyTypes",
      "/JobStatus",
      "/ScheduleStatus",
      "/AMRMeterGraphs",
      "/AlarmNightFlow",
      "/AlarmBurstPipe",
      "/AlarmLeakDetection",
      "/AlarmAverageUsage",
      "/AlarmDailyUsage",
      "/AlarmPeakUsage",
      "/UserAlarmNotificationsConfig",
      "/AlarmsPerBuilding",
      "/AlarmTriggered",
      "/TriggeredAlarmNotifications",
      "/Reports",
      "/Umfa",
      "/TenantSlips",
      "/TenantDashboard",
      "/SmartServices",
   ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
