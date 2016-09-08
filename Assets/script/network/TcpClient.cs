using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;

public class TcpClient
{
    class AsyncData
    {
        public Socket clientSock;
        public const int msgMaxLength = 2048;
        public byte[] msg = new byte[msgMaxLength];
        public int msgLength;
    }

    public delegate void OnReceivedEvent(Socket sender, byte[] msg, int size);
    public event OnReceivedEvent OnReceived;

    private Socket m_clientSock = null;
    private AsyncCallback asyncReceiveHeaderCallback;
    private AsyncCallback asyncReceiveContentCallback;
    private string m_strIP;
    private int m_port;
    const int SizeOfPacketSizeInHeader = 2; // 헤더에서 패킷의 사이즈를 나타내는 바이트 개수
    const int SizeOfOthersInHeader = 1;     // 헤더에서 사이즈를 나타내는것 외의 바이트 개수

    public Socket socket
    {
        get { return m_clientSock; }
    }

    public TcpClient()
    {
        asyncReceiveHeaderCallback = new AsyncCallback(HandleAsyncReceiveHeader);
        asyncReceiveContentCallback = new AsyncCallback(HandleAsyncReceiveContent);
    }

    public bool Connect(string ip, int port)
    {
        m_strIP = ip;
        m_port = port;

        if (m_clientSock != null)
        {
            if (m_clientSock.Connected)
                return false;
        }

        // connect server
        try
        {
            m_clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_clientSock.Connect(new IPEndPoint(IPAddress.Parse(m_strIP), m_port));
        }
        catch (SocketException e)
        {
            Debug.Log("TCPClient::Connect() : Connect Fail" + (int)e.SocketErrorCode);
            return false;
        }

        // begin receive
        AsyncData asyncData = new AsyncData();
        asyncData.clientSock = m_clientSock;

        if(!BeginReceiveHeader(asyncData, asyncData))
        {
            return false;
        }

        return true;
    }
    

    
    private bool BeginReceiveHeader(AsyncData asyncData, object state)
    {
        try
        {
            m_clientSock.BeginReceive(asyncData.msg, 0, SizeOfPacketSizeInHeader, SocketFlags.None, asyncReceiveHeaderCallback, asyncData);
            return true;
        }
        catch
        {
            Debug.Log("TCPClient::BeginReceiveHeader - 예외");
            Disconnect();
            return false;
        }
    }

    private bool BeginReceiveContent(AsyncData asyncData, int size, object state)
    {
        try
        {
            m_clientSock.BeginReceive(asyncData.msg, SizeOfPacketSizeInHeader, size + SizeOfOthersInHeader, SocketFlags.None, asyncReceiveContentCallback, asyncData);
            return true;
        }
        catch
        {
            Debug.Log("TCPClient::BeginReceiveContent - 예외");
            Disconnect();
            return false;
        }
    }

    private void HandleAsyncReceiveHeader(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;
        Socket clientSock = asyncData.clientSock;

        try
        {
            asyncData.msgLength = clientSock.EndReceive(asyncResult);
        }
        catch
        {
            Debug.Log("TCPClient::HandleAsyncReceive() : EndReceive - 예외 " + m_clientSock.RemoteEndPoint.ToString());
            Disconnect();
            return;
        }

        if (asyncData.msgLength < 2) // 다시 해더 받기
        {
            Debug.Log("TcpClient::헤더 오류 - 다시 헤더 받기");
            if (!BeginReceiveHeader(asyncData, asyncData))
            {
                return;
            }
        }

        short packetContentSize = BitConverter.ToInt16(asyncData.msg, 0);
        BeginReceiveContent(asyncData, packetContentSize, asyncData);
        
    }
    private void HandleAsyncReceiveContent(IAsyncResult asyncResult)
    {
        AsyncData asyncData = (AsyncData)asyncResult.AsyncState;
        Socket clientSock = asyncData.clientSock;

        try
        {
            asyncData.msgLength = clientSock.EndReceive(asyncResult);
        }
        catch
        {
            Debug.Log("TCPClient::HandleAsyncReceive() : EndReceive - 예외 " + m_clientSock.RemoteEndPoint.ToString());
            Disconnect();
            return;
        }
        if (OnReceived != null)
        {
            OnReceived(clientSock, asyncData.msg, SizeOfPacketSizeInHeader+asyncData.msgLength);
        }

        BeginReceiveHeader(asyncData, asyncData);
    }    

    public int Send(byte[] data, int size)
    {
        if (!m_clientSock.Connected)
        {
            Debug.Log("Send() : Send - 소켓이 연결되지 않음");
            return -1;
        }
        try
        {
            return m_clientSock.Send(data, size, SocketFlags.None);
        }
        catch
        {
            Debug.Log("Send() : Send - 예외");
        }
        return -1;
    }
    public void Disconnect()
    {
        if (m_clientSock == null)
            return;
        if (m_clientSock.Connected)
        {
            Debug.Log("TcpClient::Disconnect - Remote : " + m_clientSock.RemoteEndPoint.ToString());
            try
            {
                //m_clientSock.Disconnect(false);
                m_clientSock.Close();
            }
            catch
            {

            }
        }
    }
}
