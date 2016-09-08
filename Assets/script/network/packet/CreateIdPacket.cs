using UnityEngine;
using System.Collections;
using System;

public class CreateIdPacket : IPacket<CreateIdData>
{
    CreateIdData m_data;

    public CreateIdPacket(CreateIdData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public CreateIdPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        CreateIdSerializer serializer = new CreateIdSerializer();
        serializer.SetDeserializedData(data);
        m_data = new CreateIdData();
        serializer.Deserialize(ref m_data); 
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        CreateIdSerializer serializer = new CreateIdSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public CreateIdData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.CreateId;
    }
}
