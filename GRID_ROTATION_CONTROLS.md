# 网格旋转控制系统

## 概述

这个3D俄罗斯方块游戏现在支持两种控制模式：
1. **传统模式**：移动和旋转方块
2. **网格旋转模式**：旋转整个环形网格来控制游戏

## 控制模式

### 网格旋转模式（默认）

在网格旋转模式下，玩家通过旋转环形网格来控制游戏：

#### 键盘控制
- **左右箭头键** 或 **A/D**：旋转网格左右
- **上下箭头键** 或 **W/S**：旋转网格上下（倾斜）
- **空格键**：硬降方块
- **C键**：保持方块
- **Tab键**：切换控制模式
- **ESC/P键**：暂停游戏

#### 鼠标控制
- **左键拖拽**：旋转网格（根据鼠标移动方向）

### 传统模式

在传统模式下，玩家直接控制方块：

#### 键盘控制
- **左右箭头键** 或 **A/D**：移动方块左右
- **上下箭头键** 或 **W/S**：移动方块上下/旋转
- **空格键**：硬降方块
- **C键**：保持方块
- **Tab键**：切换控制模式
- **ESC/P键**：暂停游戏

## 游戏机制

### 网格旋转模式特性

1. **自动下降**：方块会自动下降，玩家通过旋转网格来控制方块的位置
2. **网格旋转**：整个环形网格可以平滑旋转
3. **位置计算**：方块位置会根据网格旋转角度重新计算
4. **视觉反馈**：网格旋转时，所有已放置的方块会跟随移动

### 环形网格特性

1. **X轴环绕**：方块可以在环形网格的X轴上环绕
2. **平滑旋转**：网格旋转使用插值实现平滑效果
3. **位置同步**：方块的世界坐标和网格坐标保持同步

## 脚本组件

### 核心脚本

1. **InputManager3D.cs**
   - 处理输入事件
   - 支持两种控制模式
   - 提供模式切换功能

2. **GameGrid3D.cs**
   - 管理网格数据
   - 处理网格旋转
   - 更新方块位置

3. **GameManager3D.cs**
   - 游戏逻辑控制
   - 处理网格旋转输入
   - 管理游戏状态

### 辅助脚本

1. **GridRotationTester.cs**
   - 测试网格旋转功能
   - 提供调试信息
   - 交互式测试界面

2. **GridRotationSetup.cs**
   - 快速配置网格旋转模式
   - 自动设置所有相关组件
   - 提供便捷的设置选项

## 配置选项

### GameGrid3D 设置

```csharp
[Header("Grid Rotation Settings")]
public float rotationSpeed = 2f;        // 网格旋转速度
public bool enableGridRotation = true;  // 是否启用网格旋转
public float rotationSmoothness = 5f;   // 旋转平滑度
```

### InputManager3D 设置

```csharp
[Header("Input Settings")]
public float gridRotationSpeed = 2f;    // 网格旋转速度
public bool enableGridRotation = true;  // 是否启用网格旋转模式
```

### GameManager3D 设置

```csharp
[Header("Grid Rotation Settings")]
public bool enableGridRotationMode = true; // 是否启用网格旋转模式
```

## 使用方法

### 快速设置

1. 在场景中添加 `GridRotationSetup` 组件
2. 配置所需参数
3. 点击 "Setup Grid Rotation" 按钮
4. 游戏将自动配置为网格旋转模式

### 手动设置

1. 确保场景中有 `GameGrid3D`、`GameManager3D`、`InputManager3D` 组件
2. 设置 `GameGrid3D.enableGridRotation = true`
3. 设置 `GameManager3D.enableGridRotationMode = true`
4. 设置 `InputManager3D.enableGridRotation = true`

### 运行时切换

- 按 **Tab键** 在两种控制模式之间切换
- 使用 `GridRotationSetup.ToggleControlMode()` 方法
- 使用 `GameManager3D.ToggleGridRotationMode()` 方法

## 测试功能

### GridRotationTester

- **G键**：运行网格旋转测试
- **T键**：切换控制模式
- **R键**：重置网格旋转角度

### 测试界面

游戏运行时会在屏幕左上角显示测试界面，包含：
- 当前网格模式
- 网格旋转状态
- 控制模式
- 测试按钮

## 故障排除

### 常见问题

1. **网格不旋转**
   - 检查 `GameGrid3D.enableGridRotation` 是否为 true
   - 检查 `GameGrid3D.enableRingMode` 是否为 true
   - 确保输入事件正确绑定

2. **方块位置不正确**
   - 检查 `GridToWorldPosition` 和 `WorldToGridPosition` 方法
   - 确保网格旋转角度计算正确
   - 验证方块位置更新逻辑

3. **控制模式切换失败**
   - 检查所有相关组件的 enable 设置
   - 确保事件系统正常工作
   - 验证输入管理器配置

### 调试信息

启用调试日志来查看详细信息：
- 网格旋转角度
- 方块位置变化
- 输入事件处理
- 模式切换状态

## 性能优化

1. **旋转平滑度**：调整 `rotationSmoothness` 参数
2. **更新频率**：优化方块位置更新逻辑
3. **内存管理**：及时清理不需要的方块对象
4. **渲染优化**：使用对象池管理方块实例

## 扩展功能

### 可能的改进

1. **网格倾斜**：实现网格的上下倾斜功能
2. **多层网格**：支持多层环形网格
3. **动态半径**：支持动态调整环形半径
4. **特效系统**：添加旋转时的视觉特效
5. **音效系统**：添加旋转音效反馈

### 自定义控制

可以通过修改 `InputManager3D` 来添加自定义控制：
- 手柄支持
- 触摸控制
- 语音控制
- 手势识别 