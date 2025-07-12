# 固定摄像机系统

## 概述

CameraController3D脚本已经重新设计为固定摄像机系统，摄像机不再进行旋转移动控制，而是固定在指定位置。系统提供了丰富的参数修改方法和变量，可以灵活配置摄像机的外观和行为。

## 主要特性

### 1. 固定摄像机模式
- 摄像机固定在指定位置，不进行旋转移动
- 支持环形模式和矩形模式的不同配置
- 可以随时切换固定摄像机模式

### 2. 丰富的参数配置
- 视野角度、裁剪面、背景颜色等基本参数
- 环形模式和矩形模式的专用参数
- 雾效、景深、运动模糊等视觉效果参数

### 3. 完整的参数修改方法
- 提供所有参数的修改方法
- 支持运行时动态调整
- 包含参数验证和边界检查

## 配置参数

### 固定摄像机设置
```csharp
[Header("Fixed Camera Settings")]
public bool enableFixedCamera = true;        // 是否启用固定摄像机模式
public Vector3 fixedPosition = new Vector3(0, 15, -20); // 固定摄像机位置
public Vector3 fixedLookAt = Vector3.zero;   // 固定摄像机朝向目标
public float fieldOfView = 60f;              // 摄像机视野角度
```

### 摄像机参数
```csharp
[Header("Camera Parameters")]
public float nearClipPlane = 0.3f;           // 近裁剪面
public float farClipPlane = 1000f;           // 远裁剪面
public bool enableFog = false;               // 是否启用雾效
public Color backgroundColor = Color.black;  // 背景颜色
```

### 环形模式设置
```csharp
[Header("Ring Mode Settings")]
public float ringCameraDistance = 20f;       // 环形模式摄像机距离
public float ringCameraHeight = 15f;         // 环形模式摄像机高度
public float ringCameraAngle = 30f;          // 环形模式摄像机俯视角度
```

### 矩形模式设置
```csharp
[Header("Rectangular Mode Settings")]
public Vector3 rectangularOffset = new Vector3(0, 10, -15); // 矩形模式摄像机偏移
public float rectangularCameraAngle = 45f;   // 矩形模式摄像机俯视角度
```

### 摄像机效果
```csharp
[Header("Camera Effects")]
public bool enableDepthOfField = false;      // 是否启用景深效果
public float depthOfFieldDistance = 10f;     // 景深距离
public float depthOfFieldBlur = 2f;          // 景深模糊程度
public bool enableMotionBlur = false;        // 是否启用运动模糊
public float motionBlurStrength = 0.5f;      // 运动模糊强度
```

## 参数修改方法

### 基本参数修改
```csharp
// 设置固定摄像机位置
public void SetFixedPosition(Vector3 position)

// 设置固定摄像机朝向目标
public void SetFixedLookAt(Vector3 target)

// 设置摄像机视野角度
public void SetFieldOfView(float fov)

// 设置近裁剪面
public void SetNearClipPlane(float near)

// 设置远裁剪面
public void SetFarClipPlane(float far)

// 设置背景颜色
public void SetBackgroundColor(Color color)
```

### 环形模式参数修改
```csharp
// 设置环形模式摄像机距离
public void SetRingCameraDistance(float distance)

// 设置环形模式摄像机高度
public void SetRingCameraHeight(float height)

// 设置环形模式摄像机俯视角度
public void SetRingCameraAngle(float angle)
```

### 矩形模式参数修改
```csharp
// 设置矩形模式摄像机偏移
public void SetRectangularOffset(Vector3 offset)

// 设置矩形模式摄像机俯视角度
public void SetRectangularCameraAngle(float angle)
```

### 模式切换和重置
```csharp
// 切换固定摄像机模式
public void ToggleFixedCamera()

// 切换雾效
public void ToggleFog()

// 切换景深效果
public void ToggleDepthOfField()

// 切换运动模糊
public void ToggleMotionBlur()

// 重置摄像机到默认设置
public void ResetToDefaults()
```

### 信息获取
```csharp
// 获取当前摄像机信息
public string GetCameraInfo()
```

## 使用方法

### 基本设置
```csharp
// 获取摄像机控制器
CameraController3D cameraController = FindObjectOfType<CameraController3D>();

// 设置基本参数
cameraController.SetFieldOfView(75f);
cameraController.SetBackgroundColor(Color.blue);
cameraController.SetNearClipPlane(0.1f);
cameraController.SetFarClipPlane(500f);

// 设置环形模式参数
cameraController.SetRingCameraDistance(25f);
cameraController.SetRingCameraHeight(18f);
cameraController.SetRingCameraAngle(35f);

// 设置矩形模式参数
cameraController.SetRectangularOffset(new Vector3(0, 15, -20));
cameraController.SetRectangularCameraAngle(50f);
```

