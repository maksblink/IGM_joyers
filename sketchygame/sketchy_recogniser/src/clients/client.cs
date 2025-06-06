using System;
using System.Net.Sockets;
using System.Text;
// Example client implementation in C#.
//
//    'Connects to server and sends test data.
//
//    'Methods:
//        'Main(): Entry point
//           ' - Connects to localhost:9999
//            'Sends hardcoded test data
//           'Receives and displays response
class Client {

    static void Main(string[] args) {
        string host = "localhost";
        int port = 9999;
        string data = "{\"image\": [0,0,0,1,1,1,2,2,2]}";

        try {
            using (TcpClient client = new TcpClient(host, port)) {
                NetworkStream stream = client.GetStream();

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                stream.Write(dataBytes, 0, dataBytes.Length);

                byte[] newline = Encoding.UTF8.GetBytes("\n");
                stream.Write(newline, 0, newline.Length);

                Console.WriteLine("Sent:    " + data);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine("Received: " + received);
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}