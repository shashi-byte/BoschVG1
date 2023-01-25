using BoschVG1.BLL;
using BoschVG1.DAL;
using BoschVG1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace BoschVG1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WSController : ControllerBase
    {
        private readonly IDataRepository _datarepository;
        public WSController(IDataRepository datarepository)
        {
            _datarepository = datarepository;
        }

        [HttpGet("/ws1")]
        public async Task<string> Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);
                return "testPass";
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                return "testFail";
            }
        }

        private async Task Echo(WebSocket webSocket)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            int hours = currentTime.Hours;
            int shift;
            if (hours > 6 && hours < 15)
            {
                shift = 1;
            }
            else if (hours > 15 && hours < 23)
            {
                shift = 2;
            }
            else
            {
                shift = 3;
            }
            int jobId = 10000001;
            var randNum = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int? maxJobId = await _datarepository.GetJobIdDetails();
                var coll = new ObservableCollection<ModelClass>();
                if (maxJobId != null)
                {
                    int JobId = maxJobId.Value + 1;
                    for (int j = 0; j < 4; j++)
                    {
                        ModelClass obj1 = new ModelClass()
                        {
                            job_id = JobId,
                            box_id = randNum.Next(10000, 20000),
                            part_nu = randNum.Next(1000, 2000),
                            quantity = randNum.Next(5, 50),
                            shift = shift,
                            timestamp = DateTime.Now
                        };
                        coll.Add(obj1);
                    }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                    {
                        ModelClass obj1 = new ModelClass()
                        {
                            job_id = jobId,
                            box_id = randNum.Next(10000, 20000),
                            part_nu = randNum.Next(1000, 2000),
                            quantity = randNum.Next(5, 50),
                            shift = shift,
                            timestamp = DateTime.Now
                        };
                        coll.Add(obj1);
                    }
                }
                string message = JsonConvert.SerializeObject(coll);
                var serverMsg = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(serverMsg, WebSocketMessageType.Text,
                    true, CancellationToken.None);
                Thread.Sleep(2000);
            }
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                    "manual closing", CancellationToken.None);

            return;
        }

        [HttpGet("/testListener")]
        public async Task testListener(WebSocket webSocket)
        {
            try
            {
                IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("UNIWINPW0155BP");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint ipEndPoint = new(ipAddress, 80);
                using Socket listener = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

                //listener.Bind(ipEndPoint);
                //listener.Listen(100);
                //var handler = await listener.AcceptAsync();
                //var buffer = new byte[1_024];
                //var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                //var response = Encoding.UTF8.GetString(buffer, 0, received);
                //var a = JsonConvert.SerializeObject(response);
                var randNum = new Random();
                for (int i = 0; i < 1000; i++)
                {
                    var coll = new ObservableCollection<ModelClass>();
                    for (int j = 0; j < 4; j++)
                    {
                        ModelClass obj1 = new ModelClass()
                        {
                            job_id = i,
                            box_id = randNum.Next(10000, 20000),
                            part_nu = randNum.Next(1000, 2000),
                            quantity = randNum.Next(5, 50),
                        };
                        coll.Add(obj1);
                    }
                    string message = JsonConvert.SerializeObject(coll);
                    var serverMsg = Encoding.UTF8.GetBytes(message);
                    await webSocket.SendAsync(serverMsg, WebSocketMessageType.Text,
                        true, CancellationToken.None);
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {

            }
        }

        [HttpGet("/test1")]
        public async Task Test1()
        {
            try
            {
                IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("UNIWINPW0155BP");
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint ipEndPoint = new(ipAddress, 80);
                using Socket client = new(
                ipEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);
                await client.ConnectAsync(ipEndPoint);
                var message = "Hi !<|EOM|>";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                Console.WriteLine($"Socket client sent message: \"{message}\"");

                var buffer = new byte[1_024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                if (response == "<|ACK|>")
                {
                    Console.WriteLine(
                        $"Socket client received acknowledgment: \"{response}\"");
                }

            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost("/test3")]
        public async Task Test3([FromBody] List<ModelClass> vg)
        {
            try
            {
                await _datarepository.SaveData(vg);
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost("/ExcelUpload")]
        public async Task ExcelUpload([FromForm] IFormFile file)
        {
            try
            {
                ExcelDataRead dataread = new ExcelDataRead();
                string filename = Path.GetFileNameWithoutExtension(file.FileName);
                if (filename == "Palletization matrix")
                {
                    var data = await dataread.GetConvertedPalletExcelData(file);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
