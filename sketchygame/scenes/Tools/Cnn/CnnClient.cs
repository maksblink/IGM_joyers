using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SketchyGame.scenes.Tools.Cnn;

/// <summary>
/// Klasa odpytująca serwer AI.
/// </summary>
class CnnClient {
    /// <summary>
    /// Metoda wysyłająca obraz w postaci pliku json do serwera AI i czekająca na odpowiedź z nazwą obiektu najbardziej podobnym do obiektu z bazy danych.
    /// </summary>
    /// <param name="data">Dane obrazu w formacie pliku json.</param>
    /// <returns>Zwraca nazwę najbardziej podobnego obiektu.</returns>
    public static Task<string> GetCnnOpinion(string data) {
        string host = "localhost";
        int port = 9999;

        try {
            using (TcpClient client = new TcpClient(host, port)) {
                NetworkStream stream = client.GetStream();

                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                stream.Write(dataBytes, 0, dataBytes.Length);

                byte[] newline = Encoding.UTF8.GetBytes("\n");
                stream.Write(newline, 0, newline.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string received = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                return Task.FromResult(received);
            }
        }
        catch (Exception ex) {
            Console.WriteLine("Error: " + ex.Message);
        }

        return Task.FromResult(string.Empty);
    }
}