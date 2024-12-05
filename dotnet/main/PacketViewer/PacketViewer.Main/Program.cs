using System;
using System.Net.Sockets;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace PacketViewer.Main
{
  using System;
  using System.IO;
  using PacketDotNet; // For packet analysis
  using SharpPcap;    // For packet capture handling

  class Program
  {
    static void Main()
    {
      string pcapFullFilePath = Path.GetFullPath("camera-1.pcapng");
      int packetCount = 0;

      try
      {
        var device = new CaptureFileReaderDevice(pcapFullFilePath);
        Console.WriteLine("Starting PacketCapture File Conversion");
        Console.WriteLine();
        Console.WriteLine($"Reading PacketCapture File: {pcapFullFilePath}");
        Console.WriteLine();

        device.Open();

        RawCapture rawCapture;
        PacketCapture packetCapture = new PacketCapture();

        while (device.GetNextPacket(out packetCapture) == GetPacketStatus.PacketRead)
        {
          packetCount++;

          var packet = Packet.ParsePacket(LinkLayers.Ethernet, packetCapture.Data.ToArray());

          Console.WriteLine($"[INFO] Packet Sequence #: {packetCount}");
          //Console.WriteLine($"[INFO]       PayloadData: {packet.PayloadData}");
          //Console.WriteLine($"[INFO]       PayloadDataSegment: {packet.PayloadDataSegment}");
          Console.WriteLine($"[INFO]       PayloadDataSegment: {packet.HeaderData}");
        }

        device.Close();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error reading the packet capture file: {ex.Message}");
      }
      Console.WriteLine("Packet reading completed.");
      Console.WriteLine("");
      Console.WriteLine("Press ENTER to exit...");
      Console.ReadLine();
    }
  }

  public static class PacketCapturePropertyDisplayer
  {
    public static void DisplayPacketProperties(Packet packet)
    {
      Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd")} [INFO] Packet HeaderData: {packet.HeaderData}");
      Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd")} [INFO] Packet PayloadData: {packet.PayloadData}");
      Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd")} [INFO] Packet PayloadDataSegment: {packet.PayloadDataSegment}");
      Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd")} [INFO] Packet Type: {packet.Bytes.ToString()}");
    }

    public static string GetPacketByteData(Byte[] packetBytes)
    {
      var results = string.Empty;

      foreach (var packetByte in packetBytes)
      {
        results += packetByte.ToString() + " || ";
      }

      return results;
    }
  }
}