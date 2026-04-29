# G2 - 2D平台动作游戏

基于Unity 2022.3开发的2D平台动作游戏，包含完整的游戏循环、技能系统、背包合成管理、掉落权重系统和WebGL构建支持。

## 游戏概述

G2是一个2D平台动作游戏，玩家控制角色在平台上跳跃、战斗，使用技能击败敌人并收集掉落物品。采用MVC架构设计，具备完整的游戏状态管理和UI系统。

### 核心特性

- **玩家控制**：灵活的移动、跳跃和多种技能（5种技能 + ME道具机制）
- **敌人AI**：随机移动、自动转向、受击击退和掉落
- **技能系统**：冲刺、恢复、超级跳、强化攻击、射击
- **背包管理**：物品拾取、堆叠、排序、滚动查看、左/右键操作
- **合成系统**：两个同级物品合成更高级物品
- **ME道具**：特殊道具，持有时可无消耗使用技能
- **掉落系统**：基于技能解锁状态的动态权重掉落算法
- **状态管理**：开始/游戏状态切换、暂停、死亡处理
- **音频系统**：包含BGM和5种技能音效
- **对象池**：背包格子使用对象池管理，高效滚动

## 技术栈

- **引擎**: Unity 2022.3.62f2c1 (2D项目模板)
- **编程语言**: C#（含DOTween动画、TextMesh Pro文本渲染）
- **主要插件**: DOTween（动画系统）、TextMesh Pro（高级文本渲染）
- **架构模式**: MVC模式（分离Controller/Model/View）、单例模式、事件驱动
- **构建目标**: WebGL (960×600分辨率)

## 项目结构

```
G2/
├── Assets/
│   ├── Scenes/
│   │   ├── GameScene.unity        # 主游戏场景
│   │   └── UiScene.unity          # UI场景（叠加加载）
│   ├── Sprites/                   # 游戏逻辑脚本
│   │   ├── Player/
│   │   │   ├── Controller.cs      # 玩家输入与技能处理 (CharterController)
│   │   │   ├── Model.cs           # 玩家数据模型 (HP、技能解锁列表)
│   │   │   ├── View.cs            # 玩家视觉表现（面部方向、HP颜色）
│   │   │   └── Test.cs            # 废弃/实验性脚本
│   │   ├── Enemy/
│   │   │   └── EnemyController.cs # 敌人AI、受击、掉落权重算法
│   │   ├── Ui/
│   │   │   ├── UiManager.cs       # UI状态管理（背包开关）
│   │   │   ├── UiController.cs    # 开始/暂停/退出逻辑
│   │   │   └── UiView.cs          # UI视图（按钮注册、面板动画）
│   │   ├── Ui/Bag/
│   │   │   ├── BagManager.cs      # 背包数据管理（添加/删除/合成/排序/扩容）
│   │   │   ├── BagView.cs         # 背包视图更新
│   │   │   ├── BagContrtoller.cs  # 背包交互（拖拽、滚动、左/右键操作）
│   │   │   ├── CargoController.cs # 单个物品格交互（点击、悬浮提示）
│   │   │   ├── ObjectPoolController.cs # 物品格对象池（预创建24个）
│   │   │   └── Package Table.cs   # 物品配置表（ScriptableObject）
│   │   ├── GameManager.cs         # 游戏状态管理（单例、场景加载）
│   │   ├── CameraController.cs    # 摄像机跟随
│   │   ├── Bullet.cs              # 子弹逻辑
│   │   ├── Physices.cs            # 自定义物理（重力、地面/墙壁检测）
│   │   ├── SoundManager.cs        # 音频管理器（当前为桩代码）
│   │   └── Drops/
│   │       └── DropsController.cs # 掉落物（旋转动画、拾取）
│   ├── Perfabs/                   # 预制体文件夹
│   │   ├── Player.prefab          # 玩家预制体
│   │   ├── Face.prefab            # 角色面部预制体
│   │   ├── Enemy.prefab           # 敌人预制体
│   │   ├── Drops.prefab           # 掉落物预制体
│   │   ├── Bullet.prefab          # 子弹预制体
│   │   ├── Cargo.prefab           # 背包物品格预制体
│   │   ├── Descripition.prefab    # 物品悬浮提示预制体
│   │   ├── GetCargoPanel.prefab   # 拾取物品拖拽面板预制体
│   │   └── Trap.png               # 陷阱贴图
│   ├── Resources/
│   │   ├── Audio/                 # 音频资源
│   │   │   ├── kprtz-w52q9.mp3   # 背景音乐
│   │   │   ├── Skill_Dash.mp3
│   │   │   ├── Skill_Hit.mp3
│   │   │   ├── Skill_Jump.flac
│   │   │   ├── Skill_Restore.mp3
│   │   │   └── Skill_Shoot.mp3
│   │   ├── TableData/
│   │   │   └── PackageTable.asset # 物品配置数据表
│   │   ├── Tile/                  # 地图瓦片资源
│   │   ├── Skillicon/             # 技能图标
│   │   ├── Tips/                  # 游戏提示图
│   │   └── Player.png/PlayerFace.png/EnemyRect.png/MapTile.png
│   ├── Editor/
│   │   └── GMcommand.cs           # GM命令工具
│   ├── DOTween/                   # DOTween动画插件
│   ├── TextMesh Pro/              # TextMesh Pro资源
│   └── font/                      # 字体资源
├── ProjectSettings/
├── Packages/
└── WebGL/                         # WebGL构建输出
    ├── Build/
    ├── TemplateData/
    └── index.html
```

