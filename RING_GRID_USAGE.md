# 环形网格使用说明

## 快速开始

### 1. 自动设置（推荐）
1. 在场景中创建一个空的GameObject
2. 添加 `RingGridSetup` 组件
3. 运行游戏，系统会自动配置所有必要的组件

### 2. 手动设置
1. 创建GameGrid3D对象并配置参数
2. 为主相机添加CameraController3D组件
3. 创建GameManager3D对象
4. 添加其他必要的管理器组件

## 控制说明

### 游戏控制
- **WASD**：移动方块
- **空格**：旋转方块
- **Shift**：硬降（快速下降）
- **C**：保持方块
- **P**：暂停游戏

### 测试控制
- **T键**：运行网格测试
- **R键**：切换环形/矩形模式

### 界面控制
- 屏幕右上角有设置面板
- 可以快速切换模式和重置相机

## 参数配置

### 环形网格参数
```csharp
// 在GameGrid3D组件中设置
gridWidth = 20;        // 环形周长
gridHeight = 20;       // 高度
ringRadius = 10f;      // 环形半径
enableRingMode = true; // 启用环形模式
```

### 相机参数
```csharp
// 在CameraController3D组件中设置
ringCameraDistance = 20f;    // 相机距离
ringCameraHeight = 15f;      // 相机高度
enableAutoRotation = true;   // 自动旋转
ringRotationSpeed = 1f;      // 旋转速度
```

## 常见问题

### Q: 方块位置不正确
A: 检查GameGrid3D的enableRingMode是否设置为true

### Q: 相机视角不佳
A: 调整ringCameraDistance和ringCameraHeight参数

### Q: 游戏无法开始
A: 确保所有必要的管理器组件都已创建

### Q: 性能问题
A: 减少gridWidth或禁用自动旋转

## 调试技巧

1. 使用RingGridTester进行功能测试
2. 查看控制台输出的调试信息
3. 使用OnGUI界面进行实时调试
4. 按T键运行完整测试

## 扩展功能

### 自定义环形参数
```csharp
// 动态调整环形半径
gameGrid.ringRadius = 15f;

// 动态调整网格大小
gameGrid.gridWidth = 30;
gameGrid.gridHeight = 25;
```

### 自定义相机行为
```csharp
// 手动控制相机旋转
cameraController.SetRotation(45f);

// 切换自动旋转
cameraController.ToggleAutoRotation();
```

## 性能优化

1. 减少网格大小（gridWidth, gridHeight）
2. 禁用自动旋转
3. 减少网格可视化线条
4. 使用对象池管理方块

## 故障排除

### 错误信息
- "GameGrid3D not found"：需要创建GameGrid3D对象
- "CameraController3D not found"：需要为主相机添加CameraController3D组件
- "GameManager3D not found"：需要创建GameManager3D对象

### 解决方案
1. 使用RingGridSetup进行自动设置
2. 手动创建缺失的组件
3. 检查组件之间的引用关系
4. 查看控制台错误信息 