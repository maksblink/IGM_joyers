using System;
using System.Net.Sockets;
using System.Text;

namespace SketchyGame.scenes.gui_components;

class CnnClient
{
    public static string GetCnnOpinion(string data)
    {
        string host = "localhost";
        int port = 9999;

        try
        {
            using (TcpClient client = new TcpClient(host, port))
            {
                NetworkStream stream = client.GetStream();

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                stream.Write(dataBytes, 0, dataBytes.Length);

                byte[] newline = Encoding.UTF8.GetBytes("\n");
                stream.Write(newline, 0, newline.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                return received;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return string.Empty;
    }
    
}