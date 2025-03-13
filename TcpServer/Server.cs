using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace TcpServer
{
    public class Server
    {
        private const int PORTNUMMER = 5346;  

        public void Start()
        {
           
            TcpListener server = new TcpListener(PORTNUMMER);
            server.Start();

            Console.WriteLine($"Server is listening on port {PORTNUMMER}...");

            
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                clientThread.Start(client);  
            }
        }

        
        private static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream networkStream = client.GetStream();
            StreamReader reader = new StreamReader(networkStream);
            StreamWriter writer = new StreamWriter(networkStream);

            try
            {
               

                string request = reader.ReadLine();
                Console.WriteLine($"Received request: {request}");

                
                if (request.StartsWith("Random"))
                {
                    writer.WriteLine("Input numbers");
                    writer.Flush();

                    

                    string numbers = reader.ReadLine();
                    string[] parts = numbers.Split(' ');

                    if (parts.Length == 2 && int.TryParse(parts[0], out int tal1) && int.TryParse(parts[1], out int tal2))
                    {
                        Random rand = new Random();
                        int randomNumber = rand.Next(tal1, tal2 + 1); // Generer et tilfældigt tal mellem tal1 og tal2
                        writer.WriteLine(randomNumber);
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("Invalid input.");
                        writer.Flush();
                    }
                }
                else if (request.StartsWith("Add"))
                {
                    writer.WriteLine("Input numbers");
                    writer.Flush();

                    string numbers = reader.ReadLine();
                    string[] parts = numbers.Split(' ');

                    if (parts.Length == 2 && int.TryParse(parts[0], out int tal1) && int.TryParse(parts[1], out int tal2))
                    {
                        int sum = tal1 + tal2; 
                        writer.WriteLine(sum);
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("Invalid input.");
                        writer.Flush();
                    }
                }
                else if (request.StartsWith("Subtract"))
                {
                    writer.WriteLine("Input numbers");
                    writer.Flush();

                    
                    string numbers = reader.ReadLine();
                    string[] parts = numbers.Split(' ');

                    if (parts.Length == 2 && int.TryParse(parts[0], out int tal1) && int.TryParse(parts[1], out int tal2))
                    {
                        int difference = tal1 - tal2;
                        writer.WriteLine(difference);
                        writer.Flush();
                    }
                    else
                    {
                        writer.WriteLine("Invalid input.");
                        writer.Flush();
                    }
                }
                else
                {
                    writer.WriteLine("Unknown command");
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                client.Close(); 
                Console.WriteLine("Client disconnected.");
            }
        }
    }

    
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start();  
        }
    }
}