## 如何运行

### 在Unity编辑器中运行
1. 使用Unity 2022.3或更高版本打开项目
2. 打开 `Assets/Scenes/GameScene.unity`
3. 点击Play按钮（游戏启动时为暂停状态，点击"开始游戏"后开始）

### WebGL版本
项目已预构建WebGL版本，位于 `G2/WebGL/` 目录：
1. 打开 `WebGL/index.html` 在浏览器中运行
2. 或使用本地HTTP服务器（如Python的 `python -m http.server`）

### 重新构建WebGL
1. Unity编辑器：File → Build Settings
2. 选择WebGL平台
3. 点击Build，选择输出目录（推荐 `G2/WebGL/`）

## 游戏控制

### 基本操作
- **A/D 或 左右箭头**：水平移动
- **空格键**：跳跃
- **鼠标**：瞄准方向（影响射击方向）
- **Tab**：打开/关闭背包界面
- **Esc**：暂停/继续
- **R**：背包物品排序（同ID合并）

### 技能快捷键（数字键1-5）

| 按键 | 技能 | 效果 |
|------|------|------|
| 1 | 冲刺 (Dash) | 快速水平冲刺，可伤害敌人 |
| 2 | 恢复 (Restore) | 恢复1点生命值 |
| 3 | 超级跳 (Super Jump) | 更高跳跃 |
| 4 | 强化攻击 (Power Hit) | 5秒内攻击力提升至5（默认2） |
| 5 | 射击 (Shoot) | 向鼠标方向发射子弹（对敌人造成1伤害） |

### 技能解锁
- 初始解锁：冲刺（ID:0）、恢复（ID:1）
- 通过碰撞场景中 `skillCollider` 标签的触发器解锁新技能
- 每个触发器用名字表示技能ID，碰触即解锁

### ME道具机制
- 持有ME道具（物品ID:6）时，使用技能不消耗背包物品
- 用于测试和调试

## 背包系统

### 基本功能
- 敌人被击败后会掉落物品，触碰拾取
- 每个物品格最大堆叠64个
- 背包初始24格，满时自动扩容（+4格）

