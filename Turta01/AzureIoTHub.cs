using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client; 

namespace Turta01
{
    class AzureIoTHub
    {

         public static async void SendDevieceToCloudMessageAsync(string mesaj) 
        {
            string iotHubUri = "TurtaIoTHub.azure-devices.net";
            string deviceId = "RaspiWin01";
            string deviceKey = "Sl4jGYeuSeCGYPr3fUHsL6aqMTaXOy7gxuumHSLdmnc=";

            var deviceClient = DeviceClient.Create(iotHubUri,
                AuthenticationMethodFactory.
                CreateAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                TransportType.Amqp);

            var message = new Message(Encoding.ASCII.GetBytes(mesaj));

            await deviceClient.SendEventAsync(message);
        }
    }
}
