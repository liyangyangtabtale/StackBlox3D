using UnityEngine;

public class CylinderRotator : MonoBehaviour
{
    public CylindricalGrid grid;
    public float sensitivity = 0.3f;
    public bool canRotate = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private bool isDragging = false;
    private Vector2 lastTouchPos;

    void Update()
    {
        if (!canRotate) return;
        // 触摸滑动
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            // 吸附到最近格子
            float anglePerCell = 360f / grid.ringCount;
            int nearest = Mathf.RoundToInt(currentAngle / anglePerCell) % grid.ringCount;
            if (nearest < 0) nearest += grid.ringCount;
            targetAngle = nearest * anglePerCell;
        }
        else if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
            lastTouchPos = Input.mousePosition;
            currentAngle += delta.x * sensitivity;
        }

        // 平滑插值到目标角度
        if (!isDragging)
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * 10f);

        // 应用旋转
        if (grid != null)
            grid.transform.localRotation = Quaternion.Euler(0, -currentAngle, 0);
    }

    // 获取当前环向偏移（0~ringCount-1）
    public int GetRingOffset()
    {
        float anglePerCell = 360f / grid.ringCount;
        int offset = Mathf.RoundToInt(currentAngle / anglePerCell) % grid.ringCount;
        if (offset < 0) offset += grid.ringCount;
        return offset;
    }
    
    // 获取当前角度
    public float GetCurrentAngle()
    {
        return currentAngle;
    }
} 