### 交互操作
- **左键点击**：拾取整个物品堆，再次点击放入空格或合并
- **右键点击**：拾取一半物品 / 每次放入1个
- **鼠标悬浮**：显示物品名称和描述
- **滚轮**：滚动背包列表（物品格无限滚动）

### 合成系统
- 在背包中将两个相同ID的物品放在前两格（索引0、1）
- 点击"Craft"按钮合成更高级物品（ID + 1）
- 需要目标技能已解锁

### 物品类型 (6种)
| ID | 名称 | 对应技能 |
|----|------|---------|
| 0 | 冲刺道具 | 冲刺 |
| 1 | 恢复道具 | 恢复 |
| 2 | 超级跳道具 | 超级跳 |
| 3 | 强化攻击道具 | 强化攻击 |
| 4 | 射击道具 | 射击 |
| 6 | ME道具 | 无消耗使用所有技能 |

## 战斗系统

### 玩家
- 初始HP：30
- 基础攻击力：2（Dash命中触发）
- 强化攻击力：5（技能4持续5秒）
- 受击无敌：0.2秒
- 受击时被击退

### 敌人
- HP：3
- 移动速度：2
- 随机左右徘徊，遇墙/悬空转向
- 受击击退，0.3秒受伤无敌
- 击败后延迟0.3秒生成掉落物

### 掉落权重算法
- 根据已解锁技能数量动态调整权重
- 解锁2个技能：权重偏向初始技能（7:3）
- 解锁5个技能：所有技能权重均等（1:1:1:1:1）
- 使用线性插值过渡

## 自定义物理系统 (`Physices.cs`)
- 使用自定义重力（-9.8），而非Unity默认重力
- 地面检测：左右脚各一条射线（向下0.5单位）
- 墙壁检测：上下各一条射线（左右0.5单位）
- 返回状态：-1（左/右触墙），0（悬空），1（右/左触墙），2（双侧触墙）

## 音频系统
- 包含BGM（kprtz-w52q9.mp3）和5种技能音效
- 当前 `SoundManager.PlaySound()` 为桩代码，尚未接入技能系统

## 开发工具

### GM命令（编辑器菜单）
Unity编辑器顶部菜单栏 `GMcmd` 提供以下调试命令：

- **读取表格**：输出PackageTable中所有物品数据
- **读取背包**：显示当前背包中所有物品ID和数量
- **添加背包**：添加30个冲刺道具（ID:0）
- **添加ME道具**：添加1个ME道具（ID:6，无消耗使用技能）
- **一键添加8格**：随机添加8格满堆叠物品
- **一键添加随机奇数格**：压力测试，添加大量物品到背包

### 物品配置表 (PackageTable)
- 基于ScriptableObject的配置系统
- 支持id、name、description、sprite字段
- 可通过 `CreateAssetMenu` 创建新配置表

## 已知问题

1. `Perfabs` 文件夹命名拼写错误（应为 `Prefabs`）
2. `Sprites` 文件夹实际存放C#脚本，命名不准确
3. `SoundManager` 的 `PlaySound` 方法为空，音效未接入游戏逻辑
4. 部分中文注释可能存在编码问题（GM命令菜单名等）

## 后续改进建议

1. **代码质量**
   - 统一命名规范（Perfabs→Prefabs，Sprites→Scripts）
   - 完成音频系统接入
   - 重构 `Phy.cs` 中物理常量的可配置性

2. **功能扩展**
   - 添加更多敌人类型和BOSS战
   - 实现存档系统（GameManager无持久化）
   - 添加更多关卡和地图
   - 实现完整的死亡处理逻辑（UiView已有`DeadPanel`）

3. **用户体验**
   - 添加技能冷却时间显示
   - 优化UI/UX设计
   - 添加游戏教程

## 许可证

本项目基于Unity标准资产和插件开发，具体许可证请参考各组件文档。

---

**项目状态**: 核心功能完整，包含创新性的背包合成系统和动态权重掉落，已成功构建WebGL版本。

**最后更新**: 2026-04-29
