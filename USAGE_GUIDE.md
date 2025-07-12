# 3D俄罗斯方块 - 网格旋转控制系统使用指南

## 快速开始

### 1. 场景设置

1. 打开Unity项目
2. 在场景中创建一个空的GameObject，命名为 "GameController"
3. 添加以下组件到GameController：
   - `GameGrid3D`
   - `GameManager3D`
   - `InputManager3D`
   - `UIManager3D`（可选）
   - `CameraController3D`

### 2. 快速配置

1. 创建一个空的GameObject，命名为 "GridRotationSetup"
2. 添加 `GridRotationSetup` 组件
3. 配置参数：
   - `enableRingMode = true`
   - `enableGridRotation = true`
   - `enableGridRotationMode = true`
4. 点击 "Setup Grid Rotation" 按钮

### 3. 运行游戏

1. 按Play按钮开始游戏
2. 使用左右箭头键旋转环形网格
3. 方块会自动下降，通过旋转网格来控制方块位置
4. 按Tab键可以在网格旋转模式和传统模式之间切换

## 详细配置

### GameGrid3D 配置

```csharp
// 网格设置
gridWidth = 20;           // 环形周长（格子数）
gridHeight = 20;          // 网格高度
ringRadius = 10f;         // 环形半径

// 旋转设置
enableGridRotation = true;    // 启用网格旋转
rotationSpeed = 2f;           // 旋转速度（度/次）
rotationSmoothness = 5f;      // 旋转平滑度
```

### InputManager3D 配置

```csharp
// 输入设置
enableGridRotation = true;    // 启用网格旋转模式
gridRotationSpeed = 2f;       // 网格旋转速度
moveRepeatDelay = 0.1f;       // 移动重复延迟
moveRepeatRate = 0.05f;       // 移动重复频率
```

### GameManager3D 配置

```csharp
// 游戏设置
enableGridRotationMode = true;    // 启用网格旋转模式
initialDropSpeed = 1.0f;          // 初始下降速度
speedIncreasePerLevel = 0.1f;     // 每级速度增加
linesPerLevel = 10;               // 每级所需行数
```

## 控制说明

### 网格旋转模式（默认）

#### 主要控制
- **左右箭头键** 或 **A/D**：旋转网格左右
- **上下箭头键** 或 **W/S**：旋转网格上下（倾斜）
- **空格键**：硬降方块
- **C键**：保持方块
- **Tab键**：切换控制模式
- **ESC/P键**：暂停游戏

#### 鼠标控制
- **左键拖拽**：根据鼠标移动方向旋转网格

### 传统模式

#### 主要控制
- **左右箭头键** 或 **A/D**：移动方块左右
- **上下箭头键** 或 **W/S**：移动方块上下/旋转
- **空格键**：硬降方块
- **C键**：保持方块
- **Tab键**：切换控制模式
- **ESC/P键**：暂停游戏

## 游戏玩法

### 网格旋转模式

1. **自动下降**：方块会自动下降，无需手动控制下降
2. **网格旋转**：通过旋转环形网格来控制方块的位置
3. **位置计算**：方块位置会根据网格旋转角度自动重新计算
4. **视觉反馈**：网格旋转时，所有已放置的方块会跟随移动

### 策略技巧

1. **预判位置**：通过旋转网格来预判方块的最佳落点
2. **快速旋转**：利用网格旋转的平滑性进行快速位置调整
3. **空间利用**：充分利用环形网格的360度空间
4. **连锁消除**：通过旋转网格来创造连锁消除的机会

## 测试功能

### GridRotationTester

在场景中添加 `GridRotationTester` 组件来获得测试功能：

#### 测试按键
- **G键**：运行完整的网格旋转测试
- **T键**：切换控制模式
- **R键**：重置网格旋转角度

#### 测试界面
游戏运行时会在屏幕左上角显示测试信息：
- 当前网格模式（环形/矩形）
- 网格旋转状态（启用/禁用）
- 当前旋转角度
- 是否正在旋转
- 当前控制模式

### 调试信息

启用Console窗口来查看详细的调试信息：
- 网格旋转角度变化
- 方块位置更新
- 输入事件处理
- 模式切换状态
- 错误和警告信息

## 故障排除

### 常见问题

#### 1. 网格不旋转
**症状**：按左右箭头键时网格没有反应

**解决方案**：
1. 检查 `GameGrid3D.enableGridRotation` 是否为 true
2. 检查 `GameGrid3D.enableRingMode` 是否为 true
3. 检查 `InputManager3D.enableGridRotation` 是否为 true
4. 检查 `GameManager3D.enableGridRotationMode` 是否为 true
5. 确保所有组件都正确连接

#### 2. 方块位置不正确
**症状**：方块显示位置与预期不符

**解决方案**：
1. 检查 `ringRadius` 设置是否合适
2. 验证 `gridWidth` 和 `gridHeight` 设置
3. 确保 `GridToWorldPosition` 方法计算正确
4. 检查方块的世界坐标更新逻辑

#### 3. 控制模式切换失败
**症状**：按Tab键无法切换控制模式

**解决方案**：
1. 检查所有相关组件的 enable 设置
2. 确保事件系统正常工作
3. 验证输入管理器配置
4. 检查Console中是否有错误信息

#### 4. 性能问题
**症状**：游戏运行缓慢或卡顿

**解决方案**：
1. 调整 `rotationSmoothness` 参数
2. 减少 `gridWidth` 和 `gridHeight`
3. 优化方块位置更新频率
4. 检查是否有内存泄漏

### 调试步骤

1. **启用调试日志**
   - 在Console窗口中查看详细日志
   - 关注错误和警告信息

2. **检查组件连接**
   - 确保所有必要的组件都已添加
   - 验证组件之间的引用关系

3. **测试基本功能**
   - 使用 `GridRotationTester` 进行基本测试
   - 逐步验证每个功能模块

4. **参数调整**
   - 尝试不同的参数设置
   - 找到最适合的配置

## 高级功能

### 自定义控制

可以通过修改 `InputManager3D` 来添加自定义控制：

```csharp
// 添加手柄支持
if (Input.GetJoystickButton(0, "Left"))
{
    OnInputReceived?.Invoke(InputAction.RotateGridLeft);
}

// 添加触摸控制
if (Input.touchCount > 0)
{
    Touch touch = Input.GetTouch(0);
    if (touch.deltaPosition.x > 10f)
    {
        OnInputReceived?.Invoke(InputAction.RotateGridRight);
    }
}
```

### 扩展功能

1. **网格倾斜**：实现网格的上下倾斜功能
2. **多层网格**：支持多层环形网格
3. **动态半径**：支持动态调整环形半径
4. **特效系统**：添加旋转时的视觉特效
5. **音效系统**：添加旋转音效反馈

## 性能优化建议

1. **旋转平滑度**：根据设备性能调整 `rotationSmoothness`
2. **更新频率**：优化方块位置更新逻辑
3. **内存管理**：及时清理不需要的方块对象
4. **渲染优化**：使用对象池管理方块实例
5. **LOD系统**：根据距离调整渲染细节

## 联系支持

如果遇到问题或需要帮助：
1. 查看Console窗口的调试信息
2. 检查本文档的故障排除部分
3. 使用 `GridRotationTester` 进行诊断
4. 查看 `GRID_ROTATION_CONTROLS.md` 获取技术细节 