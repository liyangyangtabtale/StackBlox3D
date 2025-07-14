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
        
        // 记录旋转前的角度
        float previousAngle = currentAngle;
        
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
            float newTargetAngle = nearest * anglePerCell;
            
            // 先判断是否可以旋转到目标角度
            if (CanRotateToAngle(newTargetAngle))
            {
                targetAngle = newTargetAngle;
            }
            else
            {
                // 如果不能旋转到目标角度，保持当前角度
                targetAngle = currentAngle;
            }
        }
        else if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 currentTouchPos = Input.mousePosition;
            Vector2 delta = currentTouchPos - lastTouchPos;
            float proposedAngle = currentAngle + delta.x * sensitivity;
            
            // 先判断是否可以旋转到建议角度
            if (CanRotateToAngle(proposedAngle))
            {
                // 只有在确认可以旋转时才应用新角度
                currentAngle = proposedAngle;
            }
            
            // 无论是否旋转都更新lastTouchPos，避免积累
            lastTouchPos = currentTouchPos;
        }

        // 平滑插值到目标角度
        if (!isDragging)
        {
            float lerpedAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * 10f);
            
            // 先判断插值角度是否安全
            if (CanRotateToAngle(lerpedAngle))
            {
                currentAngle = lerpedAngle;
            }
        }
        
        // 如果角度发生变化，通知BlockController更新位置
        if (Mathf.Abs(currentAngle - previousAngle) > 0.01f)
        {
            if (BlockController.Instance != null)
            {
                BlockController.Instance.OnCylinderRotated();
            }
        }
        
        transform.localRotation = Quaternion.Euler(0, -currentAngle, 0);
    }

    // 检测是否可以安全旋转到指定角度
    private bool CanRotateToAngle(float angle)
    {
        // 如果游戏未激活或没有下落方块，允许旋转
        if (BlockController.Instance == null || !BlockController.Instance.isActive)
            return true;
        
        // 获取当前下落方块的信息
        var currentTetromino = BlockController.Instance.GetCurrentTetromino();
        if (currentTetromino == null) return true;
        
        // 计算在给定角度下的ring偏移
        float anglePerCell = 360f / grid.ringCount;
        int ringOffset = Mathf.RoundToInt(angle / anglePerCell) % grid.ringCount;
        if (ringOffset < 0) ringOffset += grid.ringCount;
        
        // 计算考虑旋转后的实际ring位置
        int actualRing = (currentTetromino.ring - ringOffset) % grid.ringCount;
        if (actualRing < 0) actualRing += grid.ringCount;
        
        // 检测在新位置是否会发生碰撞
        return BlockController.Instance.CanPlace(currentTetromino.layer, actualRing, currentTetromino.rotation);
    }

    // 获取当前环向偏移（0~ringCount-1）
    public int GetRingOffset()
    {
        float anglePerCell = 360f / grid.ringCount;
        int offset = Mathf.RoundToInt(currentAngle / anglePerCell) % grid.ringCount;
        if (offset < 0) offset += grid.ringCount;
        return offset;
    }
} 