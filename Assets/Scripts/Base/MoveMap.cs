using UnityEngine;

public class MoveMap : MonoBehaviour
{
    private Vector3 dragOrigin;
    public float moveSpeed = 0.25f; // Hệ số điều chỉnh tốc độ di chuyển

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lưu tọa độ chuột khi bắt đầu kéo
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragOrigin.z = transform.position.z; // Đảm bảo z không thay đổi
        }

        if (Input.GetMouseButton(0))
        {
            // Tính toán sự khác biệt và điều chỉnh tốc độ di chuyển
            Vector3 difference =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - dragOrigin;
            difference.z = 0; // Đảm bảo không thay đổi z

            // Nhân với hệ số tốc độ để điều chỉnh tốc độ di chuyển
            transform.position += difference * moveSpeed;

            // Cập nhật dragOrigin để tính toán sự thay đổi liên tục
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragOrigin.z = transform.position.z; // Đảm bảo z không thay đổi
        }
    }
}