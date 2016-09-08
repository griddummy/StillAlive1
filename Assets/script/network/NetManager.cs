using UnityEngine;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Net.Sockets;

public class NetManager : MonoBehaviour {

    TcpClient m_client;                 // 서버 연결 소켓
    TcpServer m_host;                   // 호스트 소켓
    TcpClient m_guest;                  // 게스트 소켓

    PacketController m_pcFromServer;
    PacketController m_pcFromP2P;

    byte[] m_recvBuffer;

    const int BufferSize = 2048;

    public delegate void RecvNotifier(Socket sock, byte[] data); // 누가, 어떤 데이타를 보냇는지    
    private Dictionary<int, RecvNotifier> m_notiServer = new Dictionary<int, RecvNotifier>();
    private Dictionary<int, RecvNotifier> m_notiP2P = new Dictionary<int, RecvNotifier>();

    public delegate void OnDisconnectGuest(Socket sock); // 클라이언트 끊어짐
    
    public string GameServerIP;
    public int GameServerPort = 23579;
    public int HostPort = 23578;
    
    void Awake()
    {
        m_recvBuffer = new byte[BufferSize];
        m_pcFromServer = new PacketController();
        m_pcFromP2P = new PacketController();

        // GameServer Connection Context
        m_client = new TcpClient();        
        m_client.OnReceived += m_pcFromServer.Receive;        

        // Host Connection Context
        m_host = new TcpServer();
        m_host.OnReceived += m_pcFromP2P.Receive;

        // Guest Connection Context
        m_guest = new TcpClient();
        m_guest.OnReceived += m_pcFromP2P.Receive;
    }

    void Start()
    {
        StartHostServer();
        ConnectToGameServer();
    }

    void Update()
    {
        // 서버로부터 받기
        if(m_pcFromServer.GetPacketCount() > 0)
        {
            PacketQueue queue = m_pcFromServer.GetBuffer();
            Socket sock;
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                queue.Dequeue(out sock, ref m_recvBuffer, m_recvBuffer.Length);
                ReceivePacket(m_notiServer, sock, m_recvBuffer);
            }
        }

