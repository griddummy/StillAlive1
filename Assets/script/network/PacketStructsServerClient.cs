
public enum ClientPacketId
{
    CreateId = 1,           // 가입
    DeleteId,               // 탈퇴
    Login,                  // 로그인
    RequestRoomList,        // 방 목록 조회
    CreateRoom,             // 방 생성
    EnterRoom,              // 방 입장
    LeaveRoom,              // 방 퇴장
    StartGame,              // 게임시작
    Logout,                 // 로그아웃
    ProgramExit             // 클라이언트 종료
}

public enum ServerPacketId
{
    CreateIdResult = 1,     // 가입 결과
    DeleteResult,           // 아이디 삭제
    LoginResult,            // 로그인 결과
    GetRoomListResult,      // 방 목록 결과
    CreateRoomResult,       // 방 생성 결과
    EnterRoomResult         // 방 입장 결과
}
