using System;
using PacketDotNet;
using SharpPcap;

class Program
{
    static void Main()
    {
        // Path to the pcapng file
        string filePath = "./camera-1.pcapng";

        try
        {
            // Open the offline pcapng file
            using (var device = new SharpPcap.LibPcap.CaptureFileReaderDevice(filePath))
            {
                device.Open();
                Console.WriteLine($"Reading from file: {filePath}");

                // Loop through packets in the file
                PacketCapture packetCapture;
                while (device.GetNextPacket(out packetCapture) == GetPacketStatus.PacketRead)
                {
                    // Parse the raw packet
                    var packet = Packet.ParsePacket(packetCapture.LinkLayerType, packetCapture.Data);

                    // Display information about the packet
                    Console.WriteLine(packet.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading pcapng file: {ex.Message}");
        }
    }
}