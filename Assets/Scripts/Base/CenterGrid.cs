using UnityEngine;

public class CenterGrid : MonoBehaviour
{
    public Camera mainCamera;
    public Transform gridTransform; // Transform của Grid
    public Vector2 gridSize; // Kích thước của Grid (width, height)

    void Start()
    {
        // Lấy kích thước Grid từ GridManager
        CenterGridInCamera();
        EventDispatcher.Instance.RegisterListener(EventID.OnUpdateCenterGrid,param=>CenterGridInCamera());
    }

    void CenterGridInCamera()
    {
        gridSize.x = GridManager.Instance.levelData.sizeGridX * GridManager.Instance.tileSize;
        gridSize.y = GridManager.Instance.levelData.sizeGridY * GridManager.Instance.tileSize;
        
        // Lấy kích thước của camera trong không gian thế giới
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Tính toán tỷ lệ khung hình của Grid
        float gridAspect = gridSize.x / gridSize.y;
        float cameraAspect = mainCamera.aspect;

        // Điều chỉnh kích thước camera để khớp với Grid
        if (cameraAspect >= gridAspect)
        {
            // Camera có tỷ lệ khung hình rộng hơn hoặc bằng Grid, điều chỉnh kích thước theo chiều cao
            mainCamera.orthographicSize = (gridSize.y / 2f)+0.5f;
        }
        else
        {
            // Camera có tỷ lệ khung hình hẹp hơn, điều chỉnh kích thước theo chiều rộng
            mainCamera.orthographicSize = (gridSize.x / (2f * cameraAspect))+0.5f;
        }

        // Tính toán vị trí trung tâm của camera
        Vector3 cameraCenter = new Vector3(gridSize.x / 2f, gridSize.y / 2f, mainCamera.transform.position.z);

        // Căn chỉnh Grid sao cho giữa camera

        // Đặt vị trí của camera sao cho nó nằm chính giữa Grid
        mainCamera.transform.position = new Vector3((gridSize.x / 2f) - 0.5f, (gridSize.y / 2f)-0.5f, mainCamera.transform.position.z);
    }
}
