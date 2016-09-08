using UnityEngine;

// 무기 정보 클래스
[System.Serializable]
public class Weapon  {
        
    // 타입  주무기 보조무기
    public enum Type { Primary, Secondary }
    public enum FireMode { Auto, Semi, Single }

    public string name; // 무기 이름
    public Type type;// 무장 타입 - 주무기, 보조무기
    public FilterMode fireMode;  // 사격 가능한 모드     
    public int damage;// 데미지
    public float fireRate;// 연사속도    
    //public float Accuracy// 명중률
    //public float control// 반동
    public int magazineSize;// 탄약 수

}