        // 게스트(또는 호스트)로부터 받기
        if (m_pcFromP2P.GetPacketCount() > 0)
        {
            PacketQueue queue = m_pcFromP2P.GetBuffer();
            Socket sock;
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                queue.Dequeue(out sock, ref m_recvBuffer, m_recvBuffer.Length);
                ReceivePacket(m_notiP2P, sock, m_recvBuffer);
            }
        }
    }   

    void OnApplicationQuit()
    {
        Debug.Log("NetManager::AllSocketClose");

        //ProgramExitPacket packet = new ProgramExitPacket();
        //SendToServer(packet);
        
        m_client.Disconnect();
        m_host.ServerClose();
        m_guest.Disconnect();
    }

    public void StartHostServer()
    {
        m_host.Start(HostPort);
    }

    public bool ConnectToGameServer() // 서버로 연결
    {
        return m_client.Connect(GameServerIP, GameServerPort);
    }

    public bool ConnectToHost(string ip) // 호스트에게 연결
    {
        return m_guest.Connect(ip, HostPort);
    }

    public void DisconnectMyGuestSocket() // (내가 게스트일때) 호스트와 연결을 끊는다.
    {
        m_guest.Disconnect();
    }

    public void DisconnectGuestSocket(Socket other) //(내가 호스트일때) 특정 게스트와 연결을 끊는다.
    {
        m_host.DisconnectClient(other);
    }

    public void CloseHostSocket() // (내가 호스트 일 떄) 모든 게스트와 연결을 끊는다.
    {
        m_host.DisconnectAll();
    }

    private void Receive(PacketQueue queue, Socket sock, Dictionary<int, RecvNotifier> noti) // 서버나 호스트에게 받은 큐
    {        
        //int Count = queue.Count;
        
        //for( int i = 0; i < Count; i++)
        //{
        //    int recvSize = 0;
        //    recvSize = queue.Dequeue(ref m_recvBuffer, m_recvBuffer.Length);                
            
        //    if (recvSize > 0)
        //    {
        //        byte[] msg = new byte[recvSize];

        //        Array.Copy(m_recvBuffer, msg, recvSize);
        //        ReceivePacket(noti, sock, msg); // 서버나 호스트로 부터 받은건 0번
        //    }
        //}
    }

    private void ReceiveFromGuest()
    {
        //int Count = m_recvQueueFromGuest.Count;

        //for (int i = 0; i < Count; i++)
        //{
        //    int recvSize = 0;
        //    recvSize = m_recvQueueFromGuest.Dequeue(ref m_recvBuffer, m_recvBuffer.Length);
        //    Socket sock;
        //    lock (m_objLockGuestPacketQueue)
        //    {                
        //         sock = indexGuestQueue.Dequeue();
        //    }            
        //    if (recvSize > 0)
        //    {
        //        byte[] msg = new byte[recvSize];
        //        Array.Copy(m_recvBuffer, msg, recvSize);
        //        ReceivePacket(m_notiP2P, sock, msg);
        //    }
        //}
    }
    private byte[] CreatePacket<T>(IPacket<T> packet) // 패킷 만드는 메서드
    {
        byte[] packetData = packet.GetPacketData(); // 패킷의 데이터를 바이트화

        // 헤더 생성
        PacketHeader header = new PacketHeader();
        HeaderSerializer serializer = new HeaderSerializer();

        header.length = (short)packetData.Length; // 패킷 데이터의 길이를 헤더에 입력
        header.id = (byte)packet.GetPacketId(); // 패킷 데이터에서 ID를 가져와 헤더에 입력
        //Debug.Log("패킷 전송 - id : " + header.id.ToString() + " length :" + header.length);
        byte[] headerData = null;
        if (serializer.Serialize(header) == false)
        {
            return null;
        }

        headerData = serializer.GetSerializedData(); // 헤더 데이터를 패킷 바이트로 변환


        byte[] data = new byte[headerData.Length + header.length]; // 최종 패킷의 길이 = 헤더패킷길이+내용패킷길이

        // 헤더와 내용을 하나의 배열로 복사
        int headerSize = Marshal.SizeOf(header.id) + Marshal.SizeOf(header.length);
        Buffer.BlockCopy(headerData, 0, data, 0, headerSize);
        Buffer.BlockCopy(packetData, 0, data, headerSize, packetData.Length);
        return data;
    }
    public int SendToHost<T>(IPacket<T> packet) // 패킷에 헤더를 부여하고 송신하는 메서드
    {
        int sendSize = 0;
        byte[] data = CreatePacket(packet);
        if (data == null)
            return 0;
        //Debug.Log("SendToHost()::소켓에 패킷 Send" + data.Length);
        sendSize = m_guest.Send(data, data.Length);
        return sendSize;
    }
    public int SendToGuest<T>(Socket guest, IPacket<T> packet) // 패킷에 헤더를 부여하고 송신하는 메서드
    {
        int sendSize = 0;
        byte[] data = CreatePacket(packet);
        if (data == null)
            return 0;
        sendSize = m_host.Send(guest, data, data.Length);
        
        return sendSize;
    }
    public int SendToAllGuest<T>(IPacket<T> packet) // 패킷에 헤더를 부여하고 송신하는 메서드
    {
        int sendSize = 0;
        byte[] data = CreatePacket(packet);
        if (data == null)
        {
            Debug.Log("SendToAllGuest() - 잘못된 패킷" + packet.GetPacketId().ToString());
            return 0;
        }            

        //전송
        m_host.SendAll( data, data.Length);        
        return sendSize;
    }
    public int SendToAllGuest<T>(Socket excludeClient, IPacket<T> packet) // 패킷에 헤더를 부여하고 송신하는 메서드
    {
        int sendSize = 0;
        byte[] data = CreatePacket(packet);
        if (data == null)
            return 0;

        //전송
        m_host.SendAll(excludeClient, data, data.Length);
        return sendSize;
    }
    public int SendToServer<T>(IPacket<T> packet) // 패킷에 헤더를 부여하고 송신하는 메서드
    {
        int sendSize = 0;
        byte[] data = CreatePacket(packet);
        if (data == null)
            return 0;

        //전송
        sendSize = m_client.Send(data, data.Length);
        return sendSize;
    }

    public void RegisterReceiveNotificationServer( int packetID , RecvNotifier notifier)
    {
        m_notiServer.Add(packetID, notifier);
    }

    public void UnRegisterReceiveNotificationServer(int packetID)
    {
        m_notiServer.Remove(packetID);
    }
    public void RegisterReceiveNotificationP2P(int packetID, RecvNotifier notifier)
    {
        m_notiP2P.Add(packetID, notifier);
    }
    public void UnRegisterReceiveNotificationP2P(int packetID)
    {
        m_notiP2P.Remove(packetID);
    }
    private bool getPacketContent(byte[] data, out int id, out byte[] outContent)
    {
        PacketHeader header = new PacketHeader();
        HeaderSerializer serializer = new HeaderSerializer();

        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref header);

        
        int headerSize = 3; // 헤더사이즈 short + byte = 3
        int packetContentSize = data.Length - headerSize;

        byte[] packetContent = null;
        if (packetContentSize > 0) //헤더만 있는 패킷을 대비해서 예외처리, 데이터가 있는 패킷만 데이터를 만든다
        {
            packetContent = new byte[packetContentSize];
            Buffer.BlockCopy(data, headerSize, packetContent, 0, packetContent.Length);
        }
        else
        {
            id = header.id;
            outContent = null;
            return false;
        }
        //Debug.Log("받은 패킷 - id : " + header.id + " dataLength : " + packetData.Length);
        id = header.id;
        outContent = packetContent;
        return true;
    }
    private void ReceivePacket(Dictionary<int,RecvNotifier> noti, Socket sock , byte[] data)
    {
        byte[] packetContent;
        int packetId;
        getPacketContent(data, out packetId, out packetContent);

        //try
        //{
        //    Debug.Log("ReceivePacket::" + sock.RemoteEndPoint.ToString() + "패킷id:" + packetId + " 길이:" + packetData.Length);
        //}
        //catch
        //{
        //    Debug.Log("ReceivePacket::" + "패킷id:" + packetId + " 길이:" + data.Length);
        //}
        
        RecvNotifier recvNoti;
        if (noti.TryGetValue(packetId, out recvNoti))
        {
            recvNoti(sock, packetContent);
        }
        else
        {
            Debug.Log("NetManager::ReceivePacket() - 존재하지 않는 타입 패킷 :"+ packetId + " " + sock.RemoteEndPoint.ToString());
        }        
    }
}
