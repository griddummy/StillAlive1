using UnityEngine;
using System.Collections;

public interface IPacket<T>  {
    int GetPacketId(); // 패킷 ID 얻기
    T GetData(); // 데이터 얻기
    byte[] GetPacketData(); // 바이너리 데이터 얻기
}