### 运行时调整
```csharp
// 切换雾效
cameraController.ToggleFog();

// 切换固定摄像机模式
cameraController.ToggleFixedCamera();

// 重置到默认设置
cameraController.ResetToDefaults();
```

### 获取信息
```csharp
// 获取摄像机信息
string info = cameraController.GetCameraInfo();
Debug.Log(info);
```

## 测试功能

### FixedCameraTester

FixedCameraTester提供了完整的摄像机测试功能：

#### 测试按键
- **C键**：运行完整的摄像机测试
- **F键**：切换固定摄像机模式
- **R键**：重置摄像机设置
- **1-9键**：测试不同参数
- **0键**：切换雾效

#### 测试参数
- 视野角度：30°、60°、90°
- 背景颜色：红色、绿色、蓝色
- 环形模式参数：距离、高度、角度
- 雾效开关

#### GUI界面
游戏运行时会在屏幕右侧显示测试界面，包含：
- 当前摄像机状态
- 参数显示
- 测试按钮
- 控制说明

## 摄像机位置计算

### 环形模式
```csharp
// 摄像机位置计算
Vector3 position = new Vector3(0, ringCameraHeight, -ringCameraDistance);
transform.position = position;

// 朝向目标
Vector3 lookAtTarget = new Vector3(0, gameGrid.gridHeight / 2f, 0);
transform.LookAt(lookAtTarget);

// 应用俯视角度
Vector3 eulerAngles = transform.eulerAngles;
eulerAngles.x = ringCameraAngle;
transform.eulerAngles = eulerAngles;
```

### 矩形模式
```csharp
// 目标位置
Vector3 targetPosition = new Vector3(
    gameGrid.gridWidth / 2f - 0.5f,
    gameGrid.gridHeight / 2f - 0.5f,
    0
);

// 摄像机位置
Vector3 position = targetPosition + rectangularOffset;
transform.position = position;

// 朝向目标
transform.LookAt(targetPosition);

// 应用俯视角度
Vector3 eulerAngles = transform.eulerAngles;
eulerAngles.x = rectangularCameraAngle;
transform.eulerAngles = eulerAngles;
```

## 视觉效果

### 雾效设置
```csharp
// 启用雾效
RenderSettings.fog = true;
RenderSettings.fogColor = backgroundColor;
RenderSettings.fogMode = FogMode.Exponential;
RenderSettings.fogDensity = 0.01f;
```

### 景深效果
景深效果可以通过后处理效果实现，当前版本提供了切换接口。

### 运动模糊
运动模糊效果可以通过后处理效果实现，当前版本提供了切换接口。

## 调试和可视化

### Gizmos绘制
在Scene视图中选中摄像机对象时，会显示：
- 摄像机位置（黄色球体）
- 摄像机朝向（青色线条）
- 视野范围（绿色矩形）

### 调试信息
```csharp
// 获取详细的摄像机信息
string info = cameraController.GetCameraInfo();
// 包含位置、旋转、参数等所有信息
```

## 性能优化

### 参数验证
所有参数修改方法都包含边界检查：
- 视野角度：1° - 179°
- 裁剪面：近裁剪面 > 0.01f，远裁剪面 > 近裁剪面
- 俯视角度：0° - 90°

### 更新优化
- 只在参数改变时更新摄像机
- 避免不必要的计算
- 使用缓存减少重复计算

## 扩展功能

### 可能的改进
1. **后处理效果**：实现景深和运动模糊效果
2. **动态LOD**：根据距离调整渲染细节
3. **多摄像机支持**：支持多个摄像机切换
4. **摄像机动画**：支持摄像机位置动画
5. **自定义着色器**：支持自定义后处理着色器

### 自定义效果
可以通过扩展CameraController3D来添加自定义效果：
```csharp
// 添加自定义效果
public void AddCustomEffect(CustomEffect effect)
{
    // 实现自定义效果
}
```

## 故障排除

### 常见问题

1. **摄像机不显示**
   - 检查 `enableFixedCamera` 是否为 true
   - 验证摄像机位置是否在视野范围内
   - 检查裁剪面设置

2. **参数修改无效**
   - 确保摄像机组件存在
   - 检查参数是否在有效范围内
   - 验证模式设置是否正确

3. **性能问题**
   - 减少参数更新频率
   - 优化视野角度和裁剪面
   - 禁用不必要的视觉效果

### 调试步骤
1. 使用 `GetCameraInfo()` 获取当前状态
2. 检查Console中的错误信息
3. 使用FixedCameraTester进行测试
4. 验证所有参数设置

## 总结

新的固定摄像机系统提供了：
- ✅ 固定的摄像机位置，无旋转移动
- ✅ 丰富的参数配置选项
- ✅ 完整的参数修改方法
- ✅ 环形和矩形模式支持
- ✅ 视觉效果控制
- ✅ 完整的测试工具
- ✅ 详细的调试信息
- ✅ 性能优化
- ✅ 扩展性支持 