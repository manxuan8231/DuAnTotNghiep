using UnityEngine;

public class Tornado : MonoBehaviour
{
    public GameObject[] objects; // Các vật thể cần xoay (5 vật thể)
    public float radius = 5f;    // Bán kính vòng tròn
    public float speed = 50f;    // Tốc độ xoay

    private Vector3 centerPoint; // Tâm của vòng tròn

    void Start()
    {
        if (objects.Length == 0)
        {
            Debug.LogError("Vui lòng thêm các vật thể vào danh sách objects.");
            return;
        }

        // Lấy tâm của vòng tròn (vị trí của object này)
        centerPoint = transform.position;

        // Đặt vị trí ban đầu của các vật thể
        float angleStep = 360f / objects.Length; // Góc giữa các vật thể
        for (int i = 0; i < objects.Length; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep; // Chuyển sang radian
            float x = centerPoint.x + Mathf.Cos(angle) * radius;
            float z = centerPoint.z + Mathf.Sin(angle) * radius;
            objects[i].transform.position = new Vector3(x, centerPoint.y, z);
        }
    }

    void Update()
    {
        // Xoay các vật thể xung quanh tâm
        transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);

        // Xoay từng vật thể đối với tâm của chính nó (nếu muốn)
        foreach (GameObject obj in objects)
        {
            obj.transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.Self);
        }
    }
}
