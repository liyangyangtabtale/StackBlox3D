# Unity场景搭建指南

## 快速设置（推荐）

### 方法一：自动设置
1. 在场景中创建一个空的GameObject
2. 添加 `SceneSetup` 组件
3. 勾选 "Auto Setup On Start"
4. 运行游戏，所有组件将自动创建

### 方法二：手动设置
1. 右键点击 `SceneSetup` 组件
2. 选择 "Setup Scene"
3. 所有游戏组件将自动创建

## 详细手动设置步骤

### 1. 创建游戏管理器
```
1. 创建空GameObject，命名为"GameManager"
2. 添加 GameManager 组件
3. 设置参数：
   - Max Layers: 10
   - Blocks Per Ring: 20
   - Fall Speed: 1
   - Fast Fall Speed: 3
```

### 2. 创建圆柱形井
```
1. 创建空GameObject，命名为"CylindricalWell"
2. 添加 CylindricalWell 组件
3. 设置参数：
   - Radius: 5
   - Height: 2
   - Max Layers: 10
   - Blocks Per Ring: 20
4. 分配材质和预制体引用
```

### 3. 创建方块生成器
```
1. 创建空GameObject，命名为"BlockSpawner"
2. 添加 BlockSpawner 组件
3. 设置参数：
   - Spawn Height: 15
   - Spawn Radius: 5
   - Spawn Interval: 2
   - Queue Size: 3
4. 创建生成点和预览位置
```

### 4. 设置摄像机
```
1. 选择Main Camera
2. 添加 CameraController 组件
3. 设置参数：
   - Target Position: (0, 10, -15)
   - Target Rotation: (30, 0, 0)
   - Follow Target: CylindricalWell
   - Enable Screen Shake: true
```

### 5. 创建触摸控制器
```
1. 创建空GameObject，命名为"TouchController"
2. 添加 TouchController 组件
3. 设置参数：
   - Swipe Threshold: 50
   - Rotation Speed: 2
   - Tap Threshold: 0.2
```

### 6. 创建UI系统
```
1. 创建Canvas，命名为"GameCanvas"
2. 设置Canvas参数：
   - Render Mode: Screen Space Overlay
   - UI Scale Mode: Scale With Screen Size
3. 添加CanvasScaler和GraphicRaycaster组件
4. 创建UIManager子对象
```

### 7. 创建UI元素
```
1. 在Canvas下创建UI元素：
   - ScoreText (分数显示)
   - LevelText (等级显示)
   - LinesText (行数显示)
   - ComboText (连击显示)
   - GameOverPanel (游戏结束面板)
   - PausePanel (暂停面板)
   - 控制按钮 (旋转、快速下落、硬降)
```

### 8. 创建分数系统
```
1. 创建空GameObject，命名为"ScoreSystem"
2. 添加 ScoreSystem 组件
3. 设置分数参数
```

### 9. 创建游戏设置
```
1. 创建空GameObject，命名为"GameSettings"
2. 添加 GameSettings 组件
3. 配置各种游戏参数
```

## 预制体创建

### 创建方块预制体
```
1. 创建空GameObject，添加 BlockPrefabCreator 组件
2. 右键点击组件，选择 "Create Block Prefabs"
3. 这将创建：
   - Block.prefab (基础方块)
   - PreviewBlock.prefab (预览方块)
   - 各种俄罗斯方块预制体
```

### 创建材质
```
1. 右键点击 BlockPrefabCreator 组件
2. 选择 "Create Materials"
3. 这将创建7种不同颜色的方块材质
```

### 创建粒子特效
```
1. 右键点击 BlockPrefabCreator 组件
2. 选择 "Create Particle Effects"
3. 这将创建：
   - RingClearEffect.prefab (圆环消除特效)
   - VortexEffect.prefab (连锁消除特效)
```

## 组件连接

### 自动连接
使用 `SceneSetup` 组件的 "Connect Components" 功能会自动连接所有组件。

### 手动连接
```
1. GameManager 连接：
   - Well: CylindricalWell
   - Spawner: BlockSpawner
   - Camera Controller: CameraController

2. BlockSpawner 连接：
   - Well: CylindricalWell

3. TouchController 连接：
   - Well: CylindricalWell
   - Spawner: BlockSpawner
   - Camera Controller: CameraController

4. CameraController 连接：
   - Follow Target: CylindricalWell

5. 事件连接：
   - Well.OnRingCleared → Camera.OnRingCleared
   - Well.OnVortexCleared → Camera.OnVortexCleared
   - Well.OnWellFull → GameManager.GameOver
```

## 场景层级结构

```
Scene
├── GameManager
├── CylindricalWell
├── BlockSpawner
│   ├── SpawnPoint
│   ├── QueuePosition_0
│   ├── QueuePosition_1
│   └── QueuePosition_2
├── Main Camera (with CameraController)
├── TouchController
├── GameCanvas
│   ├── UIManager
│   ├── ScoreText
│   ├── LevelText
│   ├── LinesText
│   ├── ComboText
│   ├── GameOverPanel
│   ├── PausePanel
│   └── Control Buttons
├── ScoreSystem
└── GameSettings
```

## 测试和调试

### 运行测试
```
1. 确保所有组件都已正确连接
2. 点击Play按钮
3. 检查Console中的调试信息
4. 测试触摸控制和游戏逻辑
```

### 常见问题解决
```
1. 方块不显示：
   - 检查BlockPrefab是否正确设置
   - 检查材质是否正确分配

2. 触摸无响应：
   - 检查TouchController是否正确连接
   - 检查Canvas设置

3. UI不显示：
   - 检查Canvas的Render Mode
   - 检查UI元素的Active状态

4. 摄像机问题：
   - 检查CameraController的Follow Target
   - 检查摄像机位置和角度
```

## 优化建议

### 性能优化
```
1. 使用对象池管理方块
2. 优化粒子特效数量
3. 减少不必要的Update调用
4. 使用LOD系统
```

### 视觉优化
```
1. 添加后处理效果
2. 优化光照设置
3. 添加阴影效果
4. 使用更好的材质和纹理
```

### 移动端优化
```
1. 调整UI缩放
2. 优化触摸响应
3. 减少内存使用
4. 优化电池消耗
```

## 扩展功能

### 添加音效
```
1. 创建AudioManager组件
2. 添加音效文件
3. 在相应事件中播放音效
```

### 添加动画
```
1. 创建动画控制器
2. 添加方块放置动画
3. 添加消除动画
```

### 添加更多特效
```
1. 创建自定义粒子系统
2. 添加屏幕特效
3. 添加环境特效
```

---

**注意：** 确保在运行游戏前保存场景，并检查所有组件的连接是否正确。 