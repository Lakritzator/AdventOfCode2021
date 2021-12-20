using System.Diagnostics;
using System.Globalization;
using BitStreams;

namespace AdventOfCode.Solutions;

/// <summary>
/// Solution to https://adventofcode.com/2021/day/16
/// </summary>
public class Day16 : AdventOfCodeBase
{
    private IList<Packet> _packets;

    public Day16()
    {
        Initialize(this.InputFilename);
        //Initialize(this.InputExampleFilename);
    }

    /// <summary>
    /// Part one only calculates the sum of all versions in all packets and sub packets
    /// </summary>
    /// <returns>string for the main routine</returns>
    public override string AnswerPartOne()
    {
        uint answer = 0;
        foreach (var packet in _packets)
        {
            answer += CalculateVersionSum(packet);
        }
        return $"Answer 1: {answer}";
    }

    /// <summary>
    /// Part two processes all operators
    /// </summary>
    /// <returns>string for the main routine</returns>
    public override string AnswerPartTwo()
    {
        long answer = 0;
        foreach (var packet in _packets)
        {
            answer += ProcessOperation(packet);
        }
        return $"Answer 2: {answer}";
    }

    /// <summary>
    /// Read the input, and process it to packets which are used in the part one and two
    /// </summary>
    /// <param name="path">Path to the file with the input</param>
    private void Initialize(string path)
    {
        Assert.True(File.Exists(path));
        var lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
        var hexSpan = lines.First().AsSpan();

        using var stream = new MemoryStream();
        var i = 0;
        while (i < hexSpan.Length)
        {
            var hexByteString = hexSpan.Slice(i, 2);
            var hexValue = byte.Parse(hexByteString, NumberStyles.AllowHexSpecifier);
            stream.WriteByte(hexValue);
            i += 2;
        }

        stream.Seek(0, SeekOrigin.Begin);
        var bitStream = new BitStream(stream, true)
        {
            AutoIncreaseStream = true
        };
        var bitsAvailable = bitStream.Length * 8;
        var bitsRead = 0;
        var packets = new List<Packet>();
        while (bitsAvailable - bitsRead > 6)
        {
            bitsRead += ReadPacket(bitStream, packets);
        }

        _packets = packets;
    }

    /// <summary>
    /// Look at the packet and process the value according to the operation it represents
    /// </summary>
    /// <param name="packet">Packet</param>
    /// <param name="indent">string with the indention</param>
    /// <returns>int with the value after processing the operation</returns>
    private static long ProcessOperation(Packet packet, string indent = "")
    {
        var newIndent = indent += "\t";
        var result = packet.PacketType switch
        {
            PacketTypes.Literal => packet.Value,
            PacketTypes.Sum => packet.SubPackets.Sum(sp => ProcessOperation(sp, newIndent)),
            PacketTypes.Minimum => packet.SubPackets.Min(sp => ProcessOperation(sp, newIndent)),
            PacketTypes.Maximum => packet.SubPackets.Max(sp => ProcessOperation(sp, newIndent)),
            PacketTypes.Product => packet.SubPackets.Aggregate((long)1, (current, subPacket) => current * ProcessOperation(subPacket, newIndent)),
            PacketTypes.EqualTo => ProcessOperation(packet.SubPackets[0], newIndent) == ProcessOperation(packet.SubPackets[1], newIndent) ? 1 : 0,
            PacketTypes.LessThan => ProcessOperation(packet.SubPackets[0], newIndent) < ProcessOperation(packet.SubPackets[1], newIndent) ? 1 : 0,
            PacketTypes.GreaterThan => ProcessOperation(packet.SubPackets[0], newIndent) > ProcessOperation(packet.SubPackets[1], newIndent) ? 1 : 0,
            _ => throw new ArgumentOutOfRangeException()
        };
        Debug.WriteLine($"{indent}{packet.PacketType} result: {result}");
        return result;
    }

    /// <summary>
    /// Calculate the sum of the versions, this is for part one
    /// </summary>
    /// <param name="packet">Packet</param>
    /// <returns>int with sum of versions</returns>
    private uint CalculateVersionSum(Packet packet)
    {
        uint result = packet.Version;

        foreach (var subPacket in packet.SubPackets)
        {
            result += CalculateVersionSum(subPacket);
        }

        return result;
    }

    /// <summary>
    /// Recursively read the packets
    /// </summary>
    /// <param name="bitStream">BitStream</param>
    /// <param name="packets">IList with packet</param>
    /// <returns></returns>
    private static int ReadPacket(BitStream bitStream, IList<Packet> packets)
    {
        uint version = ReadInt(bitStream, 3);
        var packetType = (PacketTypes)ReadInt(bitStream, 3);
        var packet = new Packet(version, packetType);
        packets.Add(packet);

        int bitsRead = 6;
        switch (packetType)
        {
            case PacketTypes.Literal:
                long literal = 0;
                while (bitStream.ReadBit() == 1)
                {
                    bitsRead += 5;
                    literal <<= 4;
                    literal |= ReadInt(bitStream, 4);
                }
                bitsRead += 5;
                literal <<= 4;
                literal |= ReadInt(bitStream, 4);
                packet.Value = literal;
                break;
            // All others are operator packages
            default:
                bitsRead++;
                if (!bitStream.ReadBit())
                {
                    var length = ReadInt(bitStream, 15);
                    bitsRead += 15;
                    var before = bitsRead;
                    while (bitsRead < before + length)
                    {
                        bitsRead += ReadPacket(bitStream, packet.SubPackets);
                    }
                }
                else
                {
                    var count = ReadInt(bitStream, 11);
                    bitsRead += 11;
                    while (count > 0)
                    {
                        bitsRead += ReadPacket(bitStream, packet.SubPackets);
                        count--;
                    }

                }
                break;
        }

        return bitsRead;
    }

    /// <summary>
    /// Read an integer of length bits from the bit-stream, it does so by automatically shifting the bits.
    /// </summary>
    /// <param name="bitStream">BitStream</param>
    /// <param name="bits">int</param>
    /// <returns>int with the value</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    private static uint ReadInt(BitStream bitStream, int bits)
    {
        if (bits > 32)
        {
            throw new IndexOutOfRangeException();
        }

        uint result = 0;
        while (bits > 0)
        {
            result <<= 1;
            var bitValue = (uint)bitStream.ReadBit().AsInt();
            result |= bitValue;
            bits--;
        }
        return result;
    }
}


public enum PacketTypes
{
    Sum,
    Product,
    Minimum,
    Maximum,
    Literal,
    GreaterThan,
    LessThan,
    EqualTo
}

/// <summary>
/// The packet which encapsulates the information from the bit stream
/// </summary>
public class Packet
{
    private readonly List<Packet> _packets = new();

    public Packet(uint version, PacketTypes packetType)
    {
        Version = version;
        PacketType = packetType;
    }
    public uint Version { get; }

    public PacketTypes PacketType { get; }

    public IList<Packet> SubPackets => _packets;

    public long Value { get; set; }
}