using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TurtaIoTHAT;
using System.Threading;
using System.Diagnostics;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Turta01
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // BME280Sensor
        static BME280Sensor bme;
        // Sensor timer
        Timer sensorTimer;
        static int count = 0;

        public MainPage()
        {
            //this.InitializeComponent();
            // Initialize sensor and timer
            Initialize();
        }

        private async void Initialize()
        {
            // Initialize and configure sensor
            await InitializeBME280Sensor();

            // Configure timer to 2000ms delayed start and 60000ms interval
            sensorTimer = new Timer(new TimerCallback(SensorTimerTick), null, 2000, 10000);
        }

        private async Task InitializeBME280Sensor()
        {
            // Create sensor instance
            bme = new BME280Sensor();

            // Delay 1ms
            await Task.Delay(1);
        }

        private static void SensorTimerTick(object state)
        {
            // Read sensor data
            double derece = bme.ReadTemperature();
            double nem = bme.ReadHumidity();
            double basinc = bme.ReadPressure();

            Debug.WriteLine("Sıcaklık: " + derece.ToString());
            Debug.WriteLine("Nem: " + nem.ToString());
            Debug.WriteLine("Basınç: " + basinc.ToString());

            count = count + 1;
            Debug.WriteLine("Sıra: " + count);
            Debug.WriteLine("--------------");

            Telemetry telemetry = new Telemetry();
            telemetry.Sicaklik = derece;
            telemetry.Nem = nem;
            telemetry.Basinc = basinc;

            // Convert telemetry JSON to string
            string telemetryJSON = JsonConvert.SerializeObject(telemetry);

            // Send data to IoT Hub
            AzureIoTHub.SendDevieceToCloudMessageAsync(telemetryJSON);
        }
    }
}
