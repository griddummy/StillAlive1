using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;

// 받은 바이트 데이터를 큐에 넣고
// 큐에 있는 데이터를 패킷으로 반환해주는 기능을 가짐.

public class PacketController
{
    PacketQueue m_mainQueue;            // 주소 공간만 가지는 메인 큐
    PacketQueue m_subQueue;             // 주소 공간만 가지는 서브 큐
    MemoryStream m_tempBuffer;          // 임시로 받기위한 버퍼 스트림

    byte[] m_arrEnqueueBuffer; // 큐를 위한 버퍼
    byte[] m_arrTemp;
    int m_packetSize;

    public const int MaxPacketSize = 2048;

    object m_lockMainQueue;

    public PacketController()
    {
        m_lockMainQueue = new object();
        m_mainQueue = new PacketQueue();
        m_subQueue = new PacketQueue();
        m_tempBuffer = new MemoryStream();

        m_arrEnqueueBuffer = new byte[MaxPacketSize];
        m_arrTemp = new byte[MaxPacketSize];

        m_packetSize = 0;
    }

    public void Receive(Socket socket, byte[] data, int size) // 받기
    {
        //CheckPacket(socket, data, 0, size);
        lock (m_lockMainQueue) // 비동기 Receive에 의한 스레드와 메인스레드간의 동기화를 위해 Lock을 걸어준다.
        {
            m_mainQueue.Enqueue(socket, data, size); // 큐에 집어 넣는다.
        }
    }
    private void CheckPacket(Socket socket, byte[] data, int index, int size)
    {
        if (size <= 0)
            return;

        if (m_packetSize == 0) // 패킷 사이즈가 없으면
        {
            int bodySize = 0;
            int bufferLength = (int)m_tempBuffer.Length;

            if (bufferLength >= 2) // 임시버퍼에 패킷 사이즈 정보가 있으면
            {
                long tempPosition = m_tempBuffer.Position;
                m_tempBuffer.Position = 0;
                m_tempBuffer.Read(m_arrTemp, 0, 2);
                m_tempBuffer.Position = tempPosition;
                bodySize = BitConverter.ToInt16(m_arrTemp, 0);
            }
            else if (bufferLength <= 0) // 아예 없으면
            {
                if (size < 2)
                {
                    m_tempBuffer.Write(data, index, size);
                    return;
                }
                else
                {
                    bodySize = BitConverter.ToInt16(data, index); // 패킷 데이터부분의 사이즈 + 헤더 사이즈(short + byte = 3)

                }
            }
            else // 모자르면
            {

                long tempPosition = m_tempBuffer.Position;
                m_tempBuffer.Position = 0;
                byte big = (byte)m_tempBuffer.ReadByte();
                m_tempBuffer.Position = tempPosition;
                m_arrTemp[0] = big;
                m_arrTemp[1] = data[index];
                bodySize = BitConverter.ToInt16(m_arrTemp, 0);
            }

            // 패킷사이즈를 구한다.
            m_packetSize = bodySize + 3;
        }

        // 패킷이 완성되는지 헤더 검사
        int currentBytes = (int)m_tempBuffer.Length + size; // 이전 바이트사이즈 + 현재 바이트사이즈 = 총 바이트 크기

        if (currentBytes > m_packetSize) // 패킷사이즈보다 크면, 완성된 패킷은 큐에 넣고, 나머지는 임시버퍼에 넣는다.
        {
            int writeLength = m_packetSize - (int)m_tempBuffer.Length; // 받은데이터중에 임시버퍼에 넣을 데이터 크기를 구한다.
            int remain = size - writeLength; // 받은데이터를  임시버퍼에 넣고 남는 크기를 구한다.

            m_tempBuffer.Write(data, index, writeLength); // 임시 버퍼에 일부 데이터만 추가한다.

            Enqueue(socket, m_packetSize); // 완성된 패킷 데이터를 실제 버퍼로 옮긴다.

            CheckPacket(socket, data, index + writeLength, remain); // 남은 데이터 부분을 재 검사 한다.(재귀)
        }
        else if (currentBytes < m_packetSize) // 패킷 사이즈보다 작으면, 임시버퍼에 붙이고 끝.
        {
            m_tempBuffer.Write(data, index, size);

        }
        else // 패킷사이즈와 같으면, 완성된 패킷을 큐에 넣는다. 임시버퍼를 비운다.
        {
            // 받은 데이터를 임시버퍼에 붙인다.
            m_tempBuffer.Write(data, index, size);

            Enqueue(socket, m_packetSize); // 완성된 패킷 데이터를 실제 버퍼로 옮긴다.
        }
    }

    private void Enqueue(Socket sourceSock, int packetSize) // 임시버퍼에 있는 데이터를 실제 버퍼로 옮긴다.
    {
        m_tempBuffer.Position = 0; // 스트림 버퍼에서 읽을 위치를 초기화한다.

        // 완성된 패킷 데이터를 버퍼에서 읽는다.
        int readSize = m_tempBuffer.Read(m_arrEnqueueBuffer, 0, m_packetSize);

        lock (m_lockMainQueue) // 비동기 Receive에 의한 스레드와 메인스레드간의 동기화를 위해 Lock을 걸어준다.
        {
            m_mainQueue.Enqueue(sourceSock, m_arrEnqueueBuffer, readSize); // 큐에 집어 넣는다.
        }

        m_tempBuffer.SetLength(0);  // 임시버퍼를 초기화한다.
        m_packetSize = 0; // 헤더 초기화      
    }

    public PacketQueue GetBuffer() // 버퍼를 스위칭 한다(더블버퍼링)
    {
        PacketQueue queue = m_mainQueue;
        lock (m_lockMainQueue)
        {
            m_mainQueue = m_subQueue;
        }
        m_subQueue = queue;
        return m_subQueue;
    }
    public int GetPacketCount()
    {
        return m_mainQueue.Count;
    }
}

