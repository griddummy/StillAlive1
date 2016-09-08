using System.Collections.Generic;
using System.IO;
using System;
using System.Net.Sockets;
public class PacketQueue
{

    struct PacketInfo
    {
        public Socket sockSource; // 이 패킷을 보낸 소켓
        public int offset;          // 읽어야 되는 위치
        public int size;            // 마지막위치 + 1 
    };

    // 데이터를 보존 할 버퍼
    private MemoryStream m_streamBuffer;

    // 패킷 정보 관리 리스트
    private Queue<PacketInfo> m_offsetQueue;

    private int m_offest; // 메모리 배치 오프셋

    public PacketQueue() // 생성자
    {
        m_offest = 0;
        m_streamBuffer = new MemoryStream();
        m_offsetQueue = new Queue<PacketInfo>();
    }

    public int Count
    {
        get
        {
            return m_offsetQueue.Count;
        }
    }

    public int Enqueue(Socket sockSource, byte[] data, int size)
    {
        PacketInfo info = new PacketInfo();

        // 패킷 정보 작성
        info.sockSource = sockSource;
        info.offset = m_offest;
        info.size = size;

        m_offsetQueue.Enqueue(info); // 패킷 정보 저장

        // 패킷의 바이트데이터 쓰기
        m_streamBuffer.Position = m_offest;
        m_streamBuffer.Write(data, 0, size);
        m_offest += size;

        return size;
    }

    public int Dequeue(out Socket sourceSock, ref byte[] buffer, int size)
    {
        sourceSock = null;

        if (m_offsetQueue.Count <= 0)
        {
            return -1;
        }

        int recvSize = 0;
        PacketInfo info = m_offsetQueue.Peek();
        sourceSock = info.sockSource;

        // 버퍼에서 데이터 가져오기            
        m_streamBuffer.Position = info.offset;
        recvSize = m_streamBuffer.Read(buffer, 0, info.size);


        // 큐 데이터를 꺼냈으면 맨 앞 데이터는 삭제
        if (recvSize > 0)
        {
            m_offsetQueue.Dequeue();
        }

        // 모든 큐 데이터를 꺼냇으면 스트림 정리해서 메모리 절약한다.
        if (m_offsetQueue.Count == 0)
        {
            Clear();
            m_offest = 0;
        }
        return recvSize;
    }

    public void Clear()
    {
        m_streamBuffer.SetLength(0);
    }
}